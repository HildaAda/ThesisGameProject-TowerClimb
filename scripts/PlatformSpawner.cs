using Godot;
using System;
using System.Collections.Generic;

public partial class PlatformSpawner : Node2D
{
    [Export] public Godot.Collections.Array<PackedScene> PlatformSets;
    [Export] public PackedScene StartingPlatformSet;
    [Export] public float SpawnIntervalY = 380f;
    [Export] public float DespawnDistanceBelow = 200f;

    private float nextSpawnY;
    private List<Node2D> spawnedSets = new List<Node2D>();
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private float PlayableAreaStartX = 0f;
    private float PlayableAreaEndX = 288f;

    public override void _Ready()
    {
        rng.Randomize();

        nextSpawnY = 0f;

        if (StartingPlatformSet != null)
        {
            var startInstance = (Node2D)StartingPlatformSet.Instantiate();
            startInstance.Position = new Vector2(PlayableAreaStartX, nextSpawnY);
            AddChild(startInstance);
            spawnedSets.Add(startInstance);
            nextSpawnY -= SpawnIntervalY;
        }

        for (int i = 0; i < 4; i++)
        {
            SpawnPlatformSet();
        }
    }

    public override void _Process(double delta)
    {
        var cam = GetViewport().GetCamera2D();
        if (cam == null) return;

        float camTop = cam.GlobalPosition.Y - GetViewport().GetVisibleRect().Size.Y / 2f;
        float camBottom = cam.GlobalPosition.Y + GetViewport().GetVisibleRect().Size.Y / 2f;

        while (nextSpawnY > camTop - 100f)
        {
            SpawnPlatformSet();
        }

        for (int i = spawnedSets.Count - 1; i >= 0; i--)
        {
            var set = spawnedSets[i];
            if (!GodotObject.IsInstanceValid(set)) continue;

            if (set.GlobalPosition.Y > camBottom + DespawnDistanceBelow)
            {
                set.QueueFree();
                spawnedSets.RemoveAt(i);
            }
        }
    }

    private void SpawnPlatformSet()
    {
        if (PlatformSets.Count == 0)
            return;

        var setScene = PlatformSets[rng.RandiRange(0, PlatformSets.Count - 1)];
        var setInstance = (Node2D)setScene.Instantiate();

        bool flipHorizontally = rng.Randf() < 0.5f;
        float playableWidth = PlayableAreaEndX - PlayableAreaStartX;

        if (flipHorizontally)
        {
            setInstance.Scale = new Vector2(-1f, 1f);
            setInstance.Position = new Vector2(PlayableAreaStartX + playableWidth, nextSpawnY);
        }
        else
        {
            setInstance.Position = new Vector2(PlayableAreaStartX, nextSpawnY);
        }

        AddChild(setInstance);
        spawnedSets.Add(setInstance);

        nextSpawnY -= SpawnIntervalY;
    }
}

