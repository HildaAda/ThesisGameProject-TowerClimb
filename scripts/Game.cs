using Godot;
using System;

public partial class Game : Node2D
{
    public override void _Ready()
    {
        var player = GetNode<Player>("Player");
        var livesUi = GetNode<LivesUi>("LivesUI");

        player.Game = this;
        player.LivesUi = livesUi;

        livesUi.UpdateHearts(player.CurrentLives);
    }
    
    public void TriggerGameOver()
    {
        CallDeferred(nameof(DoGameOver));
    }

    private void DoGameOver()
    {
        GetTree().ChangeSceneToFile("res://scenes/game_over_menu.tscn");
    }
}
