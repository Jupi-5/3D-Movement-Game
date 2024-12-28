using Godot;
using System;

public partial class Crouching : PlayerState
{



    public override void Enter()
    {
        player.AP.Play("CrouchGrounded");
    }

    public override void HandleInput(InputEvent @event)
    {
        
        if (Input.IsActionJustReleased("crouch"))
		{
            //bug where if crouch is released and jump is input shortly thereafter a jump is stored until the next input *FIXED*
            //bug where upon releasing crouch you enter some unintended half crouch state where you cannot jump until doing some other input
            //if jump is input during this pseudo state you are immediately snapped to the floor and the charged jump is stored until the next directional input
            //if another jump is input rather than a directional press then the charged jump is overridden by a normal jump *FIXED*
            //was caused by player not *actually* being snapped to floor upon releasing crouch until next processing step with a "moveandslide" call
            //which does not happen in Idle.
            //so it would hold the end of the crouch until any other state was entered and processed.
			fsm.TransitionTo("Idle");
            
		}

        if (Input.IsActionJustPressed("jump"))
        {
            //direct transition from crouch to jump causes bug where gravity is not applied *FIXED*
            fsm.TransitionTo("Jumping");
        }
    }

    public override void Update(float delta)
    {
        if (!player.Charging)
        {
            player._chargeJump();
        }
    }

    public override void PhysicsUpdate(float delta)
    {
        velocity = MoveInDirection(player.walkSpeed * 0.55f);
        player.Velocity = velocity;
        player.MoveAndSlide();

        if (!player.IsOnFloor())
        {
            fsm.TransitionTo("Falling");
        }

        

    }

    public override void Exit()
    {

    }
}
