using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PhoneInteractionController : MonoBehaviour {
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
        // Do whatever...
        
        yield return new WaitForSeconds(waitTime);
        _goal.Finish();
    }
}
