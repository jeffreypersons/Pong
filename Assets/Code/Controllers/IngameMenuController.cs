﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IngameMenuController : MonoBehaviour
{
    public GameObject ingameMenu;
    public TMPro.TextMeshProUGUI title;
    public TMPro.TextMeshProUGUI subtitle;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button restartButton;
    public Button quitButton;

    private List<Button> buttonsToHideWhenActive;
    private List<TMPro.TextMeshProUGUI> labelsToHideWhenActive;
    private List<SpriteRenderer> spritesToHideWhenActive;

    void Awake()
    {
        ingameMenu.SetActive(false);  // ensures that the fetched objects are OUTSIDE the ingame menu
        buttonsToHideWhenActive = GameObjectUtils.FindAllObjectsWithTags<Button>("Button");
        labelsToHideWhenActive  = GameObjectUtils.FindAllObjectsWithTags<TMPro.TextMeshProUGUI>("Label");
        spritesToHideWhenActive = GameObjectUtils.FindAllObjectsWithTags<SpriteRenderer>("Ball", "Paddle", "GuidingLine");

        GameEventCenter.pauseGame.AddListener(OpenAsPauseMenu);
        GameEventCenter.winningScoreReached.AddListener(OpenAsEndGameMenu);
    }
    void OnDestroy()
    {
        GameEventCenter.pauseGame.RemoveListener(OpenAsPauseMenu);
        GameEventCenter.winningScoreReached.RemoveListener(OpenAsEndGameMenu);
    }

    void OnEnable()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(MoveToMainMenu);
        restartButton.onClick.AddListener(TriggerRestartGameEvent);
        quitButton.onClick.AddListener(SceneUtils.QuitGame);
    }
    void OnDisable()
    {
        resumeButton.onClick.RemoveListener(ResumeGame);
        mainMenuButton.onClick.RemoveListener(MoveToMainMenu);
        restartButton.onClick.RemoveListener(TriggerRestartGameEvent);
        quitButton.onClick.RemoveListener(SceneUtils.QuitGame);
    }
    private void ToggleMenuEnable(bool enableMenu)
    {
        Time.timeScale = enableMenu? 0 : 1;
        ingameMenu.SetActive(enableMenu);

        bool hideBackground = !enableMenu;
        for (int i = 0; i < buttonsToHideWhenActive.Count; i++)
        {
            buttonsToHideWhenActive[i].gameObject.SetActive(hideBackground);
            buttonsToHideWhenActive[i].enabled = hideBackground;
        }
        for (int i = 0; i < labelsToHideWhenActive.Count; i++)
        {
            labelsToHideWhenActive[i].enabled = hideBackground;
        }
        for (int i = 0; i < spritesToHideWhenActive.Count; i++)
        {
            spritesToHideWhenActive[i].enabled = hideBackground;
        }
    }

    private void OpenAsPauseMenu(RecordedScore recordedScore)
    {
        title.text    = "Game Paused";
        subtitle.text = recordedScore.LeftPlayerScore.ToString() + " - " + recordedScore.RightPlayerScore.ToString();

        GameObjectUtils.SetButtonVisibility(resumeButton, true);
        ToggleMenuEnable(true);
    }
    private void OpenAsEndGameMenu(RecordedScore recordedScore)
    {
        title.text    = recordedScore.IsLeftPlayerWinning() ? "Game Won" : "Game Lost";
        subtitle.text = recordedScore.LeftPlayerScore.ToString() + " - " + recordedScore.RightPlayerScore.ToString();

        GameObjectUtils.SetButtonVisibility(resumeButton, false);
        ToggleMenuEnable(true);
    }

    private void ResumeGame()
    {
        GameEventCenter.resumeGame.Trigger("Resuming game");
        ToggleMenuEnable(false);
    }
    private void MoveToMainMenu()
    {
        GameEventCenter.gotoMainMenu.Trigger("Opening main menu");
        Time.timeScale = 1;
        SceneUtils.LoadScene("MainMenu");
    }
    private void TriggerRestartGameEvent()
    {
        GameEventCenter.restartGame.Trigger("Restarting game");
        ToggleMenuEnable(false);
    }
}