using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRingEmitterController : MonoBehaviour {
    [SerializeField] 
    private SoundRingEmitter _soundRingEmitter;
    
    public float interval;

    private void Awake() {
        if (_soundRingEmitter == null)
            _soundRingEmitter = GetComponent<SoundRingEmitter>();

        _soundRingEmitter.emissionTimeout = 0;
    }

    private void Start() {
        StartCoroutine(StartEmittingSoundRings());
    }

    private IEnumerator StartEmittingSoundRings() {
        while (true) {
            yield return new WaitForSeconds(interval);
                
            _soundRingEmitter.EmitSoundRing();
        }
    }
}