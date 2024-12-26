using Godot;
using System;

public partial class Falling : PlayerState
{

	public Vector3 velocity;

	public void Enter()
	{
		//play a falling animation
		GD.Print("falling");
	}

    public override void PhysicsUpdate(float delta)
    {
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 Direction = player.collision.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		
		velocity = player.Velocity;
		velocity.Y += player._getRealGravity() * delta;
		GD.Print("Falling");
		if (player.IsOnFloor())
		{
			fsm.TransitionTo("Idle");
		}
		else if (Direction != Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(velocity.X, Direction.X * player.walkSpeed, delta *2);
			velocity.Z = Mathf.Lerp(velocity.Z, Direction.Z * player.walkSpeed, delta *2);
		}

		player.Velocity = velocity;
		player.MoveAndSlide();
    }

	public override void Exit() {}
}
