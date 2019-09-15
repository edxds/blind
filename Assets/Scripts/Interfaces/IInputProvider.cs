public interface IInputProvider {
    void UnlockCursor();
    
    float ProvideLookInputY();
    float ProvideLookInputX();

    float ProvideMoveInputY();
    float ProvideMoveInputX();

    bool ProvideWantsToRunInput();
    bool ProvidePrimaryActionInput();
    bool ProvidePrimaryActionInputDown();
    bool ProvideInteractionInput();
    bool ProvideInteractionInputDown();
}