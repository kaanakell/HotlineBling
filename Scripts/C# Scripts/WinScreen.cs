using Godot;

public partial class WinScreen : CanvasLayer
{
    [Export] public Label GradeLabel;
    [Export] public Label ScoreLabel;
    [Export] public Label TitleLabel;
    [Export] public Label GradeText;
    [Export] public Button RestartButton;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        GetTree().Root.GetNode("MusicManager").Call("play_win_music");

        if (RestartButton != null)
        {
            RestartButton.Pressed += OnRestartPressed;
        }

        if (TitleLabel != null)
        {
            PulseText(TitleLabel, Colors.Yellow, Colors.OrangeRed);
        }
        if (GradeText != null)
        {
            PulseText(GradeText, Colors.Yellow, Colors.OrangeRed);
        }
    }

    public void Setup(int score, string grade)
    {
        if (GradeLabel != null)
        {
            GradeLabel.Text = grade;
            PulseText(GradeLabel, Colors.WebPurple, Colors.HotPink);
        }

        if (ScoreLabel != null)
        {
            ScoreLabel.Text = score.ToString();
            PulseText(ScoreLabel, Colors.HotPink, Colors.WebPurple);
        }
    }

    private void PulseText(Label label, Color colorA, Color colorB)
    {
        if (label == null) return;

        Tween tween = CreateTween().SetLoops();
        tween.TweenProperty(label, "modulate", colorB, 0.5f)
             .SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(label, "modulate", colorA, 0.5f);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("restart"))
        {
            OnRestartPressed();
        }
    }

    private void OnRestartPressed()
    {
        GetTree().Paused = false;

        GetTree().CallDeferred("reload_current_scene");

        QueueFree();
    }
}