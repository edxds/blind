using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class PlaygroundUIController : MonoBehaviour {
    public PlaygroundViewModel viewModel;

    public TextMeshProUGUI locationUpperTitle;
    public TextMeshProUGUI locationTitle;

    public TextMeshProUGUI goalTitle;

    public TextMeshProUGUI interactionUpperTitle;
    public TextMeshProUGUI interactionTitle;
    
    private void Start() {
        UpdateElementVisibilityFromObservable(locationTitle, viewModel.ShouldShowLocation);
        UpdateElementVisibilityFromObservable(interactionTitle, viewModel.ShouldShowInteraction);
        
        viewModel.CurrentRoomUpperTitle
            .Where(title => title != null)
            .Do(title => locationUpperTitle.text = title)
            .Subscribe()
            .AddTo(this);

        viewModel.CurrentRoomTitle
            .Where(title => title != null)
            .Do(title => locationTitle.text = title)
            .Subscribe()
            .AddTo(this);

        viewModel.CurrentInteractionUpperTitle
            .Where(title => title != null)
            .Do(title => interactionUpperTitle.text = title)
            .Subscribe()
            .AddTo(this);

        viewModel.CurrentInteractionTitle
            .Where(title => title != null)
            .Do(title => interactionTitle.text = title)
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
}
