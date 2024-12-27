using Godot;
using System;

public partial class Jumping : PlayerState
{
    public override void Enter()
    {
        velocity = player.Velocity;
        if (jumpCharge == 0)
        {
	        velocity.Y = player.jumpVelocity;
        }
        else
        {
	        velocity.Y = player.jumpVelocity + jumpCharge / 10;
        }
        player.Velocity = velocity;
        
    }

    public override void PhysicsUpdate(float delta)
    {
        velocity = player.Velocity;

		velocity.Y += player._getRealGravity() * delta;
		velocity += player.GetGravity() * delta;

        if (player.IsOnFloor())
        {
            fsm.TransitionTo("Idle");
        }
        velocity = MoveInDirection(player.walkSpeed, delta);
        player.Velocity = velocity;
        player.MoveAndSlide();
    }
}
