using Godot;

public partial class DarknessAction : ActionBase {
    [Export] public required AnimationPlayer Animations;
    [Export] public required Node3D PillarStart;
    [Export] public required Node3D PillarEnd;

    public required PlayerController Player;
    public required CommonResourceHolder Resources;

    [Export]
    public float DarknessFactor {
        get;
        set {
            field = value;
            if (Player == null!) { return; }
            var brightness = float.Max(10f, float.Lerp(Player.Brightness, 0f, value)) / 255f;
            Helpers.SetBrightness(new Color(brightness, brightness, brightness));
        }
    }

    private void AddEnemy() {
        var enemy = Resources.Mental.Instantiate<Mental>();
        AddChild(enemy);
        enemy.GlobalPosition = FloorCenter.GlobalPosition with { Y = FloorCenter.GlobalPosition.Y - 0.5f };
        enemy.Speed = 0.6f;
        enemy.KillDistance = 0.7f;
        AudioManager.PlaySound(Resources.HorrorSounds.RandomElement());
    }

    public override void _Ready() {
        FloorStart.RemoveChild(PillarStart);
        FloorEnd.RemoveChild(PillarEnd);
    }

    public override void _PhysicsProcess(double delta) {
        if (!IsActive || Animations.AssignedAnimation != "") { return; }

        if (FloorCenter.GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) < 1f) {
            FloorStart.AddChild(PillarStart);
            FloorEnd.AddChild(PillarEnd);
            AudioManager.PlaySound3D(Resources.Stone, PillarEnd);
            Animations.Play("Actions");
        }
    }
}
