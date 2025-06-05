using Godot;

public readonly record struct AnimationInfo(float Start, float End, float Speed) {
    public static AnimationInfo Still = new(0f, 1f, 0f);
    public static AnimationInfo Breathing = new(206f, 250f, 3f);
    public static AnimationInfo Walking = new(1f, 14f, 9f);
}

public partial class Mental : CharacterControllerBase {
    [Export] public required AnimationPlayer Animations;
    [Export] public float Speed;
    public float? KillDistance;

    public required PlayerController Player;

    public AnimationInfo AnimationInfo {
        get;
        set {
            field = value;
            Animations.Stop();
        }
    } = AnimationInfo.Still;

    public override void _Process(double delta) {
        // This is really fucking stupid, but I don't want to manually fix up the animations for this model.
        if (!Animations.IsPlaying()) {
            Animations.PlaySection("anim", AnimationInfo.Start, AnimationInfo.End, -1, AnimationInfo.Speed);
        }
    }

    public override void _PhysicsProcess(double deltaD) {
        if (GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) < KillDistance * KillDistance) {
            Player.Kill();
        }

        if (GetWorld3D().IsVisible(GlobalPosition, Player.Camera.GlobalPosition, [GetRid(), Player.GetRid()])) {
            GlobalBasis = Basis.LookingAt((Player.Camera.GlobalPosition - GlobalPosition) with { Y = 0 }, Vector3.Up);
            // Uses mental's mesh position for some reason.
            var horizontalVel = (GlobalPosition - UpDirection * 0.4f)
                .DistanceSquaredTo(Player.Camera.GlobalPosition) > 1.5f * 1.5f
                    ? GlobalBasis * new Vector3(0, 0, -Speed)
                    : Vector3.Zero;
            Velocity = horizontalVel with { Y = Velocity.Y };
        }

        base._PhysicsProcess(deltaD);
    }
}
