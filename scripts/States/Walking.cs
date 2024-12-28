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
