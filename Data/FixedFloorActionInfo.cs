using Godot;

[GlobalClass, Tool]
public partial class FixedFloorActionInfo : Resource {
    [Export] public required PackedScene[] Actions;
    [Export] public int MinFloor;
    [Export] public int MaxFloor;
    [Export] public float Probability = 1f;
}
