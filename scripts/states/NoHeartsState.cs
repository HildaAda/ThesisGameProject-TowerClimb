using Godot;
using System;

public class NoHeartsState : PlayerState
{
    private bool gameOverTriggered = false;
    public NoHeartsState(Player player) : base(player){}

    public override void Enter()
    {
        player.AnimatedSprite.Play("Hurt");
        player.SetCollisionLayerValue(1, false); 
        player.SetCollisionMaskValue(1, false);
        player.Velocity = new Vector2(0, -400);
    }

    public override void PhysicsUpdate(double delta)
    {
        player.Velocity += new Vector2(0, player.Gravity * (float)delta);

        player.MoveAndSlide();

        if (!gameOverTriggered && player.Position.Y > player.GetViewportRect().Size.Y + 100)
        {
            gameOverTriggered = true;
            player.Game.CallDeferred(nameof(Game.TriggerGameOver));
        }
    }
}
