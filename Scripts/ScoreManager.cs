using Godot;
using System;

public partial class ScoreManager : Node
{
	private IScoringRule _rule = new DefaultScoringRule();
	private int _kills;
	private int _shotsFired;
	private int _shotsHit;
	private DateTime _start;

	public override void _Ready() => _start = DateTime.UtcNow;

	public void OnShotFired(bool hit)
	{
		_shotsFired++;
		if (hit) _shotsHit++;
	}

	public void OnEnemyKilled() => _kills++;

	public int FinalScore(IWeapon weapon)
	{
		double time = (DateTime.UtcNow - _start).TotalSeconds;
		float accuracy = _shotsFired == 0 ? 0 : (float)_shotsHit / _shotsFired;
		return _rule.ComputeScore(_kills, (float)time, accuracy, weapon.ScoreMultiplier);
	}
}
