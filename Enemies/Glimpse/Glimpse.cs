using Godot;
using System;

public partial class Glimpse : MeshInstance3D {
    public required PlayerController Player;
    [Export] public required Material[] Glimpses;
    [Export] public required AudioStreamPlayer3D NoSoundPlayer;

    public override void _Ready() {
        MaterialOverride = Glimpses.RandomElement();
    }

    public override void _Process(double delta) {
        var scale = GlobalBasis.Scale;
        GlobalBasis = Basis.LookingAt(Player.Camera.GlobalPosition - GlobalPosition, Vector3.Up)
                      * Basis.FromEuler(new(MathF.PI / 2, MathF.PI, 0))
                      * Basis.FromScale(scale);
    }

    public override void _PhysicsProcess(double delta) {
        if (Visible && Player.Floor - 1 == Helpers.GetFloor(GlobalPosition)
                    && (Player.GlobalPosition with { Y = 0f }).DistanceSquaredTo(GlobalPosition with { Y = 0f }) < 2.3f * 2.3f
                    && GetWorld3D().IsVisible(GlobalPosition, Player.Camera.GlobalPosition, exclude: [Player.GetRid()])) {
            NoSoundPlayer.Finished += QueueFree;
            NoSoundPlayer.Play();
            Visible = false;
        }
    }
}
