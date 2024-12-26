using Godot;
using System;

public partial class Idle : PlayerState
{
    public override void Enter()
    {
        GD.Print("Idle");
        player.AP.Play("Idle");
    }

    public override void PhysicsUpdate(float delta)
    {

        if (!player.IsOnFloor())
        // && player.velocity.Y <= 0
        {
            GD.Print("fall");
            fsm.TransitionTo("Falling");
        }
    }
    public override void HandleInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("jump"))
        {
            fsm.TransitionTo("Jumping");
        }
        
        if (Input.IsActionJustPressed("crouch") && player.IsOnFloor())
        {
            fsm.TransitionTo("Crouching");
        }

        if (GetInputDirection() != Vector3.Zero && player.IsOnFloor())
        {
            fsm.TransitionTo("Walking");
        }
    }

    public void Exit() {}
}
