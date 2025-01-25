using UnityEngine;
using UnityEngine.UI;

public interface IInGameView : IView
{
    void SetVoiceImage(float scale);
}

public class InGameView : MonoBehaviour, IInGameView
{
    [SerializeField] private Image ingameImage;

    [SerializeField] private Image voiceRecorderImage;

    [SerializeField] private Image storyImage;

    [SerializeField] private Image VoiceImage;


    public void Initialize()
    {
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Frame(float framesize)
    {
        ingameImage.transform.localScale = Vector3.right * framesize;
        ingameImage.transform.localScale = Vector3.up * framesize;
    }

    public void SetVoiceImage(float scale)
    {
        VoiceImage.transform.localScale = new Vector3(1, scale, 1);
    }

    public void VoiceRecorder(float voiderecordersize)
    {
        voiceRecorderImage.transform.localScale = Vector3.right * voiderecordersize;
    }
}