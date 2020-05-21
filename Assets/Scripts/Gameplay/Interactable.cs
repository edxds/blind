using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Interactable : MonoBehaviour {
    private readonly Subject<Unit> _onInteraction = new Subject<Unit>();
    
    public string actionTitle;

    public IObservable<Unit> OnInteraction => _onInteraction.AsObservable();

    public void Interact() {
        _onInteraction.OnNext(Unit.Default);
    }
}
