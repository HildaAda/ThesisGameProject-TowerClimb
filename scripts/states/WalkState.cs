using Godot;

public class WalkState : PlayerState
{
    public WalkState(Player player) : base(player) { }

    public override void Enter()
    {
        player.AnimatedSprite.Play("Walk");
        player.UpdateCollisionShape("Idle");
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 input = player.GetMovementInput();
        Vector2 velocity = player.Velocity;

        if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
        {
            player.TransitionTo(new JumpState(player));
            return;
        }

        velocity.X = input.X * player.Speed;
        velocity.Y += player.Gravity * (float)delta;

        if (input.X != 0)
            player.AnimatedSprite.FlipH = input.X < 0;

        player.Velocity = velocity;
        player.MoveAndSlide();

        if (input.X == 0)
        {
            player.TransitionTo(new IdleState(player));
        }

        if (!player.IsOnFloor() && velocity.Y > 0)
        {
            player.TransitionTo(new FallState(player));
        }
    }
}
