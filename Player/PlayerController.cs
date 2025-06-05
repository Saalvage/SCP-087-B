using System;
using Godot;

[SceneGlobal]
public partial class PlayerController : CharacterControllerBase {
    [Export] public required AudioStream Step;

    [Export] public required AnimationPlayer Animations;
    [Export] public required Node3D CameraRail;
    [Export] public required Camera3D Camera;
    [Export] public float ForwardSpeed = 1.2f;
    [Export] public float BackwardSpeed = 0.9f;
    [Export] public float SidewardsSpeed = 0.48f;

    [Export] public byte Brightness = 40;
    [Export] public float FogNear = 1f;
    // Originally 2.5f, adjusted to account for radial fog.
    [Export] public float FogFar = 3f;

    [Export]
    public float DeathBrightnessLerp {
        get;
        set {
            field = value;

            if (value == 0f) {
                Helpers.SetBrightness(Brightness);
                return;
            }

            var subtract = float.Lerp(0f, 90f / 255f, value);
            Helpers.SetBrightness(Color.Color8(255, 100, 100) - new Color(subtract, subtract, subtract));
        }
    }

    private bool _isDead;

    private static readonly Vector3 HeadBobInterval = new Vector3(80f, 40f, 1f) / 60f;
    private static readonly Vector3 HeadBobOffset = new(0.08f, 0.1f, 0f);
    private float _headBobAcc;

    public int Floor => Helpers.GetFloor(GlobalPosition - new Vector3(0f, 0.5f, 0f));

    public override void _Ready() {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        SetFogRange(FogNear, FogFar);
        Helpers.SetBrightness(Brightness);
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion mm) {
            HandleLook(0.01f * mm.Relative);
        }
    }

    private void HandleLook(Vector2 lookDelta) {
        CameraRail.Rotation -= new Vector3(lookDelta.Y, lookDelta.X, 0);
    }

    public override void _PhysicsProcess(double deltaD) {
        var inputDir = _isDead ? Vector2.Zero : new(
            SidewardsSpeed * (Input.GetActionStrength(Actions.MoveRight) - Input.GetActionStrength(Actions.MoveLeft)),
            Input.GetActionStrength(Actions.MoveBackward) != 0 ^ Input.GetActionStrength(Actions.MoveForward) != 0
                ? -ForwardSpeed * (Input.GetActionStrength(Actions.MoveForward))
                  + BackwardSpeed * (Input.GetActionStrength(Actions.MoveBackward))
                : 0
        );

        var rightDir = (-Camera.GlobalBasis.Z).Cross(UpDirection).Normalized();
        var forwardDir = rightDir.Cross(UpDirection).Normalized();

        Velocity = (rightDir * inputDir.X + forwardDir * inputDir.Y) with {
            Y = Velocity.Y,
        };

        if (inputDir != Vector2.Zero) {
            var oldAcc = _headBobAcc;
            _headBobAcc = (_headBobAcc + (float)deltaD) % MathF.Max(HeadBobInterval.X, HeadBobInterval.Y);
            if (_headBobAcc % HeadBobInterval.Y < oldAcc % HeadBobInterval.Y) {
                AudioManager.PlaySound(Step);
            }
            var intervalHalf = HeadBobInterval / 2f;
            Camera.Position = HeadBobOffset * (new Vector3(1f, 1f, 1f)
                                               - (new Vector3(_headBobAcc, _headBobAcc, _headBobAcc) % HeadBobInterval
                                                  - intervalHalf).Abs() / intervalHalf);
        }

        var prevVel = Velocity;
        base._PhysicsProcess(deltaD);

        if (IsOnFloor && prevVel.Y < -5.4f) {
            Kill();
        }
    }

    public void Kill() {
        if (_isDead) { return; }
        _isDead = true;

        Animations.Play("Death");
    }

    private void Crash() {
        if (Floor > 130) {
            var str = Random.Shared.Next(7) switch {
                0 => "NO",
                1 => "It's not about whether you die or not, it's about when you die.",
                2 => "NICE",
                3 => "welcome to NIL",
                _ => null,
            };
            if (str != null) {
                Input.MouseMode = Input.MouseModeEnum.Visible;
                OS.Alert(str, "Runtime Error");
            }
        }
        GetTree().Quit();
    }

    public void ResetFogRange() => SetFogRange(FogNear, FogFar);
    public void SetFogRange(float near, float far) {
        RenderingServer.GlobalShaderParameterSet("fog", new Vector2(near, far));
    }
}
