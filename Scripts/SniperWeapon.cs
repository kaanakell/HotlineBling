using Godot;

public partial class SniperWeapon : BaseWeapon
{
	public SniperWeapon()
	{
		Name = "Sniper";
		Damage = 100f;
		FireRate = 0.5f;         // 2 seconds between shots → change if you want faster
		MagazineSize = 1;
		ReloadTime = 1.8f;
		Range = 1200f;
		ScoreMultiplier = 1.6f;
		// Icon = GD.Load<Texture2D>("res://icons/sniper.png"); // optional
	}

	// If you don’t need special behavior, just call base and return the result
	public override bool Fire(Node2D shooter, Vector2 direction)
	{
		return base.Fire(shooter, direction);
	}
}
