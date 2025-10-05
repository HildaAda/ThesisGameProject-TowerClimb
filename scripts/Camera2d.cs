using Godot;
using System;

public partial class Camera2d : Camera2D
{
    [Export] public NodePath PlayerPath;
    private Node2D player;
    private float maxY;
    private float lockedX;

    public override void _Ready()
    {
        player = GetNode<Node2D>(PlayerPath);
        maxY = player.GlobalPosition.Y;
        lockedX = GlobalPosition.X;
    }

    public override void _Process(double delta)
    {
        float playerY = player.GlobalPosition.Y;

        if (playerY < maxY)
        {
            maxY = playerY;
            GlobalPosition = new Vector2(lockedX, maxY);
        }
    }
}
