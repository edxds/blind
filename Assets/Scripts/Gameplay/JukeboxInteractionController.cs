using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class JukeboxInteractionController : MonoBehaviour {
    [SerializeField]
    private Interactable _interactable;
    [SerializeField]
    private AudioSource _audioSource;
    
    private void Awake() {
        if (_interactable == null)
            _interactable = GetComponent<Interactable>();

        if (_audioSource == null)
            _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start() {
        UpdateInteractableTitle();

        _interactable.OnInteraction
            .Do(_ => { OnInteraction(); })
            .Subscribe()
            .AddTo(this);
    }

    private void UpdateInteractableTitle() {
        _interactable.actionTitle = _audioSource.isPlaying
            ? "Pausar a Música"
            : "Tocar a Música";
    }

    private void OnInteraction() {
        if (_audioSource.isPlaying)
            _audioSource.Pause();
        else 
            _audioSource.UnPause();
        
        UpdateInteractableTitle();
    }
}
