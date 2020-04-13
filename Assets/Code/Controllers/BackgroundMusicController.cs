﻿using UnityEngine;


public class BackgroundMusicController : MonoBehaviour
{
    private AudioSource track;

    void Awake()
    {
        track = transform.gameObject.GetComponent<AudioSource>();
        track.playOnAwake = true;
        track.loop        = true;
        track.volume      = 0.50f;
        track.playOnAwake = false;
    }

    void OnEnable()
    {
        GameEventCenter.startNewGame.AddListener(StartTrack);
        GameEventCenter.restartGame.AddListener(RestartTrack);
        GameEventCenter.pauseGame.AddListener(PauseTrack);
        GameEventCenter.resumeGame.AddListener(ResumeTrack);
        GameEventCenter.winningScoreReached.AddListener(EndTrack);
    }
    void OnDisable()
    {
        GameEventCenter.startNewGame.RemoveListener(StartTrack);
        GameEventCenter.restartGame.RemoveListener(RestartTrack);
        GameEventCenter.pauseGame.RemoveListener(PauseTrack);
        GameEventCenter.resumeGame.RemoveListener(ResumeTrack);
        GameEventCenter.winningScoreReached.RemoveListener(EndTrack);
    }
    void StartTrack(GameSettings gameSettings)
    {
        track.volume = gameSettings.MusicVolume / 100.0f;
        track.Play();
    }
    void RestartTrack(string _)
    {
        track.Stop();
        track.Play();
    }
    void PauseTrack(RecordedScore _)
    {
        track.Pause();
    }
    void ResumeTrack(string _)
    {
        track.UnPause();
    }
    void EndTrack(RecordedScore _)
    {
        track.Stop();
    }
}