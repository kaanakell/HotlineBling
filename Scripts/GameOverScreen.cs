using Godot;

public partial class GameOverScreen : CanvasLayer
{
    [Export] public Button RestartButton;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        if (RestartButton != null)
        {
            RestartButton.Pressed += OnRestartPressed;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("restart")) OnRestartPressed();
    }

    private void OnRestartPressed()
    {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
        QueueFree();
    }
}