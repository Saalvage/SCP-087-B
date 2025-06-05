#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[Tool]
public partial class RequiredPropertyHelper : EditorPlugin {
    [GeneratedRegex(@"public[\n ]+partial[\n ]+class[\n ]+(?<ClassName>[@A-Za-z][A-Za-z0-9]*)")]
    private static partial Regex ClassNameExtractorRegex();

    public override void _ApplyChanges() {
        foreach (var node in EnumerateChildren(GetTree().EditedSceneRoot)) {
            var sourceCode = (node.GetScript().AsGodotObject() as CSharpScript)?.SourceCode;

            if (sourceCode == null) { continue; }

            foreach (Match match in ClassNameExtractorRegex().Matches(sourceCode)) {
                var name = match.Groups["ClassName"].Value;
                
                var type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetExportedTypes())
                    .FirstOrDefault(x => x.Name == name);
                    
                if (type == null) { continue; }

                foreach (var member in type.GetMembers()) {
                    if (member.IsDefined(typeof(ExportAttribute), true)
                        && member.IsDefined(typeof(RequiredMemberAttribute))
                        && node.Get(member.Name).VariantType == Variant.Type.Nil) {
                        GD.PushError($"Required member {member.Name} in type {type.Name} is not set.");
                    }
                }
            }
        }

        static IEnumerable<Node> EnumerateChildren(Node node, bool includeInternal = false) {
            foreach (var child in node.GetChildren(includeInternal)) {
                yield return child;
                foreach (var descendant in EnumerateChildren(child, includeInternal)) {
                    yield return descendant;
                }
            }
        }
    }
}
#endif
