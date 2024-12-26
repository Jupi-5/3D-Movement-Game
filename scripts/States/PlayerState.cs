using Godot;
using System;

public abstract partial class PlayerState : State
{
	protected const string Idle = "Idle";
	protected const string Walking = "Walking";
	protected const string Running = "Running";
	protected const string Jumping = "Jumping";
	protected const string Crouching = "Crouching";
	protected const string Falling = "Falling";
	protected const string Sliding = "Sliding";
	protected const string UsingAbility = "UsingAbility";

	protected Player player;

    public override void _Ready()
    {
        player = Owner as Player;
		if (player == null)
		{
			GD.PushError("are you stupid? this shit only works if its owner is a player node moron");
		}
    }

	protected Vector3 GetInputDirection()
	{
   		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
    	return (player.collision.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
	}

	protected Vector3 MoveInDirection(float moveSpeed)
	{
		Vector3 newVelocity = player.Velocity;

        Vector3 direction = GetInputDirection();

        if (direction != Vector3.Zero)
        {
            newVelocity.X = direction.X * moveSpeed;
            newVelocity.Z = direction.Z * moveSpeed;
        }
        else
        {
            newVelocity.X += Mathf.Lerp(newVelocity.X, 0, 0.1f);
            newVelocity.Y += Mathf.Lerp(newVelocity.Y, 0, 0.1f);
        }

        return newVelocity;
	}
}
