using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class CourseUIController : MonoBehaviour {
    private IInputProvider _inputProvider;
    
    public CourseState state;

    public CanvasGroup courseUIGroup;
    public CanvasGroup courseFinishedUIGroup;
    
    public TextMeshProUGUI locationUpperTitle;
    public TextMeshProUGUI locationTitle;

    public TextMeshProUGUI goalTitle;

    public TextMeshProUGUI interactionUpperTitle;
    public TextMeshProUGUI interactionTitle;

    public Button mainMenuButton;
    public Button exitButton;
    
    [Inject]
    private void Init(IInputProvider inputProvider) {
        _inputProvider = inputProvider;
    }
    
    private void Start() {
        state.OnMainMenuClick
            .Do(OnMainMenuClicked)
            .Subscribe()
            .AddTo(this);

        state.OnExitClick
            .Do(OnExitClicked)
            .Subscribe()
            .AddTo(this);
        
        state.OnGoalStart
            .Do(_ => UpdateCanvasGroupAlpha(courseUIGroup, 0))
            .Subscribe()
            .AddTo(this);

        state.OnGoalFinish
            .Do(
                _ => {
                    _inputProvider.UnlockCursor();
                    
                    courseFinishedUIGroup.interactable = true;
                    mainMenuButton.Select();
                    
                    UpdateCanvasGroupAlpha(courseFinishedUIGroup, 1);
                }
            )
            .Subscribe()
            .AddTo(this);
        
        UpdateElementVisibilityFromObservable(locationTitle, state.ShouldShowLocation);
        UpdateElementVisibilityFromObservable(interactionTitle, state.ShouldShowInteraction);

        UpdateElementTextFromObservable(locationUpperTitle, state.CurrentRoomUpperTitle);
        UpdateElementTextFromObservable(locationTitle, state.CurrentRoomTitle);
        UpdateElementTextFromObservable(interactionUpperTitle, state.CurrentInteractionUpperTitle);
        UpdateElementTextFromObservable(interactionTitle, state.CurrentInteractionTitle);
        UpdateElementTextFromObservable(goalTitle, state.CurrentGoalTitle);
    }

    private void OnMainMenuClicked(Unit _) {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnExitClicked(Unit _) {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
    
    private void UpdateElementTextFromObservable(TMP_Text element, IObservable<string> observable) {
        observable
            .Where(title => title != null)
            .Do(title => element.text = title)
            .Subscribe()
            .AddTo(this);
    }
    
    private void UpdateElementVisibilityFromObservable(Component element, IObservable<bool> observable) {
        observable
            .Select((shouldShow, index) => {
                var isFirst = index == 0;
                return (shouldShow, isFirst);
            })
            .Do(tuple => {
                var (shouldShow, isFirst) = tuple;
                UpdateElementVisibility(element, shouldShow, !isFirst);
            })
            .Subscribe()
            .AddTo(this);
    }
    
    private void UpdateElementVisibility(Component element, bool shouldShow, bool shouldAnimate) {
        var fade = element.GetComponentInParent<CanvasGroupFade>();
        var targetAlpha = shouldShow ? 1 : 0; 
        fade.FadeTo(targetAlpha, shouldAnimate);
    }

    private void UpdateCanvasGroupAlpha(CanvasGroup group, float target) {
        var fade = group.GetComponent<CanvasGroupFade>();
        fade.FadeTo(target, true);
    }
}
