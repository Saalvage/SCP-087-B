using Godot;

public partial class ActionBase : FloorBase {
    [Export] public required Node3D FloorStart;
    [Export] public required Node3D FloorCenter;
    [Export] public required Node3D FloorEnd;

    protected bool IsActive { get; private set; }

    public sealed override void OnEnter() {
        IsActive = true;
        OnEnterInternal();
    }

    public sealed override void OnLeave() {
        IsActive = false;
        OnLeaveInternal();
    }

    protected virtual void OnEnterInternal() { }
    protected virtual void OnLeaveInternal() { }
}
