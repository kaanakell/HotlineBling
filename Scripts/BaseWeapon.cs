using Godot;
using System;

public partial class BaseWeapon : Resource, IWeapon
{
	[Export] public string Name { get; protected set; }
	[Export] public float Damage { get; protected set; }
	[Export] public float FireRate { get; protected set; }
	[Export] public int MagazineSize { get; protected set; }
	[Export] public float ReloadTime { get; protected set; }
	[Export] public float Range { get; protected set; }
	[Export] public float ScoreMultiplier { get; protected set; } = 1f;
	[Export] public Texture2D Icon { get; protected set; }
	public virtual string AmmoText => MagazineSize > 0 ? $"{_ammo}/{MagazineSize}" : "∞";

	protected int _ammo;

	public BaseWeapon()
	{
		// Do NOT call Reload() here — MagazineSize is set in derived classes
	}

	public virtual bool Fire(Node2D shooter, Vector2 direction)
	{
		if (MagazineSize > 0 && _ammo <= 0)
		{
			GD.Print($"{Name} is out of ammo!");
			return false;
		}

		if (MagazineSize > 0)
			_ammo--;

		var space = shooter.GetWorld2D().DirectSpaceState;

		var query = PhysicsRayQueryParameters2D.Create(
			from: shooter.GlobalPosition,
			to: shooter.GlobalPosition + direction * Range
		);
		query.CollisionMask = 1;

		var excludeRids = new Godot.Collections.Array<Rid>();
		if (shooter is CollisionObject2D body)
		{
			excludeRids.Add(body.GetRid());
		}
		query.Exclude = excludeRids;

		var result = space.IntersectRay(query);

		if (result.Count == 0)
		{
			GD.Print($"[FIRE MISS] {Name} | Ammo: {AmmoText}");
			return false;
		}

		if (result["collider"].AsGodotObject() is Enemy enemy)
		{
			enemy.ApplyDamage(Damage);
			GD.Print($"[FIRE HIT] {Name} | Ammo: {AmmoText}");
			return true;
		}

		GD.Print($"[FIRE MISS] {Name} | Ammo: {AmmoText}");
		return false;
	}

	public virtual void Reload()
	{
		_ammo = MagazineSize;
	}
}
