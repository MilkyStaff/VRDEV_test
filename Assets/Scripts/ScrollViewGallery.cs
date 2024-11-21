using Cysharp.Threading.Tasks;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static ResourseHelper;

public class ScrollViewGallery : MonoBehaviour
{
    private const float DEFAULT_PREVIEW_SIZE = 25;

    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private RectTransform scrollContentRoot;
    [SerializeField] private PreviewVisual simplePreviewPrefab;

    [SerializeField] private Text videoDescription;

    public void Init(List<JSONData> datas, MediaPlayer mediaPlayer)
    {
        SetVideo(mediaPlayer, datas[0].MainUrl, datas[0].Name);

        foreach (var data in datas)
            CreatePreview(mediaPlayer, data);

        SetContentSize(datas.Count);
    }

    private void SetContentSize(int count)
    {
        float offset = verticalLayoutGroup.padding.top + verticalLayoutGroup.padding.bottom;
        float height = count * (DEFAULT_PREVIEW_SIZE + verticalLayoutGroup.spacing) + offset;

        scrollContentRoot.sizeDelta = new Vector2(scrollContentRoot.sizeDelta.x, height);
    }

    private void CreatePreview(MediaPlayer mediaPlayer, JSONData data)
    {
        var visual = Instantiate(simplePreviewPrefab, scrollContentRoot);
        visual.Init(() => SetVideo(mediaPlayer, data.MainUrl, data.Name));
        SetPreviewImage(data.PreviewUrl,visual);
    }

    private async void SetPreviewImage(string url, PreviewVisual previewVisual)
    {
        string filename = Path.GetFileName(new Uri(url).AbsolutePath);

        if (!TryGetTexture(filename, PreviewWidth, PreviewHeight, out Texture2D texture))
        {
            var timeOutToken = new CancellationTokenSource();
            timeOutToken.CancelAfterSlim(TimeSpan.FromSeconds(5));
            var token = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy(), timeOutToken.Token);

            texture = await GetTextureByUrl(url, token.Token);
            SaveImage(texture, filename);
        }
        previewVisual.SetImage(texture);
    }

    private void SetVideo(MediaPlayer mediaPlayer, string mediaPath, string name, bool autoPlay = false )
    {
        if (mediaPlayer.MediaPath.Path != mediaPath)
        {
            mediaPlayer.OpenMedia(new MediaPath(mediaPath, MediaPathType.AbsolutePathOrURL), autoPlay);
            videoDescription.text = name;
        }
    }
}
