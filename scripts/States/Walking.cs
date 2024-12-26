using Godot;
using System;

public partial class Walking : PlayerState
{
	public void Enter() {}

    public override void HandleInput(InputEvent @event)
    {
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");

		if (Input.IsActionJustPressed("jump"))
		{
			fsm.TransitionTo("Jumping");
		}
		else if (Input.IsActionJustPressed("sprint"))
		{
			if (inputDir.Y <= 0)
			{
				fsm.TransitionTo("Running");
			}
		}
		else if (Input.IsActionJustPressed("crouch"))
		{
			
		}
    }
}
