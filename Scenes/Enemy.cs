using Godot;

public partial class Enemy : CharacterBody2D, IEnemy
{
    [Export] public float MaxHealth { get; private set; } = 100f;
    private float _hp;

    [Signal] public delegate void EnemyDiedEventHandler(Enemy enemy);

    public override void _Ready()
    {
        _hp = MaxHealth;
    }

    public void ApplyDamage(float dmg)
    {
        _hp -= dmg;
        if (_hp <= 0)
        {
            EmitSignal(SignalName.EnemyDied, this);
            QueueFree();
        }
    }
}
