using Godot;

public partial class FinalObjective : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (!body.IsInGroup("player")) return;

        var enemies = GetTree().GetNodesInGroup("enemies");

        if (enemies.Count > 0)
        {
            GD.Print($"Access Denied! {enemies.Count} targets remaining.");
            return;
        }

        // 3. Trigger Win
        if (body is PlayerController player)
        {
            player.Win();
        }
    }
}