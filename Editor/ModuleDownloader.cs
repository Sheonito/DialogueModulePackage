using System;
using System.Collections;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


namespace Aftertime.StorylineEngine.Editor
{
    public class ModuleDownloader : EditorWindow
    {
        private const string SAMPLE_DESC = "This package provides StorylineEngine sample to get started.";
        private const string MODULE_DESC = "Download the ModuleManager to enable additional modules.";

        private const float DEFAULT_WINDOW_WIDTH = 500;
        private const float DEFAULT_WINDOW_HEIGHT = 260;
        private const float MAX_WINDOW_HEIGHT = 600;
        private const int LOG_HEIGHT = 20;

        private string gitId = "";
        private string gitPassword = "";

        private string sampleLog;
        private string moduleLog;

        // 현재는 샘플 다운로드만 있음.
        // 하지만 샘플은 내부로 변경해서 아무 기능이 없음.
        // 모듈 다운로더로 변경 후 주석 해제 필요
        // [MenuItem("Storyline Engine/Module Downloader")]
        public static void Window()
        {
            EditorWindow window = GetWindow(typeof(ModuleDownloader));
            window.position = new Rect(Screen.width / 2, Screen.height / 2, DEFAULT_WINDOW_WIDTH,
                DEFAULT_WINDOW_HEIGHT);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            TitleSession();

            GUILayout.Space(10);

            GitLoginSession();

            GUILayout.Space(15);

            SampleSession();
            ModuleSession();

            AdjustWindowSize();
        }

        private void TitleSession()
        {
            GUILayout.Space(10);
            GUILayout.Label("Welcome to Custom Package Installer!", EditorStyles.boldLabel);
        }

        private void GitLoginSession()
        {
            GUILayout.Label("Git ID:", EditorStyles.label);
            gitId = GUILayout.TextField(gitId, GUILayout.Width(DEFAULT_WINDOW_WIDTH));

            GUILayout.Space(5);

            GUILayout.Label("Git Password:", EditorStyles.label);
            gitPassword = GUILayout.PasswordField(gitPassword, '*', GUILayout.Width(DEFAULT_WINDOW_WIDTH));
        }

        private void SampleSession()
        {
            GUILayout.Label(SAMPLE_DESC, EditorStyles.boldLabel);
            if (GUILayout.Button("Download Sample"))
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(DownloadSample());
            }

            LogSession(sampleLog);
        }

        private void ModuleSession()
        {
            // Module Manager 다운로드 버튼 & 로그 출력
            GUILayout.Label(MODULE_DESC, EditorStyles.boldLabel);
            if (GUILayout.Button("Download ModuleManager"))
            {
            }

            LogSession(moduleLog);
        }

        private void LogSession(string log)
        {
            GUILayout.Label(log, EditorStyles.wordWrappedLabel);

            if (string.IsNullOrEmpty(log) == false)
                GUILayout.Space(LOG_HEIGHT);
        }


        private void AdjustWindowSize()
        {
            int logCount = 0;
            bool existSampleLog = string.IsNullOrEmpty(sampleLog) == false;
            bool existModuleLog = string.IsNullOrEmpty(moduleLog) == false;
            if (existSampleLog)
                ++logCount;

            if (existModuleLog)
                ++logCount;

            float newHeight = DEFAULT_WINDOW_HEIGHT + (logCount) * LOG_HEIGHT;
            position = new Rect(position.x, position.y, position.width,
                Mathf.Clamp(newHeight, DEFAULT_WINDOW_HEIGHT, MAX_WINDOW_HEIGHT));
        }

        private IEnumerator DownloadSample()
        {
            string saveFolder = Application.dataPath;
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            string version = PlayerPrefs.GetString(EditorDefine.VERSION_KEY);
            string url =
                $"http://git.aftertime.co.kr/AfterTime/StorylineEngineSample/raw/branch/{version}/Assets/SamplePackage.unitypackage";
            UnityWebRequest request = UnityWebRequest.Get(url);
            string authHeader = "Basic " + Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes(gitId + ":" + gitPassword));
            request.SetRequestHeader("Authorization", authHeader);


            EditorUtility.DisplayProgressBar("Downloading", "Downloading Sample...", 0.5f);

            yield return request.SendWebRequest();

            try
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    string savePath = Path.Combine(saveFolder, "SamplePackage.unitypackage");
                    File.WriteAllBytes(savePath, request.downloadHandler.data);
                    sampleLog = "Downloaded sample successfully";
                    Debug.Log(sampleLog);


                    AssetDatabase.Refresh();
                    AssetDatabase.ImportPackage(savePath, true);
                    FocusOnDownloadedFile(savePath);
                }
                else
                {
                    sampleLog = "Failed to download sample: " + request.responseCode;
                    Debug.LogError(sampleLog);
                }
            }
            catch (Exception e)
            {
                sampleLog = e.Message;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void FocusOnDownloadedFile(string absolutePath)
        {
            string dataPath = Application.dataPath;
            string relativePath = absolutePath.Replace(dataPath, "Assets");
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(relativePath);
            if (asset != null)
            {
                Selection.activeObject = asset;
                EditorGUIUtility.PingObject(asset);
            }
        }
    }
}