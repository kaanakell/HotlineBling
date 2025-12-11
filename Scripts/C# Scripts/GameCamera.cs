using Godot;

public partial class GameCamera : Camera2D
{
    [ExportCategory("Shake Settings")]
    [Export] public float ShakeDecay = 5.0f;
    [Export] public Vector2 MaxOffset = new Vector2(10, 10);

    private float _shakeStrength = 0.0f;
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _rng.Randomize();
    }

    public override void _Process(double delta)
    {
        if (_shakeStrength > 0)
        {
            _shakeStrength = Mathf.Lerp(_shakeStrength, 0, ShakeDecay * (float)delta);

            float xOffset = _rng.RandfRange(-_shakeStrength, _shakeStrength) * MaxOffset.X;
            float yOffset = _rng.RandfRange(-_shakeStrength, _shakeStrength) * MaxOffset.Y;

            Offset = new Vector2(xOffset, yOffset);
        }
        else
        {
            Offset = Vector2.Zero;
        }
    }

    public void AddShake(float amount)
    {
        _shakeStrength = Mathf.Min(_shakeStrength + amount, 1.0f);
    }
}
