using UnityEngine;

public class InGamePresenter : IPresenter
{
    private InGameModel inGameModel;
    private IInGameView inGameView;
    private PresenterChanger presenterChanger;
    private AudioSource micAudioSource;

    public InGamePresenter(InGameModel model, IInGameView view, PresenterChanger pChanger, AudioSource audioSource)
    {
        inGameModel = model;
        inGameView = view;
        presenterChanger = pChanger;
        micAudioSource = audioSource;
    }

    public void Initialize()
    {
        Debug.Log("InGamePresenter��������");
    }

    public void Show()
    {
        Debug.Log("�C���Q�[����ʂ�\��");
        inGameView.Show();
    }

    public void Hide()
    {
        Debug.Log("�C���Q�[����ʂ��\��");
        inGameView.Hide();
    }

    private void GetMicInput(string deviceName)
    {
        micAudioSource.clip = Microphone.Start(deviceName, true, 10, 44100);
    }
}
