using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerController : CharacterBody2D
{
	[Export] public NodePath WeaponManagerPath;
	[Export] public NodePath ScoreManagerPath;
	[Export] public Node2D WeaponPivot;

	private WeaponManager _weaponManager;
	private ScoreManager _scoreManager;
	private BaseWeapon _weapon;
	private float _fireCooldown;

	public override void _Ready()
	{
		_weaponManager = GetNode<WeaponManager>(WeaponManagerPath);
		if (HasNode(ScoreManagerPath)) _scoreManager = GetNode<ScoreManager>(ScoreManagerPath);

		_weaponManager.WeaponChanged += OnWeaponChanged;
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		OnWeaponChanged(_weaponManager.CurrentWeapon);
	}

	public override void _Draw()
	{
		Vector2 center = GetLocalMousePosition();

		float offset = 10f;

		if (_weapon != null)
		{
			offset += _weapon.CurrentSpread * 4.0f;
		}

		Color crosshairColor = Colors.LightGreen;
		float thickness = 2.0f;
		float lineLength = 10f;

		DrawCircle(center, 2f, crosshairColor);
		DrawLine(
			center - new Vector2(0, offset),
			center - new Vector2(0, offset + lineLength),
			crosshairColor, thickness
		);

		DrawLine(
			center + new Vector2(0, offset),
			center + new Vector2(0, offset + lineLength),
			crosshairColor, thickness
		);

		DrawLine(
			center - new Vector2(offset, 0),
			center - new Vector2(offset + lineLength, 0),
			crosshairColor, thickness
		);

		DrawLine(
			center + new Vector2(offset, 0),
			center + new Vector2(offset + lineLength, 0),
			crosshairColor, thickness
		);
	}

	private void OnWeaponChanged(BaseWeapon newWeapon)
	{
		_weapon = newWeapon;
		_weapon?.RefillAmmo();
		_fireCooldown = 0f;
		GD.Print($"Equipped: {newWeapon.Name}");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_fireCooldown > 0f) _fireCooldown -= (float)delta;

		_weapon?.Update(delta);

		Vector2 moveDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = moveDir * 200f;
		MoveAndSlide();

		if (WeaponPivot != null)
		{
			WeaponPivot.LookAt(GetGlobalMousePosition());
			float rotDegrees = WeaponPivot.RotationDegrees;
			if (rotDegrees > 90 || rotDegrees < -90)
				WeaponPivot.Scale = new Vector2(WeaponPivot.Scale.X, -1);
			else
				WeaponPivot.Scale = new Vector2(WeaponPivot.Scale.X, 1);
		}

		QueueRedraw();

		bool canShoot = Input.IsActionPressed("shoot")
						&& _fireCooldown <= 0f
						&& _weapon != null
						&& !_weapon.IsReloading;

		if (canShoot)
		{
			Vector2 dir = (GetGlobalMousePosition() - GlobalPosition).Normalized();

			FireResult result = _weapon.Fire(this, dir);

			if (_weapon.Range > 200 && result.BulletEndPoints != null)
			{
				foreach (var endPoint in result.BulletEndPoints)
				{
					ShowBulletTrail(WeaponPivot != null ? WeaponPivot.GlobalPosition : GlobalPosition, endPoint);
				}
			}

			_scoreManager?.OnShotFired(result.Hit);
			_fireCooldown = 1f / Mathf.Max(_weapon.FireRate, 0.01f);
		}

		if (Input.IsActionJustPressed("reload") && _weapon != null)
		{
			_weapon.Reload(this);
		}
	}

	private void ShowBulletTrail(Vector2 start, Vector2 end)
	{
		var line = new Line2D();
		line.Points = new Vector2[] { start, end };
		line.Width = 2.0f;
		line.DefaultColor = Colors.Yellow;
		line.TopLevel = true;
		GetTree().Root.AddChild(line);

		var tween = CreateTween();
		tween.TweenProperty(line, "modulate:a", 0.0f, 0.1f);
		tween.TweenCallback(Callable.From(line.QueueFree));
	}
}
