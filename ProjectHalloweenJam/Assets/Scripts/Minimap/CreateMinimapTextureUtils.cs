using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CreateMinimapTextureUtils
{
    public static Sprite GetMinimapSprite(float size, LayerMask cullingMask)
    {
        Texture2D texture = CreateTexture2D(size, cullingMask);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        return sprite;
    }

    private static Texture2D CreateTexture2D(float size, LayerMask cullingMask)
    {
        Camera camera = new GameObject("ScreenshotCamera").AddComponent<Camera>();

        camera.orthographic = true;
        camera.transform.position = new Vector3(0, 0, -10f);
        camera.orthographicSize = size;
        camera.useOcclusionCulling = false;
        camera.cullingMask = cullingMask;

        Rect rect = new Rect((Vector2)camera.transform.position, new Vector2(3840, 2160));

        Texture2D texture = CreateTextureForCamera(camera, rect);
        return texture;
    }

    private static Texture2D CreateTextureForCamera(Camera camera, Rect pixelRect)
    {
        int width = Mathf.CeilToInt(pixelRect.width);
        int height = Mathf.CeilToInt(pixelRect.height);

        RenderTexture rt = new RenderTexture(
            width,
            height,
            GraphicsFormat.R8G8B8A8_SRGB,
            GraphicsFormat.None,
            1
            );
        camera.ResetAspect();

        Texture2D cameraTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        camera.targetTexture = rt;
        RenderTexture.active = rt;   

        camera.Render();

        cameraTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        cameraTexture.Apply();

        rt.Release(); 
        Object.DestroyImmediate(camera.gameObject);  

        return cameraTexture;
    }
}
