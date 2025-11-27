using Godot;
using System;
using System.Collections.Generic;

public partial class WeaponFactory : Object
{
	private readonly List<Func<BaseWeapon>> _weaponConstructors = new()
	{
		() => new SniperWeapon(),
		() => new SMGWeapon(),
		() => new AssaultRifleWeapon(),
		() => new ShotgunWeapon(),
		() => new KatanaWeapon(),
		() => new CrowbarWeapon()
	};

	public BaseWeapon GetRandomWeapon()
	{
		int index = (int)(GD.Randi() % _weaponConstructors.Count);
		var weapon = _weaponConstructors[index]();
		weapon.RefillAmmo();
		return weapon;
	}
}
