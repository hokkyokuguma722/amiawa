using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public AudioSource audioSourceBGM;
    public AudioClip[] audioClipsBGM;

    public AudioSource audioSourceSE;
    public AudioClip[] audioClipsSE;

    public void PlayBGM(SceneType sceneType)
    {
        audioSourceBGM.Stop();
        switch(sceneType)
        {
            default:
            case SceneType.FristScene:
            case SceneType.SecondScene:
               audioSourceBGM.clip = audioClipsBGM[0];
                break;
            case SceneType.ThridScene:
                audioSourceBGM.clip = audioClipsBGM[1];
                break;
            case SceneType.ForthScene:
                audioSourceBGM.clip = audioClipsBGM[2];
                break;
        }
        audioSourceBGM.Play();
    }
    public void PalySE(int index)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[index]);
    }
}
