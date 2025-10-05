using Godot;
using System;

public partial class CrumblePlatform : StaticBody2D
{
    [Export] public float CrumbleDelay = 0.6f;

    private AnimatedSprite2D sprite;
    private CollisionShape2D collision;
    private Timer timer;
    private Area2D triggerArea;

    private bool isCrumbling = false;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collision = GetNode<CollisionShape2D>("CollisionShape2D");
        timer = GetNode<Timer>("Timer");
        triggerArea = GetNode<Area2D>("Area2D");

        timer.OneShot = true;
        timer.WaitTime = CrumbleDelay;

        triggerArea.BodyEntered += OnBodyEntered;
        timer.Timeout += OnCrumble;

        sprite.AnimationFinished += OnAnimationFinished;
    }

    private void OnBodyEntered(Node body)
    {
        if (isCrumbling || !(body is CharacterBody2D)) return;

        isCrumbling = true;
        sprite.Play("Crumble");
        timer.Start();
    }

    private void OnCrumble()
    {
        collision.Disabled = true;
    }

    private void OnAnimationFinished()
    {
        if (sprite.Animation == "Crumble")
        {
            QueueFree();
        }
    }
}
