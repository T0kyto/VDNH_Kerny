using AwakeSolutions;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    [SerializeField] private AwakeMediaPlayer _contentPlayer;
    [SerializeField] private AlphaTransition _layerAlpha;

    public void Open(string folderName, string fileName)
    {
        _contentPlayer.Open(folderName,fileName);
    }

    public void StartFadeIn(float duration)
    {
        _layerAlpha.StartFadeIn(duration);
    }

    public void StartFadeOut(float duration)
    {
        _layerAlpha.StartFadeOut(duration);
    }

    public void SetOpaque()
    {
        _layerAlpha.SetOpaque();
    }

    public void SetTransparent()
    {
        _layerAlpha.SetTransparent();
    }

    public void Play()
    {
        _contentPlayer.Play();
    }
}
