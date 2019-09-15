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
}
