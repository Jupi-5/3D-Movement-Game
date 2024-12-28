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
			if (direction != Vector3.Zero)
			{
				fsm.TransitionTo("Walking");
			}
			else if (Input.IsActionPressed("crouch"))
			{
				fsm.TransitionTo("Crouching");
			}
			else
			{
				fsm.TransitionTo("Idle");
			}
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
