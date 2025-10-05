using Godot;
using System;

public class DownState : PlayerState
{
    private bool animationFinished = false;

    public DownState(Player player) : base(player) { }

    public override void Enter()
    {
        player.AnimatedSprite.Play("Down");
        player.UpdateCollisionShape("Down");
        animationFinished = false;

        player.AnimatedSprite.AnimationFinished += OnAnimationFinished;
    }

    public override void Exit()
    {
        player.AnimatedSprite.AnimationFinished -= OnAnimationFinished;
    }

    private void OnAnimationFinished()
    {
        animationFinished = true;
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 input = player.GetMovementInput();
        if (input.X != 0)
        {
            player.TransitionTo(new WalkState(player));
            return;
        }

        if (animationFinished)
        {
            player.TransitionTo(new IdleState(player));
        }
    }
}
