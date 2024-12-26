using Godot;
using System;

public partial class Falling : PlayerState
{

	public Vector3 velocity;

	public override void Enter()
	{
		//play a falling animation
		GD.Print("falling");
	}

    public override void PhysicsUpdate(float delta)
    {
		Vector3 direction = GetInputDirection();

		velocity = player.Velocity;
		velocity.Y += player._getRealGravity() * delta;
		GD.Print("Fall");
		if (player.IsOnFloor())
		{
			fsm.TransitionTo("Idle");
		}
		else if (direction != Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * player.walkSpeed, delta *2);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * player.walkSpeed, delta *2);
		}	
		player.Velocity = velocity;
		player.MoveAndSlide();
    }

	public override void Exit() {}
}
