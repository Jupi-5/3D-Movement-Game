using Godot;
using System;

public partial class Jumping : PlayerState
{

    public Vector3 velocity;
    


    public override void Enter()
    {
        GD.Print("Jump");
    }

    public override void PhysicsUpdate(float delta)
    {
        velocity = player.Velocity;

        player.Jump();
        
		GD.Print(velocity.Y);
		player._jumpChargeReset();

        if (player.IsOnFloor())
        {
            fsm.TransitionTo("Idle");
        }
        MoveInDirection(player.walkSpeed);
        player.Velocity = velocity;
        player.MoveAndSlide();
    }
}
