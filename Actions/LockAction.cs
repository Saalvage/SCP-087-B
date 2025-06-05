using Godot;

public partial class LockAction : ActionBase {
    [Export] public required Node3D Pillar;
    
    public required CommonResourceHolder Resources;
    public required PlayerController Player;

    public override void _Ready() {
        FloorStart.RemoveChild(Pillar);
    }

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || Pillar.GetParent() != null) { return; }

        if (FloorCenter.GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) < 1f) {
            FloorStart.AddChild(Pillar);
            AudioManager.PlaySound3D(Resources.Stone, Pillar);
        }
    }
}
