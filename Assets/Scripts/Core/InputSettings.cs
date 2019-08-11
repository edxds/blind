using UnityEngine;
using System.Collections;

public struct InputSettings {
    public float MouseSensitivity {
        get;
    }

    public InputSettings(float mouseSensitivity) {
        MouseSensitivity = mouseSensitivity;
    }
}
