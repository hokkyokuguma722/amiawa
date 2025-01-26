using UnityEngine;

namespace Nakata.PichTest
{
    public class ExampleClass : MonoBehaviour
    {
        [SerializeField] private string m_DeviceName = "マイク配列 (Realtek(R) Audio)";

        private AudioSource _audioSource;

        // private const string DeviceName = "ヘッドセット (WF-C500)";
        // private const string PCDevice = "マイク配列 (Realtek(R) Audio)";
        private bool _isRecording = false;

        public void Initialize(AudioSource source)
        {
            _audioSource = source;

            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartRecording();
            }

            if (_isRecording)
            {
                AnalyzeSpectrum();
            }
        }

        private void StartRecording()
        {
            string targetDevice = "";

            // マイクデバイスが存在するか確認
            foreach (var device in Microphone.devices)
            {
                Debug.Log($"Device Name: {device}");
                if (device.Contains(m_DeviceName))
                {
                    targetDevice = device;
                }
            }

            Debug.Log($"=== Device Set: {targetDevice} ===");
            _audioSource.clip = Microphone.Start(
                null,
                true,
                3,
                48000);
            
            _isRecording = true;
        }

        //TODO:値を返すように変更
        private void AnalyzeSpectrum()
        {
            float[] spectrum = new float[256];
            _audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

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
        }
    }
}