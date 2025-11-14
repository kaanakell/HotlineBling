using Godot;
using System;

public interface IEnemy
{
    float MaxHealth { get; }
    void ApplyDamage(float damage);
}

