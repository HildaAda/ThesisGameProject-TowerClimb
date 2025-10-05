using Godot;

public class IdleState : PlayerState
{
    public IdleState(Player player) : base(player) { }

    public override void Enter()
    {
        player.AnimatedSprite.Play("Idle");
        player.UpdateCollisionShape("Idle");
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 input = player.GetMovementInput();

        // Sovelletaan painovoimaa
        player.Velocity += new Vector2(0, player.Gravity * (float)delta);

        // Liikutetaan pelaajaa
        player.MoveAndSlide();

        if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
        {
            player.TransitionTo(new JumpState(player));
            return;
        }

        if (input.X != 0)
        {
            player.TransitionTo(new WalkState(player));
        }

        // Jos tippuu ilmassa, vaihda FallStateen
        if (!player.IsOnFloor())
        {
            player.TransitionTo(new FallState(player));
        }
    }
}