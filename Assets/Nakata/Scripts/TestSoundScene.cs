using Nakata.PichTest;
using UnityEngine;

class TestSoundScene : MonoBehaviour
{
    private void Awake()
    {
    }

    private void Start()
    {
        var _audioSource = gameObject.AddComponent<AudioSource>();
        if (gameObject != null)
        {
            var hoge = gameObject.AddComponent<ExampleClass>();
            hoge.Initialize(_audioSource);
        }
    }
}