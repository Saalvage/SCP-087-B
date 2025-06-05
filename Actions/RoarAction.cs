using System;
using Godot;

public partial class RoarAction : ActionBase {
    [Export] public required Node3D Emitter;
    [Export] public required AudioStream Roar;

    public required PlayerController Player;

    private bool _isRoaring;
    private bool _wasActive;
    private Basis _appliedRot = Basis.Identity;

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || _wasActive) {
            return;
        }

        if (_isRoaring) {
            Player.GlobalPosition += new Vector3(RandPosOffset(), RandPosOffset(), RandPosOffset());

            Player.GlobalBasis *= _appliedRot.Inverse();
            _appliedRot = Basis.FromEuler(new(RandRot(), RandRot(), RandRot()));
            Player.GlobalBasis *= _appliedRot;

            float RandRot() => Random.Shared.NextSingle(-1f, 1f) / 360f;
            float RandPosOffset() => Random.Shared.NextSingle(-0.005f, 0.005f);
        } else if (FloorEnd.GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) < 6f * 6f) {
            _isRoaring = true;
            AudioManager.PlaySound(Roar);
            GetTree().CreateTimer(320f / 60f).Timeout += () => {
                _isRoaring = false;
                _wasActive = true;
                Player.GlobalBasis *= _appliedRot.Inverse();
            };
        }
    }
}
