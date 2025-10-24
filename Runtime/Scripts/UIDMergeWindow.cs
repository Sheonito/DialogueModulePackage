#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Aftertime.StorylineEngine;
using UnityEditor;
using UnityEngine;

public class UIDMergeWindow : EditorWindow
{
    // 이전 버전의 텍스트 요소 리스트
    private IReadOnlyList<TextElement> _previousElements;

    // 현재 버전의 텍스트 요소 리스트
    private IReadOnlyList<TextElement> _currentElements;

    // 스크롤 위치
    private Vector2 _scrollPosition;

    // 텍스트 버튼 스타일
    private GUIStyle _textButtonStyle;

    // 포트 위치 저장 (왼쪽/오른쪽)
    private Dictionary<int, Rect> _leftPortRects = new();
    private Dictionary<int, Rect> _rightPortRects = new();

    // UID가 연결된 포트 쌍 (왼쪽 index → 오른쪽 index)
    private Dictionary<int, int> _linkedPortPairs = new();

    // 포트 드래그 관련 변수
    private bool _isDraggingPort = false;
    private bool _isDraggingFromLeft = false;
    private int _draggingIndex = -1;
    private int _draggingControlID = -1;

    // GUI 상수
    private const float PortYOffset = 30f;
    private const float LineHeight = 35f;

    private bool _disableAllCurrent = false;
    private HashSet<int> _disabledCurrentIndices = new();

    public void ShowWindow(IReadOnlyList<TextElement> prevElements, IReadOnlyList<TextElement> curElements,
        string episodeTitle)
    {
        var window = CreateInstance<UIDMergeWindow>();
        window.SetData(prevElements, curElements);
        window.titleContent = new GUIContent($"UID Merge Tool: {episodeTitle}");
        window.Show();
    }

    private void SetData(IReadOnlyList<TextElement> prev, IReadOnlyList<TextElement> curr)
    {
        _previousElements = prev;
        _currentElements = curr;
    }

