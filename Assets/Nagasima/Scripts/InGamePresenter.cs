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
        Debug.Log("InGamePresenterを初期化");
    }

    public void Show()
    {
        Debug.Log("インゲーム画面を表示");
        inGameView.Show();
    }

    public void Hide()
    {
        Debug.Log("インゲーム画面を非表示");
        inGameView.Hide();
    }

    private void GetMicInput(string deviceName)
    {
        micAudioSource.clip = Microphone.Start(deviceName, true, 10, 44100);
    }
}
