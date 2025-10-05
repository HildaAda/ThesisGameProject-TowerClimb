using Godot;
using System;

public class FallState : PlayerState
{
    private float gravity = 1200f;
    private Vector2 velocity;

    public FallState(Player player) : base(player) { }

    public override void Enter()
    {
        player.AnimatedSprite.Play("Fall");
        player.UpdateCollisionShape("Fall");
        velocity = player.Velocity;
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 input = player.GetMovementInput();
        velocity = player.Velocity;

        velocity.Y += gravity * (float)delta;
        velocity.X = input.X * player.Speed;

        if (input.X != 0)
            player.AnimatedSprite.FlipH = input.X < 0;

        player.Velocity = velocity;
        player.MoveAndSlide();

        if (player.IsOnFloor())
        {
            player.TransitionTo(new DownState(player));
        }
    }
}
