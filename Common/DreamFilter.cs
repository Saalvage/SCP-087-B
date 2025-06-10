using Godot;

public partial class DreamFilter : FullSizeViewport {
    private double _acc;

    [Export] public float UpdateInterval = 1f / 60f;

    public override void _Process(double delta) {
        _acc += delta;

        if (_acc > UpdateInterval) {
            _acc %= UpdateInterval;
            RenderTargetUpdateMode = UpdateMode.Once;
        }
    }
}
