using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    public int Score = 0;

    public override void _Ready()
    {
        Instance = this;
    }
}
