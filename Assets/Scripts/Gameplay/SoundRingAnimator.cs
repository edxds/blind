using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundRingAnimator : MonoBehaviour {
    private bool _isAnimating;
    private float _fadeoutDuration;
    private float _currentTime;

    private float _currentAnimationVelocity;
    private float _currentYPosition;
    private Vector3 _currentPosition;

    private Material _material;
    private bool _isMaterialCompatible;
    private const string _opacityPropertyName = "_Opacity";
    private static readonly int _opacityPropertyId = Shader.PropertyToID(_opacityPropertyName);

    public float targetYPosition;
    public float animationDuration;

    public float initialOpacity = 1f;
    public float finalOpacity = 0f;
    public float fadeinDuration = 0.5f;
    public float timeUntilFadeout;

    private void Awake() {
        var projector = GetComponent<Projector>();
        
        // Probably really inefficient but I don't have time :-) 
        var materialCopy = new Material(projector.material);
        projector.material = materialCopy;
        
        _material = projector.material;
        _isMaterialCompatible = _material.HasProperty(_opacityPropertyId);
        if (!_isMaterialCompatible) {
            Debug.LogWarning(
                $"Property ${_opacityPropertyName} not found in projector shader!"
            );

            return;
        }

        _material.SetFloat(_opacityPropertyId, fadeinDuration <= 0 ? initialOpacity : 0f);
    }

    private void Start() {
        _fadeoutDuration = animationDuration - timeUntilFadeout;
        if (_fadeoutDuration < 0) {
            Debug.LogWarning(
                $"Time until fadeout (${timeUntilFadeout})"
                + $" was greater than animation duration (${animationDuration})!"
            );

            _fadeoutDuration = 0;
        }

        StartCoroutine(StartAnimating());
    }

    private void Update() {
        _currentTime += Time.deltaTime;
        if (_currentTime <= fadeinDuration)
            UpdateFadein();    
        
        if (_currentTime >= timeUntilFadeout)
            UpdateFadeout();    
        
        if (_isAnimating)
            UpdateAnimation();
    }

    private IEnumerator StartAnimating() {
        _isAnimating = true;

        yield return new WaitForSeconds(animationDuration);
        _isAnimating = false;

        Remove();
    }

    private void UpdateFadein() {
        if (!_isMaterialCompatible)
            return;

        var currentInterpolationTime = _currentTime / fadeinDuration;
        var newOpacity = Mathf.Lerp(
            0f,
            initialOpacity,
            currentInterpolationTime
        );
        
        _material.SetFloat(_opacityPropertyId, newOpacity);
    }
    
    private void UpdateFadeout() {
        if (!_isMaterialCompatible)
            return;

        var currentInterpolationTime = (_currentTime - timeUntilFadeout) / _fadeoutDuration;
        var newOpacity = Mathf.Lerp(
            initialOpacity,
            finalOpacity,
            currentInterpolationTime
        );
        
        _material.SetFloat(_opacityPropertyId, newOpacity);
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