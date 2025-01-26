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
    private bool isInputting = false;
    private float currentAngle;
    private bool isIncreasing = false;
    private bool isClear = false;
    private Quaternion finalQuaternion;
    private Vector3 initialSpeechBubblePosition;

    private const float NeedleSpeed = 25.0f;
    private const float MaxAngle = 45;
    private const float FailedStopDistance = 800f;
    private const float Radius = 200; //クリア判定をとる処理
    private readonly Vector3 TargetPoint = new Vector3(292, -83, 0);

    private readonly CompositeDisposable compositeDisposableBUbble = new();
    private readonly CompositeDisposable compositeDisposableNeedle = new();
    private readonly CompositeDisposable disposables = new CompositeDisposable();

    public InGamePresenter(InGameModel model, IInGameView view, PresenterChanger pChanger, AudioSource audioSource)
    {
        inGameModel = model;
        inGameView = view;
        presenterChanger = pChanger;
        micAudioSource = audioSource;
    }

    public async void Initialize()
    {
        Debug.Log("InGamePresenter��������");
        SoundManager.instance.PlayBGM(SceneType.FristScene);

        await UniTask.WaitForSeconds(0.7f).SuppressCancellationThrow();

        inGameView.ChangeSceneImage();

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

    private void SetReactiveProperty()
    {
        inGameModel.currentSceneType
            .Subscribe(type => { SoundManager.instance.PlayBGM(type); })
            .AddTo(disposables);
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
                    MicInputEnd(null);
                    SetObjectVariableUpdateNeedle();
                    compositeDisposableBUbble.Dispose();
                }
            })
            .AddTo(compositeDisposableBUbble);
    }

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
                    SoundManager.instance.PalySE(2);
                }
            })
            .AddTo(compositeDisposableNeedle);
    }

    private async UniTask Hoge()
    {
        // 点Aから円Bの中心への方向ベクトル
        Vector3 direction = (TargetPoint - initialSpeechBubblePosition);

        // 点Aと円Bの中心の距離
        float distanceOA = direction.magnitude;

        // 点Aが円の外側にあるか確認
        if (distanceOA <= Radius)
        {
            Debug.LogError("点Aは円の内側または円周上にあります。接点を計算できません。");
            return;
        }

        // 接点までの比率を計算
        float ratio = Radius / distanceOA;

        // 接点Cを計算
        Vector3 pointC = TargetPoint + direction.normalized * Radius;

        // ベクトルABとACを計算
        Vector3 vectorAC_ = -pointC - initialSpeechBubblePosition;
        Vector3 vectorAC = pointC - initialSpeechBubblePosition;

        // 角度CABを計算
        //半分にしたのが判定用の角度
        float angleCAC = Vector3.Angle(vectorAC_, vectorAC);

        float judgeAngleCAB = angleCAC / 2;
        //場所の計算
        //クリア
        SoundManager.instance.PalySE(5);

        //吹き出しの移動
        if (finalQuaternion.eulerAngles.z < judgeAngleCAB && finalQuaternion.eulerAngles.z > -judgeAngleCAB)
        {
            if (finalQuaternion.eulerAngles.z < 0)
            {
                pointC *= -1;
            }

            await inGameView.GetSpeechBubbleTransform().DOAnchorPos(pointC, 10).AsyncWaitForCompletion();
            SoundManager.instance.PalySE(3);
        }
        else
        {
            // 角度をラジアンに変換
            float angleInRadians = (finalQuaternion * Vector3.right).z * Mathf.Deg2Rad;

            int normal = 1;
            if (finalQuaternion.z < 0) normal = -1;

            // 新しい点の位置を計算
            Vector3 movePosition = new Vector3(
                initialSpeechBubblePosition.x + FailedStopDistance * Mathf.Cos(angleInRadians),
                initialSpeechBubblePosition.y + FailedStopDistance * Mathf.Sin(angleInRadians) * normal,
                initialSpeechBubblePosition.z // 2Dの場合はzをそのまま維持
            );


            //吹き出しの移動
            await inGameView.GetSpeechBubbleTransform().DOAnchorPos(movePosition, 1f).AsyncWaitForCompletion();

            SoundManager.instance.PalySE(4);
        }

        await UniTask.WaitForSeconds(1.5f);

        inGameModel.currentSceneType.Value = isClear ? SceneType.SecondScene : SceneType.ForthScene;
        inGameView.SetGameResultPerformance(isClear);

        await UniTask.WaitForSeconds(1f);

        presenterChanger.ChangePresenter("GameResultsPresenter");
    }
}