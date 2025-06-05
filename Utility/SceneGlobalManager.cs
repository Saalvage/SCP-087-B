using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class SceneGlobalManager : Node {
    private readonly Dictionary<Type, Node> _sceneGlobals = [];
    private readonly Dictionary<Type, bool> _testedTypes = [];

    public override void _Ready() {
        var descendants = GetTree().Root.EnumerateDescendantsDepthFirst(true);

        // Two passes, first collect all scene globals, then apply them.
        foreach (var node in descendants) {
            HandleNode(node);
        }

        foreach (var node in descendants) {
            HandleNode(node);
        }

        GetTree().NodeAdded += HandleNode;


        void HandleNode(Node node) {
            var type = node.GetType();

            if (type.IsDefined(typeof(SceneGlobalAttribute), false)) {
                _sceneGlobals[type] = node;
            }

            if (!_testedTypes.TryGetValue(type, out var containsSceneGlobal)) {
                containsSceneGlobal = type.GetFields()
                    .Select(x => x.FieldType)
                    .Concat(type.GetProperties().Select(x => x.PropertyType))
                    .Any(x => x.IsDefined(typeof(SceneGlobalAttribute), false));
                _testedTypes.Add(type, containsSceneGlobal);
            }

            if (!containsSceneGlobal) { return; }

            foreach (var (prop, global) in type.GetProperties()
                         .Select(x => (Prop: x, Global: _sceneGlobals.GetValueOrDefault(x.PropertyType)))
                         .Where(x => x.Global != null)) {
                prop.SetValue(node, global);
            }

            foreach (var (field, global) in type.GetFields()
                         .Select(x => (Field: x, Global: _sceneGlobals.GetValueOrDefault(x.FieldType)))
                         .Where(x => x.Global != null)) {
                field.SetValue(node, global);
            }
        };

    }
}
