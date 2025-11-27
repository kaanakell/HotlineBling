using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

	[Export] public float MaxSpreadDeg { get; protected set; }
	[Export] public float RecoilPerShotDeg { get; protected set; }
	[Export] public float RecoilRecoverySpeed { get; protected set; }

	protected float _currentSpread = 0f;
	public float CurrentSpread => _currentSpread;
	public bool IsReloading { get; protected set; }
	public virtual string AmmoText => IsReloading ? "RELOADING" : (MagazineSize > 0 ? $"{_ammo}/{MagazineSize}" : "∞");

	protected int _ammo;

	public void RefillAmmo()
	{
		_ammo = MagazineSize;
		IsReloading = false;
		_currentSpread = 0f;
	}

	public virtual void Update(double delta)
	{
		if (_currentSpread > 0)
		{
			_currentSpread -= RecoilRecoverySpeed * (float)delta;
			if (_currentSpread < 0) _currentSpread = 0;
		}
	}

	public virtual FireResult Fire(Node2D shooter, Vector2 direction)
	{
		var result = new FireResult();
		result.BulletEndPoints = new List<Vector2>();

		if (IsReloading) return result;

		if (MagazineSize > 0 && _ammo <= 0)
		{
			Reload(shooter);
			return result;
		}

		if (MagazineSize > 0) _ammo--;

		bool performAutoReload = (MagazineSize > 0 && _ammo <= 0);

		float halfSpreadRad = Mathf.DegToRad(_currentSpread) / 2f;
		float randomAngle = (GD.Randf() * 2 * halfSpreadRad) - halfSpreadRad;
		Vector2 finalDirection = direction.Rotated(randomAngle);

		_currentSpread = Mathf.Min(_currentSpread + RecoilPerShotDeg, MaxSpreadDeg);

		var endPoint = PerformRaycast(shooter, finalDirection);
		result.BulletEndPoints.Add(endPoint.Position);

		if (endPoint.HitEntity != null)
		{
			result.Hit = true;
			result.HitEntity = endPoint.HitEntity;
			if (result.HitEntity is Enemy enemy)
			{
				enemy.ApplyDamage(Damage);
			}
		}

		if (performAutoReload) Reload(shooter);

		return result;
	}

	protected struct RayResult { public Vector2 Position; public Node2D HitEntity; }

	protected RayResult PerformRaycast(Node2D shooter, Vector2 dir)
	{
		var space = shooter.GetWorld2D().DirectSpaceState;
		var query = PhysicsRayQueryParameters2D.Create(from: shooter.GlobalPosition, to: shooter.GlobalPosition + dir * Range);
		query.CollisionMask = 1;
		if (shooter is CollisionObject2D body)
		{
			query.Exclude = new Godot.Collections.Array<Rid> { body.GetRid() };
		}

		var hitDict = space.IntersectRay(query);

		if (hitDict.Count > 0)
		{
			return new RayResult
			{
				Position = (Vector2)hitDict["position"],
				HitEntity = hitDict["collider"].AsGodotObject() as Node2D
			};
		}

		return new RayResult { Position = shooter.GlobalPosition + dir * Range, HitEntity = null };
	}

	public virtual async void Reload(Node context)
	{
		if (IsReloading || MagazineSize <= 0) return;
		if (_ammo == MagazineSize) return;

		IsReloading = true;
		GD.Print($"{Name} starting reload... ({ReloadTime}s)");

		if (context != null && context.IsInsideTree())
		{
			await context.ToSignal(context.GetTree().CreateTimer(ReloadTime), SceneTreeTimer.SignalName.Timeout);
		}
		else
		{
			await Task.Delay((int)(ReloadTime * 1000));
		}

		_ammo = MagazineSize;
		IsReloading = false;
		GD.Print($"{Name} Reload Complete! Ammo: {AmmoText}");
	}
}
