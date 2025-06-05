using Godot;

[SceneGlobal]
public partial class CommonResourceHolder : Node {
    [Export] public required PackedScene Mental;
    [Export] public required Material Mental173Material;

    [Export] public required AudioStream FireOut;
    [Export] public required AudioStream[] HorrorSounds;
    [Export] public required AudioStream Stone;
}
