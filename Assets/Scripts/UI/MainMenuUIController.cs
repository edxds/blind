using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour {
    [SerializeField] private MainMenuState _state;
    
    private void Start() {
        _state.OnPlayClick
            .Do(OnPlayClick)
            .Subscribe()
            .AddTo(this);

        _state.OnSettingsClick
            .Do(OnSettingsClick)
            .Subscribe()
            .AddTo(this);

        _state.OnExitClick
            .Do(OnExitClick)
            .Subscribe()
            .AddTo(this);
    }

    private void OnPlayClick(Unit _) {
        SceneManager.LoadScene("MainCourse");
    }

    private void OnSettingsClick(Unit _) {
        
    }

    private void OnExitClick(Unit _) {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        
        Application.Quit();
    }
}
