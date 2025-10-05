using Godot;
using System;
using System.Collections.Generic;

public partial class PlatformSpawner : Node2D
{
    [Export] public Godot.Collections.Array<PackedScene> PlatformSets;
    [Export] public PackedScene StartingPlatformSet;
    [Export] public float SpawnIntervalY = 380f; // Väli setille pystysuunnassa
    [Export] public float DespawnDistanceBelow = 200f;

    private float nextSpawnY;
    private List<Node2D> spawnedSets = new List<Node2D>();
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private float PlayableAreaStartX = 0f;
    private float PlayableAreaEndX = 288f;

    public override void _Ready()
    {
        rng.Randomize();

        // Aloitussetti
        nextSpawnY = 0f; // Voidaan laittaa nollaan tai haluttuun alkuun

        if (StartingPlatformSet != null)
        {
            var startInstance = (Node2D)StartingPlatformSet.Instantiate();
            startInstance.Position = new Vector2(PlayableAreaStartX, nextSpawnY);
            AddChild(startInstance);
            spawnedSets.Add(startInstance);
            nextSpawnY -= SpawnIntervalY;
        }

        // Alkuun muut setit
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

        // Spawnataan uusia settejä, jos kamera lähestyy seuraavaa spawnkohtaa
        while (nextSpawnY > camTop - 100f)
        {
            SpawnPlatformSet();
        }

        // Poistetaan setit, jotka ovat liian alhaalla kameran alareunaan nähden
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

        // Satunnainen peilaus vaakasuunnassa
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

