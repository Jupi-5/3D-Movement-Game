using Godot;
using System;

public partial class Walking : PlayerState
{
	//public void Enter() {}

    public override void HandleInput(InputEvent @event)
    {

		if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
		{
			fsm.TransitionTo("Jumping");
		}
		else if (Input.IsActionJustPressed("sprint"))
		{
			//if (inputDir.Y <= 0)
			//{
			//	fsm.TransitionTo("Sprinting");
			//}
		}
		else if (Input.IsActionJustPressed("crouch"))
		{
			//1. if crouch is pressed while moving and direction never equals zero then the crouching state is entered but 
			//Walking.PhysicsUpdate() is still run in place of that from the crouching state
			//no idea why this happens or how to fix it
			//2. if crouch is pressed as you step off of a platform and enter the falling state at the same time you 
			//are supposed to enter the crouching state a similar bug occurs
			//3. if crouch is pressed repeatedly and rapidly while jumping as you walk off a ledge you are immediately snapped to floor
			//and locked in a pseudo crouching state
			fsm.TransitionTo("Crouching");
		}
    }

    public override void PhysicsUpdate(float delta)
    {
		player.Velocity = MoveInDirection(player.walkSpeed);
		
		if (GetInputDirection() == Vector3.Zero)
		{
			fsm.TransitionTo("Idle");
		}
		if (!player.IsOnFloor())
		{
			fsm.TransitionTo("Falling");
		}
		player.MoveAndSlide();
    }
}
