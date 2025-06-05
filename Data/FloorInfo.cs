using Godot;

[GlobalClass, Tool]
public partial class FloorInfo : Resource {
    [Export] public required PackedScene Floor;
    [Export] public int MinFloor = -1;
    [Export] public int Weight = 1;
}
