using Godot;
using System;

public partial class Bat : Enemy
{
    [Export] public float Speed = 150f; // Pikseleitä sekunnissa
    public Vector2 Direction = Vector2.Zero; // Liikesuunta

    public override void _Ready()
    {
        base._Ready();
        
        var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play("Fly"); // Toistaa Fly-animaation loopaten
    }

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;

    }
}
