public class DefaultGameSettingsProvider : IGameSettingsProvider {
    private readonly InputSettings inputSettings = new InputSettings(15f);

    public InputSettings InputSettings => inputSettings;
}
