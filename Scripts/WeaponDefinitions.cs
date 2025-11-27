using Godot;
using System.Collections.Generic;

public partial class SniperWeapon : BaseWeapon
{
    public SniperWeapon()
    {
        Name = "Sniper";
        Damage = 100f;
        FireRate = 0.5f;
        MagazineSize = 1;
        ReloadTime = 1.5f;
        Range = 1500f;
        ScoreMultiplier = 1.5f;

        MaxSpreadDeg = 0f;
        RecoilPerShotDeg = 0f;

        //Icon = GD.Load<Texture2D>("res://icons/sniper.png");
    }
}

public partial class SMGWeapon : BaseWeapon
{
    public SMGWeapon()
    {
        Name = "Uzi";
        Damage = 15f;
        FireRate = 12f;
        MagazineSize = 30;
        ReloadTime = 1.2f;
        Range = 600f;
        ScoreMultiplier = 1.2f;

        MaxSpreadDeg = 30f;       
        RecoilPerShotDeg = 2.0f;  
        RecoilRecoverySpeed = 12f;

        //Icon = GD.Load<Texture2D>("res://icons/uzi.png");
    }
}

public partial class AssaultRifleWeapon : BaseWeapon
{
    public AssaultRifleWeapon()
    {
        Name = "M16";
        Damage = 35f;
        FireRate = 8f;
        MagazineSize = 24;
        ReloadTime = 1.4f;
        Range = 900f;
        ScoreMultiplier = 1.0f;

        MaxSpreadDeg = 15f;
        RecoilPerShotDeg = 1.5f;
        RecoilRecoverySpeed = 6f;

        //Icon = GD.Load<Texture2D>("res://icons/assault_rifle.png");
    }
}

public partial class ShotgunWeapon : BaseWeapon
{
    public int PelletCount { get; set; } = 6;
    public float BaseSpreadAngle { get; set; } = 15f;

    public ShotgunWeapon()
    {
        Name = "Shotgun";
        Damage = 20f;
        FireRate = 1.2f;
        MagazineSize = 6;
        ReloadTime = 2.0f;
        Range = 450f;
        ScoreMultiplier = 1.5f;

        MaxSpreadDeg = 15f;
        RecoilPerShotDeg = 8f;
        RecoilRecoverySpeed = 5f;

        //Icon = GD.Load<Texture2D>("res://icons/shotgun.png");
    }

    public override FireResult Fire(Node2D shooter, Vector2 direction)
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
        bool performAutoReload = MagazineSize > 0 && _ammo <= 0;

        float halfRecoil = Mathf.DegToRad(_currentSpread) / 2f;
        float recoilAngle = (GD.Randf() * 2 * halfRecoil) - halfRecoil;
        Vector2 recoilDir = direction.Rotated(recoilAngle);

        for (int i = 0; i < PelletCount; i++)
        {
            float spreadOffset = (GD.Randf() - 0.5f) * Mathf.DegToRad(BaseSpreadAngle);
            Vector2 pelletDir = recoilDir.Rotated(spreadOffset);

            var rayData = PerformRaycast(shooter, pelletDir);

            result.BulletEndPoints.Add(rayData.Position);

            if (rayData.HitEntity != null)
            {
                result.Hit = true;
                if (rayData.HitEntity is Enemy e) e.ApplyDamage(Damage);
            }
        }

        _currentSpread = Mathf.Min(_currentSpread + RecoilPerShotDeg, MaxSpreadDeg);

        if (performAutoReload) Reload(shooter);

        return result;
    }
}

public partial class KatanaWeapon : BaseWeapon
{
    public KatanaWeapon()
    {
        Name = "Katana";
        Damage = 100f;
        FireRate = 2.5f;
        MagazineSize = 0;
        Range = 90f;
        ScoreMultiplier = 2.0f;
        //Icon = GD.Load<Texture2D>("res://icons/katana.png");
    }
}

public partial class CrowbarWeapon : BaseWeapon
{
    public CrowbarWeapon()
    {
        Name = "Crowbar";
        Damage = 55f;
        FireRate = 5.0f;
        MagazineSize = 0;
        Range = 70f;
        ScoreMultiplier = 2.5f;
        //Icon = GD.Load<Texture2D>("res://icons/crowbar.png");
    }
}