using Godot;
using System;

public partial class Bat : Enemy
{
    [Export] public float Speed = 150f;
    public Vector2 Direction = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();
        
        var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play("Fly");
    }

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;

    }
}
