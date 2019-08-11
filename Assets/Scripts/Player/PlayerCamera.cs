using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCamera : MonoBehaviour {
    private IInputProvider inputProvider;

    enum CameraClampDirection { up, down, none };
    private float accumulatedInputY = 0f;

    [SerializeField]
    private Transform playerTransform;

    [Inject]
    private void Init(IInputProvider inputProvider) {
        this.inputProvider = inputProvider;
    }

    private void Update() {
        MoveCameraBasedOnInput();
    }

    private void MoveCameraBasedOnInput() {
        var inputY = inputProvider.ProvideLookInputY();
        var inputX = inputProvider.ProvideLookInputX();
        var clampDirection = CameraClampDirection.none;

        accumulatedInputY += inputY;
        if (accumulatedInputY > 90f)
            clampDirection = CameraClampDirection.up;
        else if (accumulatedInputY < -90f)
            clampDirection = CameraClampDirection.down;

        accumulatedInputY = Mathf.Clamp(accumulatedInputY, -90f, 90f);
        SetCameraRotationToMaximumAllowedIfNeeded(clampDirection);

        // Up/down movement needs to be clamped
        if (clampDirection == CameraClampDirection.none)
            transform.Rotate(Vector3.left * inputY);

        // But left/right movement does not
        playerTransform.Rotate(Vector3.up * inputX);
    }

    private void SetCameraRotationToMaximumAllowedIfNeeded(CameraClampDirection clampDirection) {
        if (clampDirection == CameraClampDirection.none)
            return;

        var targetValue = clampDirection == CameraClampDirection.up ? 270f : 90f;
        var newEulerAngles = transform.eulerAngles;
        newEulerAngles.x = targetValue;

        transform.eulerAngles = newEulerAngles;
    }
}
