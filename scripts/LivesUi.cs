using Godot;
using System;

public partial class LivesUi : CanvasLayer
{
    private TextureRect[] hearts;

    public override void _Ready()
    {
        hearts = new TextureRect[9];
        for (int i = 0; i < 9; i++)
            hearts[i] = GetNode<TextureRect>($"HBoxContainer/Heart{i + 1}");
    }

    public void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < 9; i++)
            hearts[i].Visible = i < currentLives;
    }
}
