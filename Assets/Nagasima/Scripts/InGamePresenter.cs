using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class InGamePresenter : IPresenter
{
    private InGameModel inGameModel;
    private IInGameView inGameView;
    private PresenterChanger presenterChanger;
    private AudioSource micAudioSource;
    private int pos = 0;

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
        Debug.Log("InGamePresenterを初期化");

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown("space"))
                {
                    Debug.Log("スペースキーを押した");
                    MicInputStart("Example");
                }

                if (Input.GetKey("spave"))
                {
                    Debug.Log("スペースキーを押している");
                    var volume = SoundCalcurater.CalculateAudioVolume(micAudioSource.clip, ref pos);
                    var Spectrum = SoundCalcurater.AnalyzeSpectrum(micAudioSource);


                }

                if (Input.GetKeyUp("space"))
                {
                    Debug.Log("スペースキーを離した");
                    MicInputEnd("Example");
                }
            })
            .AddTo(compositeDisposable);

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

    private void MicInputStart(string deviceName)
    {
        micAudioSource.clip = Microphone.Start(deviceName, true, 10, 44100);
    }

    private void MicInputEnd(string deviceName)
    {
        Microphone.End(deviceName);
    }


}
