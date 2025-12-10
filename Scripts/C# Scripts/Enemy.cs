using Godot;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D, IEnemy
{
	public enum AIBehavior { Stationary, Patrol }
	public enum AttackMethod { Melee, Ranged }

	[ExportCategory("AI Settings")]
	[Export] public AIBehavior Behavior = AIBehavior.Stationary;
	[Export] public AttackMethod AttackType = AttackMethod.Melee;

	[Export] public Godot.Collections.Array<Node2D> PatrolPoints;

	[Export] public float MaxHealth { get; set; } = 100f;
	[Export] public float MoveSpeed = 100f;
	[Export] public float VisionRange = 350f;
	[Export] public float FieldOfView = 90f;

	[ExportCategory("Combat")]
	[Export] public PackedScene BulletPrefab;
	[Export] public float AttackCooldown = 1.0f;
	[Export] public float MeleeRange = 50f;

	[ExportCategory("Visuals")]
	[Export] public SpriteFrames SpecificSpriteFrames;
	private AnimatedSprite2D _sprite;
	[Export] public bool FaceLeftOnSpawn = false;

	private float _hp;
	private NavigationAgent2D _navAgent;
	private RayCast2D _visionCast;
	private Node2D _player;

	private double _attackTimer = 0.0;
	private double _pathTimer = 0.0;

	private enum State { Idle, Patrol, Chase, Attack }
	private State _currentState = State.Idle;
	private int _patrolIndex = 0;

	[Signal] public delegate void EnemyDiedEventHandler(Enemy enemy);

	public override void _Ready()
	{
		_hp = MaxHealth;
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_visionCast = GetNode<RayCast2D>("VisionCast");

		_player = GetTree().GetFirstNodeInGroup("player") as Node2D;

		if (Behavior == AIBehavior.Patrol && PatrolPoints != null && PatrolPoints.Count > 0)
		{
			_currentState = State.Patrol;
			SetNextPatrolTarget();
		}
		else
		{
			_currentState = State.Idle;
		}

		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (SpecificSpriteFrames != null && _sprite != null)
		{
			_sprite.SpriteFrames = SpecificSpriteFrames;
			_sprite.Play("default");
		}

		if (_sprite != null && FaceLeftOnSpawn)
		{
			_sprite.FlipH = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_attackTimer > 0) _attackTimer -= delta;

		if (_player != null && CanSeePlayer())
		{
			_currentState = State.Chase;
		}

		switch (_currentState)
		{
			case State.Idle:
				Velocity = Vector2.Zero;
				break;

			case State.Patrol:
				ProcessPatrol((float)delta);
				break;

			case State.Chase:
				ProcessChase((float)delta);
				break;

			case State.Attack:
				Velocity = Vector2.Zero;
				break;
		}

		MoveAndSlide();
	}

	private void ProcessPatrol(float delta)
	{
		if (_navAgent.IsNavigationFinished())
		{
			_patrolIndex = (_patrolIndex + 1) % PatrolPoints.Count;
			SetNextPatrolTarget();
			return;
		}

		MoveToTarget(MoveSpeed * 0.5f, delta);
	}

	private void ProcessChase(float delta)
	{
		if (_player == null) return;

		float dist = GlobalPosition.DistanceTo(_player.GlobalPosition);
		float effectiveRange = (AttackType == AttackMethod.Melee) ? MeleeRange : VisionRange;

		if (dist <= effectiveRange && CanSeePlayer())
		{
			if (_attackTimer <= 0)
			{
				StartAttack();
			}
			else
			{
				LookAtTarget(_player.GlobalPosition);
				Velocity = Vector2.Zero;
			}
		}
		else
		{
			_pathTimer -= delta;
			if (_pathTimer <= 0)
			{
				_navAgent.TargetPosition = _player.GlobalPosition;
				_pathTimer = 0.1f;
			}
			MoveToTarget(MoveSpeed * 1.2f, delta);
		}
	}

	private void MoveToTarget(float speed, float delta)
	{
		if (_navAgent.IsNavigationFinished())
		{
			Velocity = Vector2.Zero;
			return;
		}

		Vector2 nextPathPos = _navAgent.GetNextPathPosition();
		Vector2 direction = (nextPathPos - GlobalPosition).Normalized();

		if (direction.X < 0) _sprite.FlipH = true;
		else if (direction.X > 0) _sprite.FlipH = false;

		Velocity = direction * speed;
	}

	private void LookAtTarget(Vector2 target)
	{
		if (target.X < GlobalPosition.X)
			_sprite.FlipH = true;
		else
			_sprite.FlipH = false;
	}

	private void StartAttack()
	{
		_currentState = State.Attack;
		_attackTimer = AttackCooldown;

		GD.Print($"{Name}: Attacking!");

		if (AttackType == AttackMethod.Ranged)
		{
			LookAtTarget(_player.GlobalPosition);
			SpawnBullet();
		}
		else
		{
			if (GlobalPosition.DistanceTo(_player.GlobalPosition) <= MeleeRange + 10)
			{
				if (_player is PlayerController pc) pc.Die();
			}
		}

		GetTree().CreateTimer(0.5f).Timeout += () =>
		{
			if (_hp > 0) _currentState = State.Chase;
		};
	}

	private void SpawnBullet()
	{
		if (BulletPrefab == null)
		{
			GD.PrintErr("Enemy has no BulletPrefab assigned!");
			return;
		}

		var bullet = BulletPrefab.Instantiate<EnemyBullet>();
		GetTree().Root.AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition;

		Vector2 dir = (_player.GlobalPosition - GlobalPosition).Normalized();
		bullet.Setup(dir);
	}

	private void SetNextPatrolTarget()
	{
		if (PatrolPoints == null || PatrolPoints.Count == 0) return;
		_navAgent.TargetPosition = PatrolPoints[_patrolIndex].GlobalPosition;
	}

	private bool CanSeePlayer()
	{
		if (_player == null) return false;

		Vector2 toPlayer = _player.GlobalPosition - GlobalPosition;
		if (toPlayer.Length() > VisionRange) return false;

		Vector2 facingDir = _sprite.FlipH ? Vector2.Left : Vector2.Right;

		float angle = Mathf.RadToDeg(facingDir.AngleTo(toPlayer));
		if (Mathf.Abs(angle) > FieldOfView / 2f) return false;

		_visionCast.TargetPosition = ToLocal(_player.GlobalPosition);
		_visionCast.ForceRaycastUpdate();

		if (_visionCast.IsColliding())
		{
			var hit = _visionCast.GetCollider() as Node2D;
			return hit == _player || hit.IsInGroup("player");
		}
		return false;
	}

	public void ApplyDamage(float damage)
	{
		_hp -= damage;
		_currentState = State.Chase;

		if (_hp <= 0)
		{
			EmitSignal(SignalName.EnemyDied, this);
			QueueFree();
		}
	}
}