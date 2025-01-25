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

    public AudioSource audioSourceSE;
    public AudioClip[] audioCllipsSE;

    public void PalySE(int index)
    {
        audioSourceSE.PlayOneShot(audioCllipsSE[index]);
    }
}
