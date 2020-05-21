using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

class LocalInputProvider : IInputProvider, IInitializable, ILateDisposable {
    private readonly float _inputSensibility;

    private string _lastInputType = "mouse";
    private readonly Subject<string> _currentInputType = new Subject<string>();

    [Inject]
    public LocalInputProvider(IGameSettingsProvider settingsProvider) {
        _inputSensibility = settingsProvider.InputSettings.MouseSensitivity;
    }

    public void Initialize() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public IObservable<string> CurrentInputType() {
        return _currentInputType.AsObservable();
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public float ProvideLookInputY() {
        var mouseY = Input.GetAxis("Mouse Y") * _inputSensibility;
        var controllerY = Input.GetAxis("Controller Look Y") * _inputSensibility;

        _currentInputType.OnNext(getInputTypeBasedOnTwoInputs(mouseY, controllerY));
        return mouseY + controllerY;
    }

    public float ProvideLookInputX() {
        var mouseX = Input.GetAxis("Mouse X") * _inputSensibility;
        var controllerX = Input.GetAxis("Controller Look X") * _inputSensibility;

        _currentInputType.OnNext(getInputTypeBasedOnTwoInputs(mouseX, controllerX));
        return mouseX + controllerX;
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

    private string getInputTypeBasedOnTwoInputs(float mouse, float controller) {
        var newInputType = _lastInputType;

        if (mouse > 0 && Mathf.Approximately(controller, 0)) newInputType = "mouse";
        else if (controller > 0 && Mathf.Approximately(mouse, 0)) newInputType = "controller";

        _lastInputType = newInputType;
        return newInputType;
    }
}
