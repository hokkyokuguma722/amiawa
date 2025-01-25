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
        Debug.Log("InGamePresenter��������");

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown("space"))
                {
                    Debug.Log("�X�y�[�X�L�[��������");
                    MicInputStart("Example");
                }

                if (Input.GetKey("spave"))
                {
                    Debug.Log("�X�y�[�X�L�[�������Ă���");
                    var volume = SoundCalcurater.CalculateAudioVolume(micAudioSource.clip, ref pos);
                    var Spectrum = SoundCalcurater.AnalyzeSpectrum(micAudioSource);


                }

                if (Input.GetKeyUp("space"))
                {
                    Debug.Log("�X�y�[�X�L�[�𗣂���");
                    MicInputEnd("Example");
                }
            })
            .AddTo(compositeDisposable);

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

    private void MicInputStart(string deviceName)
    {
        micAudioSource.clip = Microphone.Start(deviceName, true, 10, 44100);
    }

    private void MicInputEnd(string deviceName)
    {
        Microphone.End(deviceName);
    }


}
