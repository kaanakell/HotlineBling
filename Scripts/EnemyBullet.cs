using Godot;

public partial class EnemyBullet : Area2D
{
    [Export] public float Speed = 400f;
    [Export] public float Damage = 100f;
    [Export] public float Lifetime = 5.0f;

    private Vector2 _direction;

    public override void _Ready()
    {
        ZIndex = 10;
        GetTree().CreateTimer(Lifetime).Timeout += QueueFree;

        BodyEntered += OnBodyEntered;
    }

    public void Setup(Vector2 direction)
    {
        _direction = direction.Normalized();
        Rotation = _direction.Angle();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * Speed * (float)delta;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is PlayerController player)
        {
            player.Die();
            QueueFree();
        }

        else if (body is TileMapLayer || body is StaticBody2D)
        {
            QueueFree();
        }
    }
}
