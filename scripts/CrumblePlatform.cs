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

        // Kuuntele animaation loppua
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
        // Animaatio jatkuu → odotetaan sen päättymistä ennen tuhoamista
        // sprite.Visible = false; // Piilota vasta kun animaatio loppuu jos haluat
    }

    private void OnAnimationFinished()
    {
        // Varmistetaan että kyseessä on Crumble-animaatio
        if (sprite.Animation == "Crumble")
        {
            QueueFree();
        }
    }
}
