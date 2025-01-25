using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HoloLens 2 で音声録音を行うサンプル
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class RecorderSample : MonoBehaviour
{
    // オーディオクリップの再生を行うための AudioSource
    private AudioSource audioSource;

    // 録音したデータを保存する AudioClip
    private AudioClip audioClip;

    // 録音したデータをトリミングした AudioClip
    private AudioClip audioClip2;

    // 録音を終了した際のサンプル位置
    private int position;

    [SerializeField] private Button _button;


    void Start()
    {
        // 同一オブジェクトにアタッチされている AudioSource を取得
        audioSource = GetComponent<AudioSource>();
        _button.onClick.AddListener(delegate { recording(); });
    }

    private void recording()
    {
        if (!Microphone.IsRecording(Microphone.devices[0]))
        {
            startRecord();
        }
        else
        {
            stopRecord();
        }
    }


    /// <summary>
    /// レコーディングを開始する
    /// </summary>
    private void startRecord()
    {
        // 録音の開始
        audioClip = Microphone.Start(Microphone.devices[0], false, 60, 44100);
    }

    /// <summary>
    /// レコーディングを終了する
    /// </summary>
    private void stopRecord()
    {
        // 録音を終了した際のサンプリングの数 (終了位置)
        position = Microphone.GetPosition(Microphone.devices[0]);
        Microphone.End(Microphone.devices[0]);

        // Microphone.Start で録音したデータのサンプルを取得する
        // まずサンプル数と同じ要素数のfloat配列を初期化する
        // サンプルのデータ数は audioClip.samples * audioClip.channels で計算 
        float[] audioData = new float[audioClip.samples * audioClip.channels];
        // audioClip.GetData で audioData の配列要素数だけサンプルを取得する
        // 第二引数はオフセットサンプル数
        // (1秒後から取得したい場合は、サンプリング周波数×1秒で 44100 が第二引数となる)
        audioClip.GetData(audioData, 0);

        // audioData から録音終了時までのサンプルデータを抜き出す
        // まずサンプルデータを保存する配列を初期化する
        float[] audioData2 = new float[position * audioClip.channels];

        // audioData2 配列の要素数だけ audioData のデータをコピーする
        for (var i = 0; i < audioData2.Length; i++)
        {
            audioData2[i] = audioData[i];
        }

        // 新しい AudioClip を生成する
        audioClip2 = AudioClip.Create("audioClip2", position, audioClip.channels, 44100, false);
        // 生成した AudioClip にサンプルデータを格納する
        audioClip2.SetData(audioData2, 0);

        // 生成した AudioClip2 を再生する
        audioSource.clip = audioClip2;
        Play();

        Debug.Log($"音の長さは={audioClip2.length}");
    }

    /// <summary>
    /// AudioSource にセットされている AudioClip を再生する
    private void Play()
    {
        audioSource.Play();
    }
}