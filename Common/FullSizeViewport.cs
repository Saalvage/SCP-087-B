using Godot;

public partial class FullSizeViewport : SubViewport {
    public override void _Ready() {
        var window = GetWindow();
        Size = window.Size;
        window.SizeChanged += () => Size = window.Size;
    }
}
