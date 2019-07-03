using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public List<AudioClip> bgm;
    public List<AudioClip> se;
    public List<AudioClip> talkSE;

    public AudioSource BGMAudio;
    public AudioSource SEAudio;

    private float lowPitchRange = 0.95f;
    private float highPitchRange = 1.05f;

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

    public void TalkSE()
    {
        int i = Random.Range(0, talkSE.Count);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        SEAudio.pitch = randomPitch;
        SEAudio.clip = talkSE[i];
        SEAudio.Play();
    }
}
