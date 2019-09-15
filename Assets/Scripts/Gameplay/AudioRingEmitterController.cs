using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRingEmitterController : MonoBehaviour {
    [SerializeField] 
    private SoundRingEmitter _soundRingEmitter;
    private Coroutine _currentRoutine;
    
    public float interval;

    public void StartEmitting() {
        StopCurrentRoutine();
        _currentRoutine = StartCoroutine(StartEmittingSoundRings());
    }

    public void StopEmitting() {
        StopCurrentRoutine();
    }
    
    private void Awake() {
        if (_soundRingEmitter == null)
            _soundRingEmitter = GetComponent<SoundRingEmitter>();

        _soundRingEmitter.emissionTimeout = 0;
    }

    private void Start() {
        StartEmitting();
    }

    private void StopCurrentRoutine() {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = null;
    }
    
    private IEnumerator StartEmittingSoundRings() {
        while (true) {
            yield return new WaitForSeconds(interval);
                
            _soundRingEmitter.EmitSoundRing();
        }
    }
}