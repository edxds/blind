using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour {
    private IInputProvider _inputProvider;

    private float _currentMovementSpeed;
    private float _currentMovementSpeedDampingVelocity;

    [SerializeField]
    private CharacterController characterController;

    public float movementSpeed;
    public float runMovementSpeedModifier = 1.5f;

    [Inject]
    private void Init(IInputProvider inputProvider) {
        this._inputProvider = inputProvider;
    }

    private void Awake() {
        _currentMovementSpeed = movementSpeed;

        if (characterController == null) {
            characterController = GetComponent<CharacterController>();
        }
    }

    private void Update() {
        MoveCharacterBasedOnInput();
    }

    private void MoveCharacterBasedOnInput() {
        var moveY = _inputProvider.ProvideMoveInputY();
        var moveX = _inputProvider.ProvideMoveInputX();

        var forwardsMovement = transform.forward * moveY;
        var sidewaysMovement = transform.right * moveX;

        var unifiedMovement = (forwardsMovement + sidewaysMovement);
        var clampedMovementMagnitude = Mathf.Clamp01(unifiedMovement.magnitude) * GetMovementSpeed();

        /*
            If we don't multiply by the clamped movement magnitude, we will lose
            the "smooth"/analog input, and will get janky movement.

            We also need to normalize the movement, otherwise our player will move
            faster while going diagonally because of the addition of vectors.
        */
        var normalizedMovement = unifiedMovement.normalized * clampedMovementMagnitude;
        characterController.SimpleMove(normalizedMovement);
    }

    private float GetMovementSpeed() {
        var wantsToRun = _inputProvider.ProvideWantsToRunInput();
        var targetSpeed = wantsToRun
            ? movementSpeed * runMovementSpeedModifier
            : movementSpeed;

        _currentMovementSpeed = Mathf.SmoothDamp(
            _currentMovementSpeed,
            targetSpeed,
            ref _currentMovementSpeedDampingVelocity,
            0.5f
        );

        return _currentMovementSpeed;
    }
}
