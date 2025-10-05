using Godot;
using System;

public partial class ScoreUi : CanvasLayer
{
    [Export] public NodePath PlayerPath;
    [Export] public NodePath ScoreLabelPath;

    private Node2D player;
    private Label scoreLabel;
    private int score = 0;
    private float lastPlayerY;

    public override void _Ready()
    {
        player = GetNode<Node2D>(PlayerPath);
        lastPlayerY = player.GlobalPosition.Y;

        if (ScoreLabelPath != null)
            scoreLabel = GetNode<Label>(ScoreLabelPath);

        UpdateScoreLabel();
    }

    public override void _Process(double delta)
    {
        float playerY = player.GlobalPosition.Y;

        if (playerY < lastPlayerY)
        {
            score += (int)((lastPlayerY - playerY ) / 5);
            lastPlayerY = playerY;
            UpdateScoreLabel();
        }
    }

    private void UpdateScoreLabel()
    {
        if (scoreLabel != null)
            scoreLabel.Text = $"Score: {score}";

        GameManager.Instance.Score = (int)score;
    }

    public int GetScore()
    {
        return score;
    }
    
}
