using Godot;
using System;
using System.Collections.Generic;

public partial class BatSpawner : Node2D
{
    [Export] public PackedScene BatScene;
    [Export] public float SpawnInterval = 2f;
    [Export] public float PlayableAreaWidth = 288f;
    [Export] public float PlayableAreaHeight = 324f;
    [Export] public float SpawnOffset = 50f;

    private float timer = 0f;
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private List<Bat> activeBats = new List<Bat>();

    public override void _Ready()
    {
        rng.Randomize();
    }

    public override void _Process(double delta)
    {
        timer -= (float)delta;

        if (timer <= 0f)
        {
            SpawnBat();
            timer = SpawnInterval;
        }
    }

    private void SpawnBat()
    {
        if (BatScene == null)
        return;

    var bat = BatScene.Instantiate<Bat>();
    AddChild(bat);
    activeBats.Add(bat);

    bool fromTop = rng.Randf() < 0.5f;

    var cam = GetViewport().GetCamera2D();
    if (cam == null)
    {
        GD.PrintErr("No active Camera2D found!");
        return;
    }

    float halfHeight = GetViewport().GetVisibleRect().Size.Y * cam.Zoom.Y * 0.5f;
    float halfWidth  = GetViewport().GetVisibleRect().Size.X * cam.Zoom.X * 0.5f;

    float x = rng.RandfRange(cam.GlobalPosition.X - 160f, cam.GlobalPosition.X + 160f);

    float y = fromTop
        ? cam.GlobalPosition.Y - halfHeight - SpawnOffset
        : cam.GlobalPosition.Y + halfHeight + SpawnOffset;

    bat.GlobalPosition = new Vector2(x, y);
    bat.Direction = fromTop ? Vector2.Down : Vector2.Up;

    bat.TreeExiting += () => activeBats.Remove(bat);
        
    }
}