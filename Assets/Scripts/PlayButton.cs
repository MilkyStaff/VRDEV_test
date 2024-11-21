using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] private Image image;

    [SerializeField] private Sprite playTexture;
    [SerializeField] private Sprite pauseTexture;

    private void Start()
    {
        image.sprite = playTexture;
    }

    public void ChangeImage(bool isPlaying)
    {
        image.sprite = isPlaying ? pauseTexture: playTexture;
    }
}
