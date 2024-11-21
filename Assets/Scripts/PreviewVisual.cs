using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PreviewVisual : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private Sequence awaitingAnimation;

    public void Init(Action onPreviewClickAction)
    {
        button.onClick.AddListener(() => onPreviewClickAction());
        StartLoadingAnimation(() => image.transform.rotation = Quaternion.identity);
    }

    public void SetImage(Texture2D texture)
    {
        image.sprite = Sprite.Create(texture, new Rect(0, 0, ResourseHelper.PreviewWidth, ResourseHelper.PreviewHeight), new Vector2());
        awaitingAnimation?.Kill(true);
    }

    private void StartLoadingAnimation(Action callback)
    {
        awaitingAnimation = DOTween.Sequence();
        awaitingAnimation.Append(image.transform.DORotate(new Vector3(0, 0, -360), 2, RotateMode.FastBeyond360)
                                                .SetEase(Ease.InOutSine)
                                                .SetLoops(int.MaxValue, LoopType.Restart))
            .OnComplete(() => callback?.Invoke());
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
