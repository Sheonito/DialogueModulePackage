using Lucecita.HappinessBlossom.UI;
using UnityEngine;

public static class SpriteRendererUtil
{
    public static void CalculateScale(SpriteRenderer sr)
    {
        // 1. 스케일 초기화
        sr.transform.localScale = Vector3.one;

        // 2. 스프라이트 원본 크기 (scale=1 기준, world 단위)
        float spriteWidth = sr.sprite.rect.width / sr.sprite.pixelsPerUnit;
        float spriteHeight = sr.sprite.rect.height / sr.sprite.pixelsPerUnit;

        // 3. 카메라와 스프라이트 사이 거리
        float distance = Mathf.Abs(sr.transform.position.z - Camera.main.transform.position.z);

        // 4. 카메라 월드 크기 (Perspective)
        float fovRad = Camera.main.fieldOfView * Mathf.Deg2Rad;
        float worldScreenHeight = 2f * distance * Mathf.Tan(fovRad / 2f);
        float worldScreenWidth = worldScreenHeight * Camera.main.aspect;

        // 5. 스케일 계산
        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        float scale = Mathf.Max(scaleX, scaleY); // BG: 꽉 채우기

        sr.transform.localScale = new Vector3(scale, scale, 1);
    }
}
