using Godot;
using System;
using System.Linq;

public partial class CellAction : ActionBase {
    [Export] public required Node3D SpawnPoint;
    [Export] public required AudioStream Breath;

    public required CommonResourceHolder Resources;
    public required PlayerController Player;

    private Mental? _enemy;
    private Timer _timer = new();
    private bool _hasMoved = false;

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || _hasMoved) { return; }

        if (MathF.Abs(Player.GlobalPosition.X - _enemy?.GlobalPosition.X ?? float.PositiveInfinity) < 0.025f
            && Random.Shared.Next(40) == 0) {
            _enemy!.MoveAndCollide(_enemy.GlobalBasis * (1.5f * Vector3.Forward));
            AudioManager.PlaySound(Resources.HorrorSounds[2]);
            _enemy.AnimationInfo = AnimationInfo.Still;
            _timer.Stop();
            _hasMoved = true;
        }
    }

    protected override void OnEnterInternal() {
        if (_enemy != null) { return; }

        _enemy = Resources.Mental.Instantiate<Mental>();
        AddChild(_enemy);
        _enemy.GlobalPosition = SpawnPoint.GlobalPosition;
        _enemy.AnimationInfo = AnimationInfo.Breathing;
        var mesh = _enemy.EnumerateDescendantsDepthFirst(true).OfType<MeshInstance3D>().First();
        mesh.MaterialOverride = Resources.Mental173Material;

        AddChild(_timer);
        AudioManager.PlaySound3D(Breath, SpawnPoint);
        _timer.Timeout += () => AudioManager.PlaySound3D(Breath, SpawnPoint);
        _timer.Start(610f / 60f);
    }
}
