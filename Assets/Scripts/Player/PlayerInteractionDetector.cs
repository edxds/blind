using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerInteractionDetector : MonoBehaviour {
    private IInputProvider _inputProvider;
    
    private readonly Subject<Interactable> _currentInteractable = new Subject<Interactable>();
    private Interactable _currentInteractableInstance;
    
    public IObservable<Interactable> CurrentInteractable => _currentInteractable;

    [Inject]
    private void Init(IInputProvider inputProvider) {
        _inputProvider = inputProvider;
    }
    
    private void Update() {
        if (_inputProvider.ProvideInteractionInputDown())
            TryInteract();
    }

    private void OnTriggerEnter(Collider collider) {
        var interactable = collider.GetComponent<Interactable>();
        _currentInteractableInstance = interactable;
        _currentInteractable.OnNext(interactable);
    }

    private void OnTriggerExit(Collider collider) {
        _currentInteractableInstance = null;
        _currentInteractable.OnNext(null);
    }

    private void TryInteract() {
        if (_currentInteractableInstance == null)
            return;
        
        _currentInteractableInstance.Interact();
        
        // Because interaction titles may be updated after an interaction,
        // we call onNext to make sure listeners get an updated version of
        // the Interactable MonoBehaviour.
        _currentInteractable.OnNext(_currentInteractableInstance);
    }
}
