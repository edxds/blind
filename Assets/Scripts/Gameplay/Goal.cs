using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Goal : MonoBehaviour {
    private readonly Subject<Unit> _onFinish = new Subject<Unit>();

    public string goalTitle;
    public IObservable<Unit> OnFinish => _onFinish;
    
    public void Finish() {
        _onFinish.OnNext(Unit.Default);
    }
}
