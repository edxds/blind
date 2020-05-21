using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : MonoBehaviour {
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    public IObservable<Unit> OnPlayClick => _playButton.OnClickAsObservable();
    public IObservable<Unit> OnSettingsClick => _settingsButton.OnClickAsObservable();
    public IObservable<Unit> OnExitClick => _exitButton.OnClickAsObservable();
}
