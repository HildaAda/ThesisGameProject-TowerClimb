using Godot;
using System;

public class JumpState : PlayerState
{
    private float jumpVelocity = -550f;
    private Vector2 velocity;

    public JumpState(Player player) : base(player) { }

    public override void Enter()
    {
        player.AnimatedSprite.Play("Jump");
        player.UpdateCollisionShape("Jump");

        velocity = player.Velocity;
        velocity.Y = jumpVelocity;
        player.Velocity = velocity;
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 input = player.GetMovementInput();
        velocity = player.Velocity;

        bool isJumpHeld = Input.IsActionPressed("jump");

        if (velocity.Y < 0 && !isJumpHeld)
        {
            velocity.Y += player.Gravity * 1.5f * (float)delta;
        }
        else
        {
            velocity.Y += player.Gravity * (float)delta;
        }

        velocity.X = input.X * player.Speed;

        if (input.X != 0)
            player.AnimatedSprite.FlipH = input.X < 0;

        player.Velocity = velocity;
        player.MoveAndSlide();

        if (velocity.Y > 0)
        {
            player.TransitionTo(new FallState(player));
        }

        if (player.IsOnFloor())
        {
            player.TransitionTo(new DownState(player));
        }
    }
}