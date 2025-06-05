using System;
using System.Linq;
using Godot;

[SceneGlobal]
public partial class GameCoordinator : Node3D {
    [Export] public required AudioStream[] Ambient;

    public required MapGenerator MapGenerator;
    public required PlayerController Player;

    private int _prevFloor;

    public override void _Ready() {
        MapGenerator.Floors?[0].OnEnter();

        foreach (var floor in MapGenerator.Floors?.Skip(2) ?? []) {
            floor.Visible = false;
        }
    }

    public override void _PhysicsProcess(double delta) {
        var newPlayerFloor = Player.Floor - 1;
        if (newPlayerFloor != _prevFloor && newPlayerFloor >= 0 && newPlayerFloor < MapGenerator.Floors?.Length) {
            for (var i = -1; i <= 1; i++) {
                TrySet(_prevFloor + i, false);
            }

            MapGenerator.Floors[_prevFloor].OnLeave();
            MapGenerator.Floors[newPlayerFloor].OnEnter();
            _prevFloor = newPlayerFloor;

            for (var i = -1; i <= 1; i++) {
                TrySet(newPlayerFloor + i, true);
            }

            void TrySet(int i, bool visible) {
                if (i >= 0 && i < MapGenerator.Floors?.Length) {
                    MapGenerator.Floors[i].Visible = visible;
                }
            }
        }

        if (Random.Shared.Next(1000) == 0) {
            AudioManager.PlaySound3D(Ambient.RandomElement(),
                Player.GlobalPosition
                + new Vector3(Random.Shared.NextSingle(-1f, 1f),
                    Random.Shared.NextSingle(-10f, -2f),
                    Random.Shared.NextSingle(-1f, 1f)));
        }
    }
}
