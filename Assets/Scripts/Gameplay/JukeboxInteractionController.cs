using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class JukeboxInteractionController : MonoBehaviour {
    [SerializeField] private Interactable _interactable;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioRingEmitterController _ringEmitter;
    
    private void Awake() {
        if (_interactable == null)
            _interactable = GetComponent<Interactable>();

        if (_audioSource == null)
            _audioSource = GetComponentInChildren<AudioSource>();

        if (_ringEmitter == null)
            _ringEmitter = GetComponent<AudioRingEmitterController>();
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

    private void UpdateRingEmitterState() {
        if (_audioSource.isPlaying)
            _ringEmitter.StartEmitting();
        else
            _ringEmitter.StopEmitting();
    }
    
    private void OnInteraction() {
        if (_audioSource.isPlaying)
            _audioSource.Pause();
        else 
            _audioSource.UnPause();
        
        UpdateInteractableTitle();
        UpdateRingEmitterState();
    }
}
