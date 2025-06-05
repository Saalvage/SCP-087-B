using Godot;

public partial class StepsAction : ActionBase {
    [Export] public required AnimationPlayer Animations;
    [Export] public float RequiredDistance = 6f;

    public required PlayerController Player;

    public override void _PhysicsProcess(double delta) {
        var shouldBeActive = IsActive && Player.GlobalPosition.DistanceSquaredTo(FloorEnd.GlobalPosition)
            < RequiredDistance * RequiredDistance;

        if (shouldBeActive != Animations.IsPlaying()) {
            if (shouldBeActive) {
                Animations.Play();
            } else {
                Animations.Pause();
            }
        }
    }
}
