using Godot;
using System;

public class HurtState : PlayerState
{
    private float timer = 0.5f;
    private Vector2 knockback;

    public HurtState(Player player, Vector2 knockbackForce) : base(player)
    {
        knockback = knockbackForce;
    }

    public override void Enter()
    {
        player.AnimatedSprite.Play("Hurt");
        player.UpdateCollisionShape("Hurt");

        player.CurrentLives -= 1;
        player.LivesUi?.UpdateHearts(player.CurrentLives);

        if (player.CurrentLives <= 0)
        {
            player.TransitionTo(new NoHeartsState(player));
            return;
        }

        player.Velocity = knockback;
    }

    public override void PhysicsUpdate(double delta)
    {
        player.Velocity += new Vector2(0, player.Gravity * (float)delta);
        player.MoveAndSlide();

        timer -= (float)delta;
        if (timer <= 0)
        {
            if (player.IsOnFloor())
                player.TransitionTo(new IdleState(player));
            else
                player.TransitionTo(new FallState(player));
        }
    }
}
