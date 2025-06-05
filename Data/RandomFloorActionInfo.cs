using Godot;

[GlobalClass, Tool]
public partial class RandomFloorActionInfo : Resource {
    [Export] public required PackedScene[] Actions;
    [Export] public int Count;
    [Export] public int Start;
    [Export] public int End;
}
