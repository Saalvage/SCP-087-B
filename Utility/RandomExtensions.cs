using System;

public static class RandomExtensions {
    public static float NextSingle(this Random rnd, float min, float max)
        => min + rnd.NextSingle() * (max - min);
}
