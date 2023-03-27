using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryAudio : MonoBehaviour
{
    public AudioSource Track1;
    public AudioSource Track2;
    public AudioSource Track3;

    public int TrackSelector;
    public int TrackHistory;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        TrackSelector = Random.Range(0, 3);
        if (TrackSelector == 0)
        {
            Track1.Play();
            TrackHistory = 1;
        }
        else if (TrackSelector == 1)
        {
            Track2.Play();
            TrackHistory = 2;
        }
        else if (TrackSelector == 2)
        {
            Track3.Play();
            TrackHistory = 3;
        }
    }

    void Update()
    {
        if (Track1.isPlaying == false && Track2.isPlaying == false && Track3.isPlaying == false)
        {
            TrackSelector = Random.Range(0, 3);
            if (TrackSelector == 0 && TrackHistory != 1)
            {
                Track1.Play();
                TrackHistory = 1;
            }
            else if (TrackSelector == 1 && TrackHistory != 2)
            {
                Track2.Play();
                TrackHistory = 2;
            }
            else if (TrackSelector == 2 && TrackHistory != 3)
            {
                Track3.Play();
                TrackHistory = 3;
            }
        }
    }
}
