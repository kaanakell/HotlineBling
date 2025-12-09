using Godot;
using System;

public partial class DefaultScoringRule : Resource, IScoringRule
{
	public int ComputeScore(int kills, float timeSeconds, float accuracy, float weaponMultiplier)
	{
		int killScore = kills * 1000;

		int timeScore = Math.Max(0, 10000 - (int)(timeSeconds * 50));

		int accuracyScore = (int)(accuracy * 5000);

		float total = (killScore + timeScore + accuracyScore) * weaponMultiplier;

		return (int)total;
	}

	public static string GetGrade(int score)
	{
		if (score >= 30000) return "S";
		if (score >= 25000) return "A";
		if (score >= 20000) return "B";
		if (score >= 10000) return "C";
		if (score >= 5000) return "D";
		return "E";
	}
}
