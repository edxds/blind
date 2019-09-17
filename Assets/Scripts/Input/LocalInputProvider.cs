using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

class LocalInputProvider : IInputProvider, IInitializable, ILateDisposable
{
    private readonly float _inputSensibility;

    [Inject]
    public LocalInputProvider(IGameSettingsProvider settingsProvider) {
        _inputSensibility = settingsProvider.InputSettings.MouseSensitivity;
    }

    public void Initialize() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public float ProvideLookInputY() {
        return Input.GetAxis("Mouse Y") * _inputSensibility;
    }

    public float ProvideLookInputX() {
        return Input.GetAxis("Mouse X") * _inputSensibility;
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

    public bool ProvidePrimaryActionInput() {
        return Input.GetButton("PrimaryAction");
    }

    public bool ProvidePrimaryActionInputDown() {
        return Input.GetButtonDown("PrimaryAction");
    }

    public bool ProvideInteractionInput() {
        return Input.GetButton("Interaction");
    }

    public bool ProvideInteractionInputDown() {
        return Input.GetButtonDown("Interaction");
    }

    public void LateDispose() {
        Cursor.lockState = CursorLockMode.None;
    }
}
