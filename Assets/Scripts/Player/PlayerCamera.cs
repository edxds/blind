using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCamera : MonoBehaviour {
    private IInputProvider _inputProvider;

    enum CameraClampDirection { up, down, none };
    private float _accumulatedInputY = 0f;

    [SerializeField]
    private Transform playerTransform;

    [Inject]
    private void Init(IInputProvider inputProvider) {
        this._inputProvider = inputProvider;
    }

    private void Update() {
        MoveCameraBasedOnInput();
    }

    private void MoveCameraBasedOnInput() {
        var inputY = _inputProvider.ProvideLookInputY();
        var inputX = _inputProvider.ProvideLookInputX();
        var clampDirection = CameraClampDirection.none;

        _accumulatedInputY += inputY;
        if (_accumulatedInputY > 90f)
            clampDirection = CameraClampDirection.up;
        else if (_accumulatedInputY < -90f)
            clampDirection = CameraClampDirection.down;

        _accumulatedInputY = Mathf.Clamp(_accumulatedInputY, -90f, 90f);
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
