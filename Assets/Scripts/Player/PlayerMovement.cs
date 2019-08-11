using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour {
    private IInputProvider inputProvider;

    [SerializeField]
    private CharacterController characterController;

    public float movementSpeed;

    [Inject]
    private void Init(IInputProvider inputProvider) {
        this.inputProvider = inputProvider;
    }

    private void Awake() {
        if (characterController == null) {
            characterController = GetComponent<CharacterController>();
        }
    }

    private void Update() {
        MoveCharacterBasedOnInput();
    }

    private void MoveCharacterBasedOnInput() {
        var moveY = inputProvider.ProvideMoveInputY();
        var moveX = inputProvider.ProvideMoveInputX();

        var forwardsMovement = transform.forward * moveY;
        var sidewaysMovement = transform.right * moveX;

        var unifiedMovement = (forwardsMovement + sidewaysMovement);
        var clampedMovementMagnitude = Mathf.Clamp01(unifiedMovement.magnitude) * movementSpeed;

        /*
            If we don't multiply by the clamped movement magnitude, we will lose
            the "smooth"/analog input, and will get janky movement.

            We also need to normalize the movement, otherwise our player will move
            faster while going diagonally because of the addition of vectors.
        */
        var normalizedMovement = unifiedMovement.normalized * clampedMovementMagnitude;
        characterController.SimpleMove(normalizedMovement);
    }
}
