using System;

public class DefaultScoringRule : IScoringRule
{
    public int ComputeScore(int kills, float timeSeconds, float accuracy, float weaponMultiplier)
    {
        double timeScore = Math.Max(0, 20000 - timeSeconds * 50);
        double killScore = kills * 200;
        double accuracyBonus = Math.Round(accuracy * 50);
        double total = (timeScore + killScore + accuracyBonus) * weaponMultiplier;
        return (int)Math.Round(total);
    }
}

