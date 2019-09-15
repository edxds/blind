using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PhoneInteractionController : MonoBehaviour {
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioLowPassFilter _audioLowPassFilter;
    [SerializeField] private AudioClip _talkingClip;
    
    [SerializeField] private Interactable _interactable;
    [SerializeField] private Goal _goal;

    public float waitTime;
    
    private void Awake() {
        if (_interactable == null)
            _interactable = GetComponent<Interactable>();

        if (_goal)
            _goal = GetComponent<Goal>();
    }

    private void Start() {
        _interactable.OnInteraction
            .Do(
                _ => { StartCoroutine(OnInteraction()); }
            )
            .Subscribe()
            .AddTo(this);
    }

    private IEnumerator OnInteraction() {
        _audioSource.Stop();
        if (_audioLowPassFilter != null)
            _audioLowPassFilter.enabled = false;

        _audioSource.spatialBlend = 0;
        _audioSource.volume = 1;
        _audioSource.panStereo = -1;
        _audioSource.PlayOneShot(_talkingClip);
        
        yield return new WaitForSeconds(_talkingClip.length);
        
        _goal.Finish();
        _audioLowPassFilter.enabled = true;
    }
}
