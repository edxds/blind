using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepEmitter : MonoBehaviour {
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private AudioSource _audioSource;

    private int _currentAudioClipIndex;
    private float _currentPlayerVelocity;
    private float _currentFootstepInterval;
    private float _secondsSinceLastFootstep;

    public float basePlayerVelocity;
    public float baseFootstepInterval;

    public AudioClip[] footstepAudioClips;
    
    private void Awake() {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();

        if (_audioSource == null)
            _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update() {
        _currentPlayerVelocity = _characterController.velocity.magnitude;

        var footstepIntervalFactor = basePlayerVelocity / _currentPlayerVelocity;
        _currentFootstepInterval = baseFootstepInterval * footstepIntervalFactor;
        
        TryToEmitFootstep();
    }

    private void TryToEmitFootstep() {
        _secondsSinceLastFootstep += Time.deltaTime;

        if (!PlayerIsMovingAndGrounded()) {
            // Prevents timeout from accumulating when the player is stopped
            _secondsSinceLastFootstep = 0;
            return;
        }

        if (_secondsSinceLastFootstep <= _currentFootstepInterval) return;
        
        EmitFootstep();
        _secondsSinceLastFootstep = 0;
    }
    
    private void EmitFootstep() {
        // Not playing audio when player is idle prevents an awkward
        // 'residue' footstep when the player stops moving.
        if (_audioSource.isPlaying || !PlayerIsMovingAndGrounded())
            return;
        
        _audioSource.PlayOneShot(footstepAudioClips[_currentAudioClipIndex]);
        IncreaseOrLoopCurrentAudioClipIndex();
    }

    private void IncreaseOrLoopCurrentAudioClipIndex() {
        _currentAudioClipIndex++;

        if (_currentAudioClipIndex >= footstepAudioClips.Length)
            _currentAudioClipIndex = 0;
    }

    private bool PlayerIsMovingAndGrounded() {
        var playerIsIdle = Mathf.Approximately(_currentPlayerVelocity, 0);
        return !playerIsIdle && _characterController.isGrounded;
    }
}