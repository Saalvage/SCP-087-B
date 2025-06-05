using System.Globalization;
using Godot;

public partial class FPSCounter : Label {
    public override void _PhysicsProcess(double delta) {
        Text = Engine.GetFramesPerSecond().ToString(CultureInfo.InvariantCulture);
    }
}
