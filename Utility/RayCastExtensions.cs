using System.Linq;
using Godot;
using Godot.Collections;

public static class RayCastExtensions {
    public static bool IsVisible(this World3D world, Vector3 from, Vector3 to, Array<Rid>? exclude = null)
        => !world.DirectSpaceState.IntersectRay(
            PhysicsRayQueryParameters3D.Create(from, to, exclude: exclude)
        ).Any();
}
