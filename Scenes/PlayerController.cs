using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export] public NodePath WeaponManagerPath;
	[Export] public NodePath ScoreManagerPath;

	private WeaponManager _weaponManager;
	private ScoreManager _scoreManager;
	private IWeapon _weapon;
	private float _fireCooldown;

	public override void _Ready()
	{
		_weaponManager = GetNode<WeaponManager>(WeaponManagerPath);
		_scoreManager = GetNode<ScoreManager>(ScoreManagerPath);

		_weaponManager.WeaponChanged += OnWeaponChanged;
		OnWeaponChanged(_weaponManager.CurrentWeapon);
	}

	private void OnWeaponChanged(BaseWeapon newWeapon)
	{
		_weapon = newWeapon;
		GD.Print("════════════════════════════════");
		GD.Print($"WEAPON CHANGED → {newWeapon.Name}");
		GD.Print($"   Damage: {newWeapon.Damage}");
		GD.Print($"   Fire Rate: {newWeapon.FireRate:F1} shots/sec");
		GD.Print($"   Range: {newWeapon.Range}");
		GD.Print($"   Score Multiplier: {newWeapon.ScoreMultiplier:F1}");
		GD.Print($"   Ammo: {newWeapon.AmmoText}");
		GD.Print("════════════════════════════════");
		_fireCooldown = 0f;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_fireCooldown > 0f)
		{
			_fireCooldown -= (float)delta;
		}

		Vector2 moveDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = moveDir * 200f;
		MoveAndSlide();

		bool canShoot = Input.IsActionPressed("shoot") && _fireCooldown <= 0f && _weapon != null;
		if (Input.IsActionPressed("shoot"))
		{
			GD.Print($"[SHOOT ATTEMPT] Cooldown: {_fireCooldown:F1}s | CanShoot: {canShoot}");
		}

		if (canShoot)
		{
			Vector2 dir = (GetGlobalMousePosition() - GlobalPosition).Normalized();
			bool hit = _weapon.Fire(this, dir);

			GD.Print($"SHOT → {_weapon.Name} | Ammo: {_weapon.AmmoText} | {(hit ? "HIT!" : "MISS")}");

			_scoreManager?.OnShotFired(hit);
			_fireCooldown = 1f / Mathf.Max(_weapon.FireRate, 0.01f);
			GD.Print($"[NEW COOLDOWN] Set to {_fireCooldown:F1}s");
		}

		if (Input.IsActionJustPressed("reload") && _weapon != null)
		{
			_weapon.Reload();
			GD.Print($"RELOAD → {_weapon.Name} → {_weapon.AmmoText}");
		}
	}
}
