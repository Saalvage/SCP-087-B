using Godot;

public partial class InputForwarder : Node {
    [Export] public required SubViewport Viewport;

    public override void _Input(InputEvent @event) {
        Viewport.PushInput(@event);
    }
}
