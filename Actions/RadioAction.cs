using Godot;

public partial class RadioAction : ActionBase {
    [Export] public required AudioStream Sound;
    [Export] public float Delay;

    private bool _played;

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || _played) { return; }

        GetTree().CreateTimer(Delay).Timeout += () => {
            AudioManager.PlaySound(Sound);
        };

        _played = true;
    }
}
