using Godot;
using System;

public interface IWeapon
{
	string Name { get; }
	float Damage { get; }
	float FireRate { get; }
	int MagazineSize { get; }
	float ReloadTime { get; }
	float Range { get; }
	float ScoreMultiplier { get; }
	string AmmoText { get; }
	bool Fire(Node2D shooter, Vector2 direction);   // ← now returns bool for accuracy
	void Reload();
}
