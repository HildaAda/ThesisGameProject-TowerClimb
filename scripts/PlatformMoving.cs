using Godot;
using System;

public partial class PlatformMoving : StaticBody2D
{
    [Export] public float WaitDurationVisible = 1.5f;
    [Export] public float WaitDurationHidden = 2.5f;

    private enum State { Hidden, Out, Visible, In }
    private State currentState = State.Hidden;

    private float timer = 0f;
    private AnimatedSprite2D sprite;
    private CollisionShape2D collider;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collider = GetNode<CollisionShape2D>("CollisionShape2D");

        sprite.Visible = false;
        collider.Disabled = true;
        timer = WaitDurationHidden;
    }

    public override void _Process(double delta)
    {
        timer -= (float)delta;

        switch (currentState)
        {
            case State.Hidden:
                if (timer <= 0f)
                {
                    sprite.Play("Out");
                    sprite.Visible = true;
                    currentState = State.Out;
                    collider.Disabled = false;

                }
                break;

            case State.Out:
                if (!sprite.IsPlaying())
                {
                    currentState = State.Visible;
                    timer = WaitDurationVisible;
                }
                break;

            case State.Visible:
                if (timer <= 0f)
                {
                    sprite.PlayBackwards("Out");
                    currentState = State.In;
                }
                break;

            case State.In:
                if (!sprite.IsPlaying())
                {
                    sprite.Visible = false;
                    collider.Disabled = true;
                    timer = WaitDurationHidden;
                    currentState = State.Hidden;
                }
                break;
        }
    }
}
