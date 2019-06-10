using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM { };
public enum SE { };

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public List<AudioClip> bgm;
    public List<AudioClip> se;

    public AudioSource BGMAudio;
    public AudioSource SEAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void playSE(SE clip)
    {
        SEAudio.PlayOneShot(se[(int)clip]);
    }

    public void talkSE()
    {
        int i = Random.Range(0, 3);
        SEAudio.PlayOneShot(se[i]);
    }
}
