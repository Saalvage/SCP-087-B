using Godot;
using System;
using System.Linq;

[SceneGlobal]
public partial class MapGenerator : Node {
    [Export] public required PackedScene Sign;
    [Export] public required PackedScene Glimpse;
    [Export] public required FloorInfo[] RandomFloors;
    [Export] public required FixedFloorActionInfo[] FixedActions;
    [Export] public required RandomFloorActionInfo[] RandomActions;

    [Export] public PackedScene?[] FloorActions = [];

    public FloorBase[]? Floors { get; private set; }

    public Random Random { get; } = new();

    public override void _Ready() {
        foreach (var fixedAct in FixedActions) {
            if (fixedAct.Probability >= 1f || Random.NextSingle() < fixedAct.Probability) {
                FloorActions[Random.Next(fixedAct.MinFloor, fixedAct.MaxFloor + 1)] = Random.RandomElement(fixedAct.Actions);
            }
        }

        foreach (var randAct in RandomActions) {
            for (var i = 0; i < randAct.Count; i++) {
                var act = Random.RandomElement(randAct.Actions);
                int rand;
                do {
                    rand = Random.Next(randAct.Start, randAct.End + 1);
                } while (FloorActions[rand] != null);
                FloorActions[rand] = act;
            }
        }

        Floors = Enumerable.Range(0, FloorActions.Length)
            .Select(i => FloorActions[i]?.Instantiate<FloorBase>() ?? new FloorBase())
            .ToArray();

        var randomFloorCumSum = RandomFloors.CumSumBy(x => x.Weight).ToArray();
        var randomFloorTotalSum = RandomFloors.Sum(x => x.Weight);

        for (var i = 0; i < Floors.Length; i++) {
            var floor = Floors[i];
            floor.Name = $"Floor{i}";

            AddChild(floor);

            var y = -i * 2f;
            floor.Transform = i % 2 == 0
                ? new(Basis.Identity, new(0f, y, 0f))
                : new(Basis.FromEuler(new(0f, MathF.PI, 0f)), new(8f, y, 7f));

            floor.AddChild((floor.PredeterminedFloor ?? RandomValidFloor()).Instantiate());

            PackedScene RandomValidFloor() {
                var selectedFloor = randomFloorCumSum
                    .First(x => Random.Next(randomFloorTotalSum) < x.CumSum).Item;
                // Assume first floor is default one.
                return i > selectedFloor.MinFloor ? selectedFloor.Floor : RandomFloors[0].Floor;
            }
        }

        for (var i = 1; i < Floors.Length; i++) {
            var sign = Sign.Instantiate<Node3D>();
            sign.GetChildren(true).OfType<Label3D>().Single().Text = i <= 140
                ? Random.Next(600) switch {
                    0 => "",
                    1 => Random.Next(33, 123).ToString(),
                    2 => "NIL",
                    3 => "?",
                    4 => "stop",
                    _ => (i + 1).ToString(),
                } : string.Join("", Enumerable.Range(0, 4)
                    .TakeWhile(x => x <= Random.Next(4))
                    .Select(_ => (char)Random.Next(33, 123)));
            sign.Transform = new(Basis.FromEuler(new(0f, MathF.PI / 2f, 0f)), new(-0.24f, -0.6f, 0.5f));
            Floors[i].AddChild(sign);

            if (Floors[i].GetType() == typeof(FloorBase) && Random.Next(7) == 0) {
                var glimpse = Glimpse.Instantiate<Node3D>();
                glimpse.Position = new(1f + Random.NextSingle() * 7f, -1f, 0.3f);
                Floors[i].AddChild(glimpse);
            }
        }
    }
}
