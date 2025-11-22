using Godot;
using System;
using System.Collections.Generic;

public partial class WeaponFactory : Object
{
	private readonly List<Func<BaseWeapon>> _weaponConstructors = new()
	{
		() => new SniperWeapon(),
	};

	public BaseWeapon GetRandomWeapon()
	{
		int index = ((int)GD.Randi()) % _weaponConstructors.Count;
		var weapon = _weaponConstructors[index]();
		weapon.Reload(); // start with full magazine (MagazineSize can be 0 for melee)
		return weapon;
	}
}
