using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    private bool isInputting = false;

    string targetDevice = "";

    private const float NeedleSpeed = 25.0f;
    private const float Speed = 1.0f;

    private float currentAngle;
    private bool isIncreasing = false;
    private const float MaxAngle = 45;
    private readonly Vector3 TargetPoint = new Vector3(292, -83, 0);

    private bool isStopped = false;
    private const float MinBubbleAngle = 30.0f; //成功判定角度
    private const float MaxBubbleAngle = 60.0f; //成功判定角度
    private const float ClearStopDistance = 10.0f;
    private const float FailedStopDistance = 800f;

    private bool isTransforming = false;
    private bool isClear = false;

    private readonly CompositeDisposable compositeDisposableBUbble = new();
    private readonly CompositeDisposable compositeDisposableNeedle = new();

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

        SetObservableUpdateBubble();
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

    private Quaternion GetQuaternion()
    {
        float volume = NeedleSpeed * Time.deltaTime;

        // 角度を更新
        if (isIncreasing)
        {
            currentAngle += volume;
            if (currentAngle >= MaxAngle)
            {
                currentAngle = MaxAngle;
                isIncreasing = false; // 減少に切り替え
            }
        }
        else
        {
            currentAngle -= volume;
            if (currentAngle <= -MaxAngle)
            {
                currentAngle = -MaxAngle;
                isIncreasing = true; // 増加に切り替え
            }
        }

        return Quaternion.Euler(0, 0, currentAngle);
    }

    private void SetObservableUpdateBubble()
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                inGameView.UpdateMaicImage(isInputting);
                inGameView.SetNeedleImage();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MicInputStart(null);
                    isInputting = true;
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    var volume = SoundCalcurater.CalculateAudioVolume(micAudioSource.clip, ref pos);
                    inGameView.UpdateVoiceRecorderImage(volume);
                    inGameView.UpdateMouthImage(volume);

                    Debug.Log(volume);
                    var spectrum = SoundCalcurater.AnalyzeSpectrum(micAudioSource);

                    inGameView.UpdateSpeechBubbleImage(1 + volume, spectrum);
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Debug.Log("マイク配列 (Realtek(R) Audio)");
                    isInputting = false;
                    MicInputEnd(targetDevice);
                    SetObjectVariableUpdateNeedle();
                    compositeDisposableBUbble.Dispose();
                }
            })
            .AddTo(compositeDisposableBUbble);
    }

    private Quaternion finalQuaternion;
    private Vector3 initialSpeechBubblePosition;
    private const float Distance = 20; //クリア判定をとる処理


    private void SetObjectVariableUpdateNeedle()
    {
        initialSpeechBubblePosition = inGameView.GetSpeechBubbleTransform().position;

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                inGameView.UpdateNeedleImage(GetQuaternion());

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    finalQuaternion = GetQuaternion();
                    Hoge().Forget();
                    compositeDisposableNeedle.Dispose();
                }
            })
            .AddTo(compositeDisposableNeedle);
    }

    private async UniTask Hoge()
    {
        // 点Aから円Bの中心への方向ベクトル
        Vector3 direction = (TargetPoint - initialSpeechBubblePosition).normalized;

        // 円Bに接する点Cを計算
        Vector3 pointC = TargetPoint + direction * Distance;

        // ベクトルABとACを計算
        Vector3 vectorAB = TargetPoint - initialSpeechBubblePosition;
        Vector3 vectorAC = pointC - initialSpeechBubblePosition;

        // 角度CABを計算
        //半分にしたのが判定用の角度
        float angleCAB = Vector3.Angle(vectorAB, vectorAC);

        float judgeAngleCAB = angleCAB / 2;
        //場所の計算
        //クリア
        //吹き出しの移動
        if (finalQuaternion.eulerAngles.z < judgeAngleCAB || finalQuaternion.eulerAngles.z > -judgeAngleCAB)
        {
            var movePosition = pointC;
            await inGameView.GetSpeechBubbleTransform().DOAnchorPos(movePosition, 10).AsyncWaitForCompletion();
        }
        else
        {
            // 角度をラジアンに変換
            float angleInRadians = (finalQuaternion * Vector3.right).z * Mathf.Deg2Rad;

            // 新しい点の位置を計算
            Vector3 movePosition = new Vector3(
                initialSpeechBubblePosition.x + FailedStopDistance * Mathf.Cos(angleInRadians),
                initialSpeechBubblePosition.y + FailedStopDistance * Mathf.Sin(angleInRadians),
                initialSpeechBubblePosition.z // 2Dの場合はzをそのまま維持
            );

            await inGameView.GetSpeechBubbleTransform().DOAnchorPos(movePosition, 10).AsyncWaitForCompletion();
        }

        await UniTask.WaitForSeconds(2);

        inGameView.SetGameResultPerformance(isClear);
    }
}