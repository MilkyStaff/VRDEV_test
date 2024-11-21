using Cysharp.Threading.Tasks;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Random = UnityEngine.Random;

public static class ResourseHelper
{
    public const string TempPreviewPath = @"Assets\!tempPreview\";

    public const int PreviewWidth = 512;
    public const int PreviewHeight = 256;

    //for test awaiting animation
    public const float minDelay = 2f;
    public const float maxDelay = 2f;

    public static async UniTask<Texture2D> GetTextureByUrl(string url, CancellationToken token)
    {
        var request = await UnityWebRequestTexture
                    .GetTexture(url)
                    .SendWebRequest()
                    .WithCancellation(token);
#if UNITY_EDITOR
        await UniTask.WaitForSeconds(Random.Range(minDelay, maxDelay));
#endif
        return (request.downloadHandler as DownloadHandlerTexture).texture;
    }

    public static void SaveImage(Texture2D image, string textureUrl)
    {
        try
        {
            File.WriteAllBytes(Path.Combine(TempPreviewPath, textureUrl), image.EncodeToPNG());
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static bool TryGetTexture(string fileName, int width, int height, out Texture2D result)
    {
        result = null;

        try
        {
            string path = Path.Combine(TempPreviewPath, fileName);

            if (File.Exists(path))
            {
                Texture2D texture = new Texture2D(width, height);
                byte[] bytes = File.ReadAllBytes(path);
                texture.LoadImage(bytes);
                result = texture;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }

        return result != null;
    }
}