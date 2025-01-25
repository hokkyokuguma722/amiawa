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
    private readonly Vector3 TargetPoint = new Vector3(337, -64, 0);

    private bool isStopped = false;
    private const float MinBubbleAngle = 30.0f; //成功判定角度
    private const float MaxBubbleAngle = 60.0f; //成功判定角度
    private const float ClearStopDistance = 10.0f;
    private const float FailedStopDistance = 800f;

    private bool isTransforming = false;
    private bool isClear = false;

    private CompositeDisposable compositeDisposableBUbble = new();
    private CompositeDisposable compositeDisposableNeedle = new();

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

    private void SetObjectVariableUpdateNeedle()
    {
        var initialSpeechBubblePosition = inGameView.GetSpeechBubbleTransform().position;

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (!isTransforming)
                {
                    inGameView.UpdateNeedleImage(GetQuaternion());

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        isTransforming = true;
                    }
                }
                else
                {
                    BubbleTransform(initialSpeechBubblePosition);

                    if (isStopped)
                    {
                        Debug.Log("クリア");
                        inGameView.SetGameResultPerformance(isClear);
                        compositeDisposableNeedle.Dispose();
                    }
                }
            })
            .AddTo(compositeDisposableNeedle);
    }

    private void BubbleTransform(Vector3 position)
    {
        if (isStopped) return; // 静止中は処理をスキップ

        // 現在の角度を取得（オブジェクトのz軸方向を基準とする）
        float angle = Vector3.Angle(Vector3.right, inGameView.GetSpeechBubbleTransform().right);

        //失敗したパターン
        if (angle is < MinBubbleAngle or > MaxBubbleAngle)
        {
            // 範囲外: 現在の方向に直進
            inGameView.SetSpeechBubbleImage(Vector3.right * Speed * Time.deltaTime);
            float distance = Vector3.Distance(inGameView.GetSpeechBubbleTransform().position, position);

            if (distance > FailedStopDistance)
            {
                isStopped = true;
                isClear = false;
            }
        }
        else　//成功したパターン
        {
            // 範囲内: 目標点への距離を計算
            float distanceToTarget = Vector3.Distance(inGameView.GetSpeechBubbleTransform().position, TargetPoint);

            if (distanceToTarget > ClearStopDistance)
            {
                // 目標点に向かって移動
                Vector3 direction = (TargetPoint - inGameView.GetSpeechBubbleTransform().position).normalized;
                inGameView.SetSpeechBubbleImage(direction * Speed * Time.deltaTime);
            }
            else
            {
                // 目標点付近で静止
                isStopped = true;
                isClear = true;
                Debug.Log("目標点付近で静止しました。");
            }
        }
    }
}