using Godot;
using System.Linq;

public partial class TrickAction : ActionBase {
    [Export] public required Node3D Marker;
    
    public required CommonResourceHolder Resources { private get; init; }
    public required PlayerController Player { private get; init; }

    private Mental? _enemy;

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || _enemy != null) { return; }

        if (Player.GlobalPosition.DistanceSquaredTo(Marker.GlobalPosition) < 0.25f * 0.25f) {
            _enemy = Resources.Mental.Instantiate<Mental>();
            AddChild(_enemy);
            _enemy.GlobalPosition = Marker.GlobalTransform * (2f * Vector3.Back);
            _enemy.Speed = 0.6f;
            _enemy.KillDistance = 0.8f;
            var mesh = _enemy.EnumerateDescendantsDepthFirst(true).OfType<MeshInstance3D>().First();
            mesh.MaterialOverride = Resources.Mental173Material;
            AudioManager.PlaySound(Resources.HorrorSounds[2]);
        }
    }
}
