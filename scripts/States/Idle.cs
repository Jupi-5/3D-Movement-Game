using Godot;
using System;

public partial class Idle : PlayerState
{
    public override void Enter()
    {
        player.AP.Play("Idle");
        player.ApplyFloorSnap(); //prevents bug that stores jump velocity after crouch is released.
    }

    public override void PhysicsUpdate(float delta)
    {
        if (!player.IsOnFloor() && player.velocity.Y <= 0)
        {
            fsm.TransitionTo("Falling");
        }
    }
    public override void HandleInput(InputEvent @event)
    {
        if (player.IsOnFloor())
        {
            if (Input.IsActionJustPressed("jump"))
            {
                fsm.TransitionTo("Jumping");
            }

            if (Input.IsActionJustPressed("crouch"))
            {
                fsm.TransitionTo("Crouching");
            }

            if (GetInputDirection() != Vector3.Zero)
            {
                fsm.TransitionTo("Walking");
            }
        }
        
    }

    //public void Exit() {}
}
