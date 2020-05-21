using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Goal : MonoBehaviour {
    private readonly Subject<Unit> _onFinish = new Subject<Unit>();
    private readonly Subject<Unit> _onStart = new Subject<Unit>();
    
    public string goalTitle;
    
    public IObservable<Unit> OnStart => _onStart;
    public IObservable<Unit> OnFinish => _onFinish;

    public void StartGoal() {
        _onStart.OnNext(Unit.Default);
    }
    
    public void Finish() {
        _onFinish.OnNext(Unit.Default);
    }
}
