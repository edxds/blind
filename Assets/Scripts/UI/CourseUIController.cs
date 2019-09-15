using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class CourseUIController : MonoBehaviour {
    public CourseState state;

    public CanvasGroup courseUIGroup;
    
    public TextMeshProUGUI locationUpperTitle;
    public TextMeshProUGUI locationTitle;

    public TextMeshProUGUI goalTitle;

    public TextMeshProUGUI interactionUpperTitle;
    public TextMeshProUGUI interactionTitle;
    
    private void Start() {
        state.OnGoalStart
            .Do(_ => UpdateCanvasGroupAlpha(courseUIGroup, 0))
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
