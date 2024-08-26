using System;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private Camera mainCamera;

    private PlayerController playerController;
    private VideoPlayer videoPlayer;

    public delegate void VideoEndCallback();
    private VideoEndCallback onVideoEndCallback;

    private void Awake()
    {
        Instance = this;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void PlayCutscene(VideoClip videoClip, VideoEndCallback callback = null)
    {
        videoPlayer.enabled = true;
        playerController.canMove = false;
        playerController.canMoveCam = false;
        crosshair.SetActive(false);
        videoPlayer.clip = videoClip;

        onVideoEndCallback = callback;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.enabled = false;
        crosshair.SetActive(true);

        playerController.canMove = true;
        playerController.canMoveCam = true;

        onVideoEndCallback?.Invoke();
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}