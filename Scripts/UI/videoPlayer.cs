using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;

public class videoPlayer : MonoBehaviour
{
    private UnityEngine.Video.VideoPlayer vidPlayer;
    void OnEnable()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        vidPlayer = GetComponentInChildren<UnityEngine.Video.VideoPlayer>();
        vidPlayer.url = Path.Combine(Application.streamingAssetsPath, "shark video.mp4");
        vidPlayer.isLooping = true;
        vidPlayer.Prepare();
        while (!vidPlayer.isPrepared)
        {
            yield return null;
        }
        vidPlayer.Play();
    }
}
