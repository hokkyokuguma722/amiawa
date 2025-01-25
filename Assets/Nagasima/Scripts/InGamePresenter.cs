using UniRx;
using UnityEngine;

public class InGamePresenter : IPresenter
{
    private InGameModel inGameModel;
    private IInGameView inGameView;
    private PresenterChanger presenterChanger;
    private AudioSource micAudioSource;
    private int pos = 0;
    private string myDevice;

    private CompositeDisposable compositeDisposable = new();

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

        string targetDevice = "";

        //TODO:あとでつかうかも
        // foreach (var device in Microphone.devices)
        // {
        //     Debug.Log($"Device Name: {device}");
        //     if (device.Contains(myDevice))
        //     {
        //         targetDevice = device;
        //     }
        // }

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MicInputStart("マイク配列 (Realtek(R) Audio)");
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    var volume = SoundCalcurater.CalculateAudioVolume(micAudioSource.clip, ref pos);

                    Debug.Log(volume);
                    var Spectrum = SoundCalcurater.AnalyzeSpectrum(micAudioSource);

                    inGameView.SetVoiceImage(1 + volume);
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Debug.Log("マイク配列 (Realtek(R) Audio)");
                    MicInputEnd(targetDevice);
                }
            })
            .AddTo(compositeDisposable);
    }

    public void Show()
    {
        inGameView.Show();
    }

    public void Hide()
    {
        inGameView.Hide();
    }

    private void MicInputStart(string deviceName)
    {
        micAudioSource.clip = Microphone.Start(deviceName, true, 10, 44100);
    }

    private void MicInputEnd(string deviceName)
    {
        Microphone.End(deviceName);
    }
}