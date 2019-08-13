using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundRingAnimator : MonoBehaviour {
    private bool _isAnimating;
    private float _currentAnimationVelocity;
    private float _currentYPosition;
    private Vector3 _currentPosition;
    
    public float targetYPosition;
    public float animationDuration;
    
    private void Start() {
        StartCoroutine(StartAnimating());
    }

    private void Update() {
        if (_isAnimating)
            UpdateAnimation();
    }
    
    private IEnumerator StartAnimating() {
        _isAnimating = true;
        
        yield return new WaitForSeconds(animationDuration);
        _isAnimating = false;
        
        Remove();
    }

    private void UpdateAnimation() {
        var localPosition = transform.localPosition;
        _currentPosition = localPosition;
        _currentYPosition = localPosition.y;
        
        var newYPosition = Mathf.SmoothDamp(
            _currentYPosition,
            targetYPosition,
            ref _currentAnimationVelocity,
            animationDuration
        );

        _currentPosition.y = newYPosition;
        transform.localPosition = _currentPosition;
    }

    private void Remove() {
        Destroy(gameObject);
    }
}