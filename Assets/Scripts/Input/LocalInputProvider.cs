using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

class LocalInputProvider : IInputProvider, IInitializable, ILateDisposable {
    private readonly float inputSensibility;

    [Inject]
    public LocalInputProvider(IGameSettingsProvider settingsProvider) {
        inputSensibility = settingsProvider.InputSettings.MouseSensitivity;
    }

    public void Initialize() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public float ProvideLookInputY() {
        return Input.GetAxis("Mouse Y") * inputSensibility;
    }

    public float ProvideLookInputX() {
        return Input.GetAxis("Mouse X") * inputSensibility;
    }

    public float ProvideMoveInputY() {
        return Input.GetAxis("Vertical");
    }

    public float ProvideMoveInputX() {
        return Input.GetAxis("Horizontal");
    }

    public bool ProvideWantsToRunInput() {
        return Input.GetButton("Run");
    }

    public void LateDispose() {
        Cursor.lockState = CursorLockMode.None;
    }
}
