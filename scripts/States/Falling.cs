using Godot;
using System;

public partial class Falling : PlayerState
{

	public override void Enter()
	{
		player.AP.Play("Falling");
	}

    public override void PhysicsUpdate(float delta)
    {
		Vector3 direction = GetInputDirection();

		velocity = player.Velocity;
		velocity.Y += player._getRealGravity() * delta;
		velocity += player.GetGravity() * delta;
		player.MoveAndSlide();
		if (player.IsOnFloor())
		{
			if (Input.IsActionPressed("crouch"))
			{
				fsm.TransitionTo("Crouching");
			}
			else if (direction != Vector3.Zero)
			{
				//fixed bug caused by checking for direction before crouch so if walking and crouching at the same time would always result in walking
				fsm.TransitionTo("Walking");
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
		
    }

	public override void Exit() {}
}
