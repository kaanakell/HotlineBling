using Godot;

public partial class WinScreen : CanvasLayer
{
    [Export] public Label GradeLabel;
    [Export] public Label ScoreLabel;
    [Export] public Button RestartButton;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        if (RestartButton != null)
        {
            RestartButton.Pressed += () =>
            {
                GetTree().Paused = false;
                GetTree().ReloadCurrentScene();
                QueueFree();
            };
        }
    }

    public void Setup(int score, string grade)
    {
        if (GradeLabel != null) GradeLabel.Text = $"GRADE: {grade}";
        if (ScoreLabel != null) ScoreLabel.Text = $"SCORE: {score}";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("reload"))
        {
            GetTree().Paused = false;
            GetTree().ReloadCurrentScene();
            QueueFree();
        }
    }
}
