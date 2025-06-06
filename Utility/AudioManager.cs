using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AudioManager : Node {
    private static AudioManager _instance = null!;

    private readonly List<AudioStreamPlayer> _audioPlayers = [];
    private readonly List<AudioStreamPlayer3D> _audioPlayers3D = [];

    public override void _Ready() {
        _instance = this;
    }

    public static void PlaySound(AudioStream stream, Action? onFinished = null)
        => _instance.PlaySoundInternal(stream, onFinished);
    private void PlaySoundInternal(AudioStream stream, Action? onFinished = null) {
        var player = _audioPlayers.FirstOrDefault(x => !x.Playing);
        if (player == null) {
            player = new();
            AddChild(player);
            _audioPlayers.Add(player);
        }

        if (onFinished != null) {
            player.Finished += FinishedCallback;
            void FinishedCallback() {
                onFinished();
                player.Finished -= FinishedCallback;
            }
        }

        player.Stream = stream;
        player.Play();
    }

    public static void PlaySound3D(AudioStream stream, Vector3 origin, Action? onFinished = null) {
        var node = new Node3D { Position = origin };
        _instance.AddChild(node);
        _instance.PlaySoundInternal3D(stream, node, () => {
            node.QueueFree();
            onFinished?.Invoke();
        });
    }

    public static void PlaySound3D(AudioStream stream, Node3D origin, Action? onFinished = null)
        => _instance.PlaySoundInternal3D(stream, origin, onFinished);
    private void PlaySoundInternal3D(AudioStream stream, Node3D origin, Action? onFinished) {
        var player = _audioPlayers3D.FirstOrDefault(x => !x.Playing);
        if (player == null) {
            player = new();
            _audioPlayers3D.Add(player);
        }
        origin.AddChild(player);
        origin.TreeExiting += ReparentPlayer;
        void ReparentPlayer() {
            origin.RemoveChild(player);
            AddChild(player);
            player.GlobalTransform = origin.GlobalTransform * player.Transform;
        }

        player.Finished += FinishedCallback;
        void FinishedCallback() {
            onFinished?.Invoke();

            if (IsInstanceValid(origin)) {
                origin.RemoveChild(player);
                origin.TreeExiting -= ReparentPlayer;
            } else {
                RemoveChild(player);
            }
            player.Finished -= FinishedCallback;
        }

        player.Stream = stream;
        player.Play();
    }
}
