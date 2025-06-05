using Godot;

public partial class CharacterControllerBase : CharacterBody3D {
    protected new bool IsOnFloor { get; private set; }

    private static readonly Vector3 Step = new(0f, 0.25f, 0f);
    private static readonly float StepLengthSquared = Step.LengthSquared();

    public override void _PhysicsProcess(double delta) {
        Velocity = IsOnFloor ? Velocity with { Y = 0f } : (Velocity + GetGravity() * (float)delta);

        var oldPos = GlobalPosition;
        MoveAndCollide(Step);
        var verticalDelta = GlobalPosition - oldPos;
        MoveAndSlide();
        IsOnFloor = MoveAndCollide(Step * -(verticalDelta.Dot(Step) / StepLengthSquared * 1.001f)) != null;
    }
}
