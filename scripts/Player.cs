using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed = 100;
    [Export] public int Gravity = 1200;
    [Export] public int MaxLives = 9;

    public AnimatedSprite2D AnimatedSprite;
    public int CurrentLives;
    public LivesUi LivesUi { get; set; }
    public Game Game { get; set; }

    private PlayerState currentState;
    private CollisionShape2D collisionIdle;
    private CollisionShape2D collisionJump;
    private CollisionShape2D collisionFall;
    private CollisionShape2D collisionDown;
    
    private float originalRotationIdle;
    private float originalRotationJump;
    private float originalRotationFall;
    private float originalRotationDown;

    public override void _Ready()
    {
        CurrentLives = MaxLives;
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        collisionIdle = GetNode<CollisionShape2D>("CollisionIdle");
        collisionJump = GetNode<CollisionShape2D>("CollisionJump");
        collisionFall = GetNode<CollisionShape2D>("CollisionFall");
        collisionDown = GetNode<CollisionShape2D>("CollisionDown");

        originalRotationIdle = collisionIdle.Rotation;
        originalRotationJump = collisionJump.Rotation;
        originalRotationFall = collisionFall.Rotation;
        originalRotationDown = collisionDown.Rotation;

        TransitionTo(new IdleState(this));
    }

    public void TransitionTo(PlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState?.PhysicsUpdate(delta);

        var cam = GetViewport().GetCamera2D();
        if (cam != null)
        {
            Vector2 camPos = cam.GlobalPosition;
            Vector2 playerPos = GlobalPosition;

            float camBottom = camPos.Y + GetViewport().GetVisibleRect().Size.Y / 2f;
            if (playerPos.Y > camBottom - 10f)
            {
                GD.Print($"Pelaaja tippui! Game Over. PelaajaY: {playerPos.Y}, CamY: {camBottom}");
                GetTree().ChangeSceneToFile("res://scenes/game_over_menu.tscn");
            }
        }

    }

    public Vector2 GetMovementInput()
    {
        Vector2 dir = Vector2.Zero;
        if (Input.IsActionPressed("move_right")) dir.X += 1;
        if (Input.IsActionPressed("move_left")) dir.X -= 1;
        return dir;
    }

    public void UpdateCollisionShape(string anim)
    {
        CallDeferred(nameof(DeferredUpdateCollisionShape), anim);
    }

    private void DeferredUpdateCollisionShape(string anim)
    {
        collisionIdle.Disabled = anim != "Idle";
        collisionJump.Disabled = anim != "Jump";
        collisionFall.Disabled = anim != "Fall";
        collisionDown.Disabled = anim != "Down";

        CollisionShape2D activeCollision = anim switch
        {
            "Idle" => collisionIdle,
            "Jump" => collisionJump,
            "Fall" => collisionFall,
            "Down" => collisionDown,
            "Hurt" => collisionIdle,
            _ => null
        };

        if (activeCollision != null)
        {

            var originalPos = new Vector2(Mathf.Abs(activeCollision.Position.X), activeCollision.Position.Y);
            activeCollision.Position = AnimatedSprite.FlipH
                ? new Vector2(-originalPos.X, originalPos.Y)
                : originalPos;
                
            float baseRotation = anim switch
            {
                "Idle" => originalRotationIdle,
                "Jump" => originalRotationJump,
                "Fall" => originalRotationFall,
                "Down" => originalRotationDown,
                "Hurt" => originalRotationIdle,
                _ => 0f
            };

            activeCollision.Rotation = AnimatedSprite.FlipH ? -baseRotation : baseRotation;
        }
    }
}
