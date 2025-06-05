using Godot;

public partial class FloorBase : Node3D {
    [Export] public PackedScene? PredeterminedFloor;

    public virtual void OnEnter() { }
    public virtual void OnLeave() { }
}
