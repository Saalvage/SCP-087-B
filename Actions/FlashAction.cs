using System;
using System.Diagnostics;
using Godot;

public partial class FlashAction : ActionBase {
    public required CommonResourceHolder Resources;

    public required PlayerController Player;
    public required MapGenerator MapGenerator;

    private Mental? _enemy;

    private Node3D _spawnPoint = null!;

    public override void _Ready() {
        _spawnPoint = MapGenerator.Random.Next(3) switch {
            0 => FloorStart,
            1 => FloorCenter,
            2 => FloorEnd,
            _ => throw new UnreachableException(),
        };
    }

    public override void _PhysicsProcess(double delta) {
        if (!IsActive) { return; }

        if (_enemy == null && Player.GlobalPosition.DistanceSquaredTo(_spawnPoint.GlobalPosition) < 1.5f * 1.5f) {
            _enemy = Resources.Mental.Instantiate<Mental>();
            GetTree().Root.AddChild(_enemy);
            _enemy.GlobalPosition = _spawnPoint.GlobalPosition with { Y = _spawnPoint.GlobalPosition.Y - 0.5f };

            AudioManager.PlaySound(Resources.HorrorSounds.RandomElement());

            GetTree().CreateTimer(25.0 / 60.0).Timeout += () => {
                _enemy?.QueueFree();
            };
        }
    }
}
