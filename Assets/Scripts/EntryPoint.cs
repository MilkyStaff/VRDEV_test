using RenderHeads.Media.AVProVideo;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private const string jsonPath = @"Assets\!jsonData\";

    [SerializeField] private ScrollViewGallery scrollviewGallery;
    [SerializeField] private PlayButton playButton;
    [SerializeField] private MediaPlayer mediaPlayer;

    private void Start()
    {
        if (!TryGetJsonData(jsonPath, out List<JSONData> datas))
        {
            Debug.LogWarning($"Cant get JSONData by path {jsonPath}");
            return;
        }

        scrollviewGallery.Init(datas, mediaPlayer);

        mediaPlayer.Events.AddListener(OnMediaPlayerEvent);

        playButton.button.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnPlayButtonClick()
    {
        if (mediaPlayer.Control.IsPlaying())
            mediaPlayer.Pause();
        else
            mediaPlayer.Play();
    }

    private void OnMediaPlayerEvent(MediaPlayer mediaPlayer, MediaPlayerEvent.EventType eventType, ErrorCode errorCode)
    {
        switch (eventType)
        {
            case MediaPlayerEvent.EventType.Paused:
            case MediaPlayerEvent.EventType.FinishedPlaying:
                playButton.ChangeImage(false);
                break;
            case MediaPlayerEvent.EventType.Unpaused:
                playButton.ChangeImage(true);
                break;
        }
    }

    private bool TryGetJsonData(string path, out List<JSONData> datas)
    {
        datas = new List<JSONData>();

        var files = Directory.GetFiles(path, "*.json");
        foreach (string filePath in files)
        {
            string txt = File.ReadAllText(filePath);
            JSONData data = JsonUtility.FromJson<JSONData>(txt);
            datas.Add(data);
        }

        return datas.Count > 0;
    }
    private void OnDestroy()
    {
        mediaPlayer.Events.RemoveListener(OnMediaPlayerEvent);
        playButton.button.onClick.RemoveListener(OnPlayButtonClick);
    }
}
