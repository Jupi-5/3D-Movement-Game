using Godot;
using System;

public partial class Jumping : PlayerState
{
    public override void Enter()
    {
        player.AP.Play("Jumping");
        velocity = player.Velocity;
        if (player.jumpCharge == 0)
        {
	        velocity.Y = player.jumpVelocity;
        }
        else
        {
	        velocity.Y = player.jumpVelocity + player.jumpCharge / 10;
        }
        player._jumpChargeReset();
        player.Velocity = velocity;
    }

    public override void PhysicsUpdate(float delta)
    {
        velocity = player.Velocity;

        velocity = MoveInDirection(player.walkSpeed);

		velocity.Y += player._getRealGravity() * delta;
		velocity += player.GetGravity() * delta;

        player.Velocity = velocity;
        player.MoveAndSlide();


        if (player.IsOnFloor())
        {
            fsm.TransitionTo("Idle");
        }

        if (!player.IsOnFloor())
        {
            if (velocity.Y < 0)
            {
                fsm.TransitionTo("Falling");
            }
        }

        
        
    }
}
