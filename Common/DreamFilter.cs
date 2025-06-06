using Godot;

public partial class DreamFilter : FullSizeViewport {
    private double _acc;

    public required PlayerController Player;
    [Export] public required Sprite2D Child;

    [Export] public float UpdateInterval = 1f / 60f;

    public override void _Ready() {
        base._Ready();
        Child.Scale = Size / new Vector2(512, 512);
    }

    public override void _Process(double delta) {
        _acc += delta;

        if (_acc > UpdateInterval) {
            _acc %= UpdateInterval;
            RenderTargetUpdateMode = UpdateMode.Once;
        }
    }
}
