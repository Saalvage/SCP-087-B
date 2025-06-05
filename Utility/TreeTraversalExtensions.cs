using Godot;
using System.Collections.Generic;

public static class TreeTraversalExtensions {
    public static IEnumerable<Node> EnumerateDescendantsDepthFirst(this Node node, bool includeInternal = false) {
        foreach (var child in node.GetChildren(includeInternal)) {
            yield return child;
            foreach (var descendant in EnumerateDescendantsDepthFirst(child, includeInternal)) {
                yield return descendant;
            }
        }
    }
}
