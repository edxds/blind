public interface IInputProvider {
    float ProvideLookInputY();
    float ProvideLookInputX();

    float ProvideMoveInputY();
    float ProvideMoveInputX();

    bool ProvideWantsToRunInput();
    bool ProvidePrimaryActionInput();
    bool ProvidePrimaryActionInputDown();
}