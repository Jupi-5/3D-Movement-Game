using Godot;
using System;

public partial class Falling : PlayerState
{

	public override void Enter()
	{

	}

    public override void PhysicsUpdate(float delta)
    {
		Vector3 direction = GetInputDirection();

		velocity = player.Velocity;
		velocity.Y += player._getRealGravity() * delta;
		velocity += player.GetGravity() * delta;
		if (player.IsOnFloor())
		{
			fsm.TransitionTo("Idle");
		}
		else if (direction != Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * player.walkSpeed, delta *4);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * player.walkSpeed, delta *4);
		}	
		player.Velocity = velocity;
		player.MoveAndSlide();
    }

	public override void Exit() {}
}
