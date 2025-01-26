using UnityEngine;
using UnityEngine.UI;

public interface IInGameView : IView
{
    void UpdateSpeechBubbleImage(float scale, float spectrum);
    public void UpdateVoiceRecorderImage(float scale);
    public void UpdateMouthImage(float scale);
    public void UpdateNeedleImage(Quaternion quaternion);
    public void SetSpeechBubbleImage(Vector3 position);
    public void UpdateMaicImage(bool isSpeech);
    public RectTransform GetSpeechBubbleTransform();
    public void SetGameResultPerformance(bool isClear);
    public void SetNeedleImage();
    public void ChangeSceneImage();
}

public class InGameView : MonoBehaviour, IInGameView
{
    [SerializeField] private Image[] sceneImages;
    [SerializeField] private Image voiceRecorderImage;
    [SerializeField] private Image mouthImage;
    [SerializeField] private Image speechBubbleImage;
    [SerializeField] private Sprite[] speechBubbleSprites;
    [SerializeField] private GameObject needleImage;
    [SerializeField] private GameObject arrowIndicaterImage;
    [SerializeField] private Image maicImage;
    [SerializeField] private Sprite[] maicSprite;
    [SerializeField] private GameObject[] menFaceImageObjects;
    [SerializeField] private GameObject[] womenFaceImageObjects;


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
        voiceRecorderImage.rectTransform.localScale = new Vector3(scale, 1, 1);
    }

    /// <summary>
    /// マイクのマークを変更する
    /// </summary>
    /// <param name="isSpeech"></param>
    public void UpdateMaicImage(bool isSpeech)
    {
        maicImage.sprite = isSpeech ? maicSprite[0] : maicSprite[1];
    }

    /// <summary>
    /// 吹き出しのサイズを調整
    /// </summary>
    /// <param name="scale"></param>
    public void UpdateSpeechBubbleImage(float scale, float spectrum)
    {
        //小数点第二以下を切り捨て
        var scaleFloor = Mathf.Floor(scale * 10f) / 10f;

        //ラープ 現在地　目標値　補完族度

        //大きさを変形
        speechBubbleImage.transform.localScale = new Vector3(
            1 + Mathf.Lerp(speechBubbleImage.transform.localScale.x, scale, 0.1f),
            1 + Mathf.Lerp(speechBubbleImage.transform.localScale.x, scale, 0.1f),
            1);

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
        // mouthImage.transform.localScale = new Vector3(1 + scale, 1 + scale, 1);
    }

    /// <summary>
    /// 針の振れの動き
    /// </summary>
    /// <param name="volume"></param>
    public void UpdateNeedleImage(Quaternion quaternion)
    {
        // 針を回転
        needleImage.transform.rotation = quaternion;
    }

    /// <summary>
    /// 吹き出しの位置を指定
    /// </summary>
    /// <param name="position"></param>
    public void SetSpeechBubbleImage(Vector3 position)
    {
        speechBubbleImage.rectTransform.position = position;
    }

    public RectTransform GetSpeechBubbleTransform()
    {
        return speechBubbleImage.rectTransform;
    }

    /// <summary>
    /// 最初の場面を切り替える
    /// </summary>
    public void ChangeSceneImage()
    {
        sceneImages[0].gameObject.SetActive(false);
        sceneImages[1].gameObject.SetActive(true);
    }

    /// <summary>
    /// ゲームクリア演出
    /// </summary>
    /// <param name="isClear"></param>
    public void SetGameResultPerformance(bool isClear)
    {
        sceneImages[1].gameObject.SetActive(false);
        sceneImages[2].gameObject.SetActive(isClear);
        sceneImages[3].gameObject.SetActive(!isClear);
    }

    /// <summary>
    /// 針を出す
    /// </summary>
    /// <param name="isReady"></param>
    public void SetNeedleImage()
    {
        needleImage.gameObject.SetActive(true);
        arrowIndicaterImage.SetActive(true);
    }
}