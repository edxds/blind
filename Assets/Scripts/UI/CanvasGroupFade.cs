using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFade : MonoBehaviour {
    private Coroutine _currentRoutine;
    private float _currentElapsedTime;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void Awake() {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeTo(float target, bool animated) {
        CancelCurrentRoutine();

        if (animated)
            _currentRoutine = StartCoroutine(Fade(_canvasGroup.alpha, target));
        else
            _canvasGroup.alpha = target;
    }

    private void CancelCurrentRoutine() {
        if (_currentRoutine == null) return;
        
        StopCoroutine(_currentRoutine);
        _currentRoutine = null;
    }
    
    private IEnumerator Fade(float from, float to) {
        _currentElapsedTime = 0f;

        while (_currentElapsedTime <= _fadeDuration) {
            _currentElapsedTime += Time.deltaTime;
            var progress = _currentElapsedTime / _fadeDuration;

            var newAlpha = Mathf.Lerp(
                from,
                to,
                progress
            );

            _canvasGroup.alpha = newAlpha;
            yield return new WaitForEndOfFrame();
        }
    }
}
