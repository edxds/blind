using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SoundRingEmitter : MonoBehaviour {
    private const string _dynamicGameObjectsTag = "@DynamicGameObjects";
    private GameObject _dynamicGameObjectsParent;

    private bool _onTimeout;
    
    private IInputProvider _inputProvider;

    public float emissionTimeout = 0.3f;
    public Transform playerTransform;
    public GameObject soundRingProjectorPrefab;

    [Inject]
    private void Init(IInputProvider inputProvider) {
        _inputProvider = inputProvider;
    }

    private void Awake() {
        _dynamicGameObjectsParent = GameObject.FindWithTag(_dynamicGameObjectsTag);
    }

    public async void EmitSoundRing() {
        if (_onTimeout)
            return;
        
        InstantiateSoundRing();
        _onTimeout = true;
        
        await Task.Delay(TimeSpan.FromSeconds(emissionTimeout));
        _onTimeout = false;
    }
    
    private void InstantiateSoundRing() {
        Instantiate(
            soundRingProjectorPrefab,
            playerTransform.position,
            Quaternion.Euler(90, 0, 0),
            _dynamicGameObjectsParent.transform
        );
    }
}