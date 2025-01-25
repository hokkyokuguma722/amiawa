using System;
using System.Linq;
using UnityEngine;

public static　class SoundCalcurater
{
    private static int AmpGain = 10;

    /// <summary>
    /// 音の大きさを返す
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="lastAudioPos"></param>
    /// <returns></returns>
    public static float CalculateAudioVolume(AudioClip clip, ref int lastAudioPos)
    {
        int nowAudioPos = Microphone.GetPosition(null); // nullでデフォルトデバイス

        float[] waveData = Array.Empty<float>();

        if (lastAudioPos < nowAudioPos)
        {
            int audioCount = nowAudioPos - lastAudioPos;
            waveData = new float[audioCount];
            clip.GetData(waveData, lastAudioPos);
        }
        else if (lastAudioPos > nowAudioPos)
        {
            int audioBuffer = clip.samples * clip.channels;
            int audioCount = audioBuffer - lastAudioPos;

            float[] wave1 = new float[audioCount];
            clip.GetData(wave1, lastAudioPos);

            float[] wave2 = new float[nowAudioPos];
            if (nowAudioPos != 0)
            {
                clip.GetData(wave2, 0);
            }

            waveData = new float[audioCount + nowAudioPos];
            wave1.CopyTo(waveData, 0);
            wave2.CopyTo(waveData, audioCount);
        }

        lastAudioPos = nowAudioPos;

        if (waveData.Length <= 0) return 0;

        var average = waveData.Average(Mathf.Abs);

        return 1 + average * AmpGain;
    }


    /// <summary>
    /// 音の高さを計算して返す
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    public static float AnalyzeSpectrum(AudioSource audioSource)
    {
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        int maxIndex = 0;
        float maxValue = 0.0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxValue)
            {
                maxValue = spectrum[i];
                maxIndex = i;
            }
        }

        float freq = maxIndex * AudioSettings.outputSampleRate / 2 / spectrum.Length;
        Debug.Log($"最大周波数は={freq}Hzです");

        return freq;
    }
}