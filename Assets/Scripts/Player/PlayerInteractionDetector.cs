using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour {
    private readonly Subject<Interactable> _currentInteractable = new Subject<Interactable>();

    public IObservable<Interactable> CurrentInteractable => _currentInteractable;

    private void OnTriggerEnter(Collider collider) {
        var interactable = collider.GetComponent<Interactable>();
        _currentInteractable.OnNext(interactable);
    }

    private void OnTriggerExit(Collider collider) {
        _currentInteractable.OnNext(null);
    }
}
