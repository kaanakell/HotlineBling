using Godot;
using System;

public interface IScoringRule
{
	int ComputeScore(int kills, float timeSeconds, float accuracy, float weaponMultiplier);
}


	

	
