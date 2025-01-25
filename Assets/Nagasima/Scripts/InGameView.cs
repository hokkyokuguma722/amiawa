using UnityEngine;
using UnityEngine.UI;

public interface IInGameView : IView
{
    void UpdateSpeechBubbleImage(float scale, float spectrum);
    public void UpdateVoiceRecorderImage(float scale);
    public void UpdateMouthImage(float scale);
    public void UpdateNeedleImage(float volume);
}

public class InGameView : MonoBehaviour, IInGameView
{
    [SerializeField] private Image[] sceneImages;
    [SerializeField] private Image voiceRecorderImage;
    [SerializeField] private Image mouthImage;
    [SerializeField] private Image speechBubbleImage;
    [SerializeField] private Sprite[] speechBubbleSprites;
    [SerializeField] private GameObject needleImage;

    private const float MaxAngle = 45.0f; // 最大角度（度数法）
    private float currentAngle = 0.0f; // 現在の角度
    private bool isIncreasing = true; // 角度が増加中かどうか

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

    /// <summary>
    /// ボイスの音量を可視化するView
    /// </summary>
    /// <param name="scale"></param>
    public void UpdateVoiceRecorderImage(float scale)
    {
        voiceRecorderImage.transform.localScale = new Vector3(scale, 1, 1);
    }

    /// <summary>
    /// 吹き出しのサイズを調整
    /// </summary>
    /// <param name="scale"></param>
    public void UpdateSpeechBubbleImage(float scale, float spectrum)
    {
        //大きさを変形
        speechBubbleImage.transform.localScale = new Vector3(1, scale, 1);


        //TODO:数値は仮
        //形状を変化
        if (spectrum > 2)
        {
            speechBubbleImage.sprite = speechBubbleSprites[0];
        }
        else if (spectrum > 1)
        {
            speechBubbleImage.sprite = speechBubbleSprites[1];
        }
        else
        {
            speechBubbleImage.sprite = speechBubbleSprites[2];
        }
    }

    /// <summary>
    /// キャラクターの口の動き
    /// </summary>
    /// <param name="scale"></param>
    public void UpdateMouthImage(float scale)
    {
        voiceRecorderImage.transform.localScale = new Vector3(1 + scale, 1 + scale, 1);
    }

    /// <summary>
    /// 針の振れの動き
    /// </summary>
    /// <param name="volume"></param>
    public void UpdateNeedleImage(float volume)
    {
        void Update()
        {
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
                if (currentAngle <= 0)
                {
                    currentAngle = 0;
                    isIncreasing = true; // 増加に切り替え
                }
            }

            // 針を回転
            needleImage.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }
}