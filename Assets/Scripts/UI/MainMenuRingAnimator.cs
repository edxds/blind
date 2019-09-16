using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRingAnimator : MonoBehaviour {
    private Image _image;

    public float targetScale = 1f;
    public float fadeInDuration = 0.5f;
    public float timeToWaitUntilFadeOut = 0.5f;
    public float animationDuration = 5f;
    
    private void Awake() {
        _image = GetComponent<Image>();
    }

    private void Start() {
        StartCoroutine(StartAnimationChain());
        Destroy(gameObject, animationDuration);
    }

    private IEnumerator StartAnimationChain() {
        StartCoroutine(ScaleUp());
        FadeIn();
        
        yield return new WaitForSeconds(timeToWaitUntilFadeOut);
        FadeOut();
    }

    private IEnumerator ScaleUp() {
        var elapsedTime = 0f;

        while (elapsedTime <= animationDuration) {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;

            var progress = elapsedTime / animationDuration;
            var newScale = Mathf.Lerp(
                0f,
                targetScale,
                progress
            );
            
            _image.rectTransform.localScale = new Vector3(newScale, newScale, 1f);
        }
    }
    
    private void FadeIn() {
//        var elapsedTime = 0f;
//
//        while (elapsedTime <= fadeInDuration) {
//            yield return new WaitForEndOfFrame();
//            elapsedTime += Time.deltaTime;
//
//            var progress = elapsedTime / fadeInDuration;
//            var newAlpha = Mathf.Lerp(
//                0f,
//                1f,
//                progress
//            );
//            
//            
//        }
        _image.canvasRenderer.SetAlpha(0f);
        _image.CrossFadeAlpha(1f, fadeInDuration, false);
    }

    private void FadeOut() {
        _image.CrossFadeAlpha(0f, animationDuration - timeToWaitUntilFadeOut, false);
    }
}
