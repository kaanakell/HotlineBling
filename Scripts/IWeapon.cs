using Godot;
using System;
using System.Collections.Generic;

public struct FireResult
{
	public bool Hit;
	public Node2D HitEntity;
	public List<Vector2> BulletEndPoints;
}

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
	float MaxSpreadDeg { get; }
	float RecoilPerShotDeg { get; }
	float RecoilRecoverySpeed { get; }
	bool IsReloading { get; }
	float CurrentSpread { get; }
	FireResult Fire(Node2D shooter, Vector2 direction);
	void Reload(Node context);
	void Update(double delta);
}
