using Godot;

public static class Helpers {
    public static int GetFloor(Vector3 pos) => (int)(-pos.Y / 2f);

    public static void SetBrightness(byte b) => SetBrightness(b, b, b);
    public static void SetBrightness(byte r, byte g, byte b) => SetBrightness(Color.Color8(r, g, b));
    public static void SetBrightness(Color color)
        => RenderingServer.GlobalShaderParameterSet("brightness", color);
}
