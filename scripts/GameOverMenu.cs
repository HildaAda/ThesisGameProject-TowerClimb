using Godot;
using System;
using System.Collections.Generic;

public partial class GameOverMenu : Control
{
    private Sprite2D paw;
    private Label scoreLabel;
    private List<TextureButton> buttons = new();
    private int currentIndex = 0;

    public override void _Ready()
    {
        paw = GetNode<Sprite2D>("Paw");

        buttons.Add(GetNode<TextureButton>("VBoxContainer/PlayAgainButton"));
        buttons.Add(GetNode<TextureButton>("VBoxContainer/MainMenuButton"));

        scoreLabel = GetNode<Label>("VBoxContainer/HBoxContainer/ScoreLabel");
        scoreLabel.Text = $"{GameManager.Instance.Score}";

        UpdatePawPosition();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("menu_down"))
        {
            currentIndex = (currentIndex + 1) % buttons.Count;
            UpdatePawPosition();
        }
        else if (@event.IsActionPressed("menu_up"))
        {
            currentIndex = (currentIndex - 1 + buttons.Count) % buttons.Count;
            UpdatePawPosition();
        }
        else if (@event.IsActionPressed("menu_accept"))
        {
            OnButtonPressed(currentIndex);
        }
    }

    private void UpdatePawPosition()
    {
        var selectedButton = buttons[currentIndex];
        Vector2 buttonPos = selectedButton.GlobalPosition;
        Vector2 offset = new Vector2(295, 36);
        paw.GlobalPosition = buttonPos + offset;
    }

    private void OnButtonPressed(int index)
    {
        if (index == 0)
        {
            GetTree().ChangeSceneToFile("res://scenes/game.tscn");
        }
        else if (index == 1)
        {
            GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
        }
    }
}
