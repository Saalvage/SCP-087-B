using Godot;

public partial class LightsAction : ActionBase {
    [Export] public required AnimationPlayer Animations;
    [Export] public float ActivationDistance = 1f;

    public required CommonResourceHolder Resources;
    public required PlayerController Player;

    protected Mental? _enemy;

    protected virtual void AddEnemy() {
        _enemy = Resources.Mental.Instantiate<Mental>();
        AddChild(_enemy);
        _enemy.AnimationInfo = AnimationInfo.Walking;
        _enemy.GlobalPosition = FloorEnd.GlobalPosition with { Y = FloorEnd.GlobalPosition.Y - 0.5f };
        _enemy.Speed = 0.6f;
        _enemy.KillDistance = 0.8f;
    }

    protected virtual void RemoveEnemy() {
        _enemy?.QueueFree();
    }

    private void EnableLight() {
        Helpers.SetBrightness(Player.Brightness);
    }

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || Animations.AssignedAnimation != "") { return; }

        if (Player.GlobalPosition.DistanceSquaredTo(FloorCenter.GlobalPosition) < ActivationDistance * ActivationDistance) {
            Helpers.SetBrightness(25);
            Animations.Play("Actions");
            AudioManager.PlaySound(Resources.FireOut);
        }
    }
}
