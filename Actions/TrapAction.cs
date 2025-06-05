using Godot;

public partial class TrapAction : ActionBase {
    [Export] public required Node3D Pillar;

    public required CommonResourceHolder Resources;
    public required PlayerController Player;

    private Mental? _enemy;

    public override void _PhysicsProcess(double delta) {
        if (!IsActive) { return; }

        if (_enemy == null && FloorCenter.GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) < 1f) {
            _enemy = Resources.Mental.Instantiate<Mental>();
            AddChild(_enemy);
            _enemy.GlobalPosition = FloorStart.GlobalPosition with { Y = FloorStart.GlobalPosition.Y - 0.5f };
            _enemy.Speed = 0.6f;
            _enemy.KillDistance = 0.8f;
            AudioManager.PlaySound(Resources.HorrorSounds.RandomElement());

            GetTree().CreateTimer(500f / 60f).Timeout += () => {
                Pillar.QueueFree();
                AudioManager.PlaySound(Resources.Stone);
            };

            GetTree().CreateTimer(1000f / 60f).Timeout += () => {
                _enemy.QueueFree();
            };
        }
    }
}
