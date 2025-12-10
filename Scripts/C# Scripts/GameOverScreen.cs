using Godot;

public partial class GameOverScreen : CanvasLayer
{
    [Export] public Button RestartButton;
    [Export] public Label TitleLabel;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        GetTree().Root.GetNode("MusicManager").Call("play_lose_music");

        if (RestartButton != null)
        {
            RestartButton.Pressed += OnRestartPressed;
        }

        if (TitleLabel != null)
        {
            PulseText(TitleLabel, Colors.Red, new Color(0.5f, 0, 0, 1));
        }
    }

    private void PulseText(Label label, Color colorA, Color colorB)
    {
        Tween tween = CreateTween().SetLoops();
        tween.TweenProperty(label, "modulate", colorB, 0.8f)
             .SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(label, "modulate", colorA, 0.8f);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("restart")) OnRestartPressed();
    }

    private void OnRestartPressed()
    {
        GetTree().Paused = false;
        GetTree().CallDeferred("reload_current_scene");
        QueueFree();
    }
}