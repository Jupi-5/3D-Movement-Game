using Godot;
using System;

public partial class Crouching : PlayerState
{
    public override void Enter()
    {
        
    }

    public override void HandleInput(InputEvent @event)
    {
        if (Input.IsActionJustReleased("crouch"))
		{
			fsm.TransitionTo("Idle");
		}
    }
}
