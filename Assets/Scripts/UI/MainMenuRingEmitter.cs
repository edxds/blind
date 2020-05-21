using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRingEmitter : MonoBehaviour {
    public float interval = 2.5f;
    public Transform ringsParent;
    public GameObject ringPrefab;

    private void Start() {
        StartCoroutine(StartEmission());
    }

    private IEnumerator StartEmission() {
        while (true) {
            Instantiate(ringPrefab, ringsParent);
            yield return new WaitForSeconds(interval);
        }
    }
}
