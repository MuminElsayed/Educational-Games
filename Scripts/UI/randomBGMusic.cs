using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomBGMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] BGMusic;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        AudioClip music = BGMusic[UnityEngine.Random.Range(0, BGMusic.Length)];
        source.clip = music;
        source.Play();
    }
}
