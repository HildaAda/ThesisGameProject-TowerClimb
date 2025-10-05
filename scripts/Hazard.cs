using Godot;
using System;

public partial class Hazard : Node
{
    [Export] public string TargetGroup = "Player";
    [Export] public float KnockbackX = 45f;
    [Export] public float KnockbackY = -300f;

    public override void _Ready()
    {
        var area = GetNode<Area2D>("Area2D");
        area.BodyEntered += OnBodyEntered;
    }

    protected virtual void OnBodyEntered(Node body)
    {
        if (body.IsInGroup(TargetGroup) && body is Player player)
        {
            float direction = player.AnimatedSprite.FlipH ? 1f : -1f;
            Vector2 knockback = new Vector2(KnockbackX * direction, KnockbackY);

            player.TransitionTo(new HurtState(player, knockback));
        }
    }
}