    private void InitStyles()
    {
        if (_textButtonStyle == null)
        {
            _textButtonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(10, 10, 0, 0),
                wordWrap = false,
                normal = { textColor = GUI.skin.label.normal.textColor },
                active = { textColor = GUI.skin.label.normal.textColor },
                focused = { textColor = GUI.skin.label.normal.textColor },
                hover = { textColor = GUI.skin.label.normal.textColor },
            };
        }
    }

    private void OnGUI()
    {
        InitStyles();

        float portSize = 20f;
        float portGap = 40f;
        float leftColumnWidth = (position.width - portSize * 2 - portGap) / 2;
        float rightColumnWidth = leftColumnWidth;

        // 헤더 표시
        GUI.BeginGroup(new Rect(0, 0, position.width, position.height));

        GUI.Label(new Rect(10, 5, leftColumnWidth, 25), "Previous Elements", EditorStyles.boldLabel);

        // 텍스트 버튼 스타일의 padding과 동일하게 설정
        float labelStartX = position.width - rightColumnWidth + 10; // +10은 _textButtonStyle.padding.left과 맞춤
        float checkboxSize = 20f;

        bool prevDisableAll = _disableAllCurrent; // 이전 상태 저장

        // 변경무시 체크박스
        _disableAllCurrent = GUI.Toggle(
            new Rect(labelStartX - checkboxSize - 5, 5, checkboxSize, checkboxSize),
            _disableAllCurrent, GUIContent.none
        );

        // ✅ 사용자가 토글을 껐을 때 모든 상태 초기화
        if (prevDisableAll && !_disableAllCurrent)
        {
            _disabledCurrentIndices.Clear(); // 개별 상태 전부 리셋
        }

        // 라벨
        GUI.Label(
            new Rect(labelStartX, 5, rightColumnWidth, 25),
            "Current Elements",
            EditorStyles.boldLabel
        );
        GUI.EndGroup();

        // 스크롤 뷰 시작
        _scrollPosition = GUI.BeginScrollView(
            new Rect(0, 30, position.width, position.height - 90),
            _scrollPosition,
            new Rect(0, 0, position.width, _currentElements.Count * LineHeight)
        );

        List<float> highlightLineYs = new();
        _leftPortRects.Clear();
        _rightPortRects.Clear();

        for (int i = 0; i < Mathf.Max(_previousElements.Count, _currentElements.Count); i++)
        {
            bool wasDisabled = false;
            float y = i * LineHeight;

            // Rect 배치 계산
            Rect prevButtonRect = new Rect(0, y, leftColumnWidth, LineHeight);
            Rect leftPortRect = new Rect(prevButtonRect.xMax, y + 7f, portSize, portSize);
            Rect rightPortRect = new Rect(prevButtonRect.xMax + portSize + portGap, y + 7f, portSize, portSize);
            Rect currButtonRect = new Rect(rightPortRect.xMax, y, rightColumnWidth, LineHeight);

            // 텍스트 요소 가져오기
            TextElement prev = i < _previousElements.Count ? _previousElements[i] : new TextElement();
            TextElement curr = i < _currentElements.Count ? _currentElements[i] : new TextElement();

            bool portConnected = _linkedPortPairs.TryGetValue(i, out int rightIndex) &&
                                 rightIndex < _currentElements.Count &&
                                 _previousElements[i].Uid == _currentElements[rightIndex].Uid;

            // 변경 여부 판단
            bool isChanged = prev.IsTextChanged;
            if (isChanged && !portConnected)
                highlightLineYs.Add(y);

            Color backupColor = GUI.backgroundColor;
            GUI.backgroundColor = isChanged && !portConnected ? new Color(1f, 0.8f, 0.8f) : Color.white;

            if (!string.IsNullOrEmpty(prev.Text))
                GUI.Button(prevButtonRect, prev.Text, _textButtonStyle);

            GUI.backgroundColor = backupColor;

            // 변경된 항목만 왼쪽 포트 표시
            if (isChanged)
                DrawPort(i, leftPortRect, true);
            if (!_disableAllCurrent && !_disabledCurrentIndices.Contains(i))
                DrawPort(i, rightPortRect, false);

            if (!string.IsNullOrEmpty(curr.Text))
            {
                bool isDisabled = _disableAllCurrent || _disabledCurrentIndices.Contains(i);

                // 회색 처리 여부에 따라 버튼 색상 설정
                GUI.backgroundColor = isDisabled ? new Color(0.6f, 0.6f, 0.6f) : Color.white;

                // 버튼 클릭 처리
                if (GUI.Button(currButtonRect, curr.Text, _textButtonStyle))
                {
                    if (_disableAllCurrent)
                    {
                        // ✅ 토글 끄고 클릭한 것만 비활성화 해제
                        _disableAllCurrent = false;

                        _disabledCurrentIndices.Clear();
                        for (int j = 0; j < _currentElements.Count; j++)
                        {
                            if (j != i)
                                _disabledCurrentIndices.Add(j); // 클릭한 것만 빼고 전부 회색 유지
                        }
                    }
                    else
                    {
                        // ✅ 기존 토글 동작
                        if (_disabledCurrentIndices.Contains(i))
                            _disabledCurrentIndices.Remove(i);
                        else
                            _disabledCurrentIndices.Add(i);
                    }
                }

                // 색상 복원
                GUI.backgroundColor = backupColor;
            }
        }

        GUI.EndScrollView();

        // 기타 처리
        DrawStaticEdges();
        HandleEdgeDragging();
        DrawScrollIndicator(highlightLineYs, _currentElements.Count * LineHeight);

        if (_isDraggingPort)
            Repaint();

        // Merge 버튼 활성화 조건
        bool allCurrDisabled = _disableAllCurrent || _disabledCurrentIndices.Count == _currentElements.Count;
        bool allChangedPrevConnected = true;

        for (int i = 0; i < _previousElements.Count; i++)
        {
            if (_previousElements[i].IsTextChanged)
            {
                if (!_linkedPortPairs.TryGetValue(i, out int rightIndex))
                {
                    allChangedPrevConnected = false;
                    break;
                }

                if (rightIndex >= _currentElements.Count ||
                    _previousElements[i].Uid != _currentElements[rightIndex].Uid)
                {
                    allChangedPrevConnected = false;
                    break;
                }
            }
        }

        GUI.enabled = allCurrDisabled || allChangedPrevConnected;

        // Merge 버튼
        if (GUI.Button(new Rect(position.width / 2 - 100, position.height - 50, 200, 40), "Merge & Save"))
        {
            foreach (var curr in _currentElements)
            {
                if (string.IsNullOrEmpty(curr.Uid))
                    curr.CreateUid();
            }

            Debug.Log("Merge 완료. UID가 없는 항목은 새로 생성됨.");
            Close();
        }
    }

    private void DrawPort(int index, Rect rect, bool isLeft)
    {
        // 포트 UI 그리기
        EditorGUI.DrawRect(rect, Color.black);
        EditorGUI.DrawRect(new Rect(rect.x + rect.width / 4, rect.y + rect.height / 4, rect.width / 2, rect.height / 2),
            Color.white);

        Event e = Event.current;
        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        // 드래그 시작
        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            GUIUtility.hotControl = controlId;
            _draggingControlID = controlId;
            _isDraggingPort = true;
            _isDraggingFromLeft = isLeft;
            _draggingIndex = index;

            // 포트 연결 해제
            if (isLeft)
                _linkedPortPairs.Remove(index);
            else
            {
                foreach (var pair in _linkedPortPairs)
                {
                    if (pair.Value == index)
                    {
                        _linkedPortPairs.Remove(pair.Key);
                        break;
                    }
                }
            }

            e.Use();
        }

        // 위치 기록
        if (isLeft) _leftPortRects[index] = rect;
        else _rightPortRects[index] = rect;
    }

    private void HandleEdgeDragging()
    {
        if (!_isDraggingPort || _draggingIndex < 0) return;

        Event e = Event.current;
        Vector2 from, to;

        if (_isDraggingFromLeft && _leftPortRects.TryGetValue(_draggingIndex, out Rect fromRect))
            from = fromRect.center;
        else if (!_isDraggingFromLeft && _rightPortRects.TryGetValue(_draggingIndex, out Rect fromRect2))
            from = fromRect2.center;
        else return;

        from.y -= _scrollPosition.y;
        from.y += PortYOffset;
        to = e.mousePosition;

        // 드래그 라인 표시
        Handles.BeginGUI();
        Handles.DrawBezier(from, to, from + Vector2.right * 50, to + Vector2.left * 50, Color.cyan, null, 3f);
        Handles.EndGUI();

        // 드래그 종료
        if (e.type == EventType.MouseUp && GUIUtility.hotControl == _draggingControlID)
        {
            GUIUtility.hotControl = 0;
            int targetIndex = -1;

            if (_isDraggingFromLeft)
            {
                foreach (var pair in _rightPortRects)
                {
                    Rect port = pair.Value;
                    port.y -= _scrollPosition.y;
                    port.y += PortYOffset;
                    if (port.Contains(e.mousePosition))
                    {
                        targetIndex = pair.Key;
                        break;
                    }
                }

                if (targetIndex != -1 && _draggingIndex < _previousElements.Count &&
                    targetIndex < _currentElements.Count)
                {
                    string newUid = _previousElements[_draggingIndex].Uid;
                    _currentElements[targetIndex].SetUid(newUid);
                    _linkedPortPairs[_draggingIndex] = targetIndex;
                }
            }
            else
            {
                foreach (var pair in _leftPortRects)
                {
                    Rect port = pair.Value;
                    port.y -= _scrollPosition.y;
                    port.y += PortYOffset;
                    if (port.Contains(e.mousePosition))
                    {
                        targetIndex = pair.Key;
                        break;
                    }
                }

                if (targetIndex != -1 && _draggingIndex < _currentElements.Count &&
                    targetIndex < _previousElements.Count)
                {
                    string newUid = _previousElements[targetIndex].Uid;
                    _currentElements[_draggingIndex].SetUid(newUid);
                    _linkedPortPairs[targetIndex] = _draggingIndex;
                }
            }

            _isDraggingPort = false;
            _draggingIndex = -1;
            GUI.changed = true;
        }
    }

    private void DrawStaticEdges()
    {
        Handles.BeginGUI();
        foreach (var pair in _linkedPortPairs)
        {
            if (_leftPortRects.TryGetValue(pair.Key, out Rect leftRect) &&
                _rightPortRects.TryGetValue(pair.Value, out Rect rightRect))
            {
                Vector2 from = leftRect.center;
                Vector2 to = rightRect.center;

                from.y -= _scrollPosition.y;
                from.y += PortYOffset;

                to.y -= _scrollPosition.y;
                to.y += PortYOffset;

                Handles.DrawBezier(from, to, from + Vector2.right * 50, to + Vector2.left * 50, Color.green, null, 2f);
            }
        }

        Handles.EndGUI();
    }

    private void DrawScrollIndicator(List<float> highlightYs, float totalHeight)
    {
        totalHeight -= 30;
        float indicatorWidth = 10f;
        float viewportHeight = position.height - 90f;
        float indicatorHeight = viewportHeight;

        Rect indicatorRect = new Rect(position.width - indicatorWidth, 0, indicatorWidth, indicatorHeight);
        GUI.BeginGroup(indicatorRect);

        foreach (float y in highlightYs)
        {
            float normalizedY = y / totalHeight;
            float drawY = (normalizedY * indicatorHeight) + 30;
            EditorGUI.DrawRect(new Rect(0, drawY, indicatorWidth, 4f), Color.red);
        }

        GUI.EndGroup();
    }
}
#endif