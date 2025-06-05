using System.Linq;
using Godot;

public partial class _173Action : ActionBase {
    [Export] public required Mental? Mental;
    [Export] public required VisibleOnScreenNotifier3D Visibility;
    [Export] public required AudioStream DontLook;

    public required CommonResourceHolder Resources;
    public required PlayerController Player;

    private Timer _timer = new();
    private bool _hasSeen;

    private bool Is173Active {
        get;
        set {
            if (field == value || Mental == null) { return; }

            field = value;

            if (value) {
                Mental.KillDistance = 0.8f;
                Mental.Speed = 1.2f;
                Mental.Animations.SpeedScale = 0f;
            } else {
                Mental.KillDistance = null;
                Mental.Speed = 0f;
                Mental.Animations.SpeedScale = 1f;
            }

        }
    }

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || Mental == null) { return; }

        if (GetWorld3D().IsVisible(Mental.GlobalPosition, Player.Camera.GlobalPosition,
                [Mental.GetRid(), Player.GetRid()])) {
            if (!Visibility.IsOnScreen()) {
                Is173Active = true;
            } else {
                if (!_hasSeen) {
                    _hasSeen = true;
                    AudioManager.PlaySound(Resources.HorrorSounds[2]);
                    _timer.Start();
                }
                Is173Active = false;
            }
        } else {
            Is173Active = false;
        }
    }

    private bool _hasEntered;
    protected override void OnEnterInternal() {
        if (_hasEntered) { return; }
        _hasEntered = true;

        Mental!.AnimationInfo = AnimationInfo.Breathing;
        var mesh = Mental.EnumerateDescendantsDepthFirst(true).OfType<MeshInstance3D>().First();
        mesh.MaterialOverride = Resources.Mental173Material;

        AddChild(_timer);
        AudioManager.PlaySound3D(DontLook, Mental);
        _timer.Timeout += () => AudioManager.PlaySound3D(DontLook, Mental);
        _timer.Start(660f / 60f);
    }

    protected override void OnLeaveInternal() {
        Mental?.QueueFree();
        _timer.Stop();
        Mental = null;
    }
}
