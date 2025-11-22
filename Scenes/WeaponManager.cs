using Godot;
using System;

public partial class WeaponManager : Node
{
	[Signal]
	public delegate void WeaponChangedEventHandler(BaseWeapon newWeapon); // ← concrete type

	[Export] public float RotationInterval = 60f;

	private Timer _rotationTimer;
	private WeaponFactory _factory = new();

	public BaseWeapon CurrentWeapon { get; private set; }

	public override void _Ready()
	{
		_rotationTimer = new Timer();
		_rotationTimer.WaitTime = RotationInterval;
		_rotationTimer.OneShot = false;
		_rotationTimer.Autostart = true;
		AddChild(_rotationTimer);
		_rotationTimer.Timeout += RollWeapon;

		RollWeapon();
	}

	private void RollWeapon()
	{
		BaseWeapon weapon = _factory.GetRandomWeapon();
		CurrentWeapon = weapon;
		EmitSignal(SignalName.WeaponChanged, weapon);
	}
}
