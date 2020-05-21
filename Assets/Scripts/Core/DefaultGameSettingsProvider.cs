public class DefaultGameSettingsProvider : IGameSettingsProvider {
    private readonly InputSettings _inputSettings = new InputSettings(15f);

    public InputSettings InputSettings => _inputSettings;
}
