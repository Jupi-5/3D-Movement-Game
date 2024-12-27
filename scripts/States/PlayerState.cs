using Godot;
using System;
using System.Threading.Tasks;

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

	public AnimationPlayer AP;

	public Vector3 velocity;

	public int jumpCharge = 0;

    public override void _Ready()
    {
		AP = GetNode<AnimationPlayer>("Player/AnimationPlayer");
		GD.Print(AP);
		fsm = GetParent<StateMachine>();
        player = Owner as Player;
		if (player == null)
		{
			GD.PushError("are you stupid? this shit only works if its owner is a player node moron");
		}
    }

	protected Vector3 GetInputDirection()
	{
   		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
    	return (player.collision.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
	}

	protected Vector3 MoveInDirection(float moveSpeed, double delta)
	{
		Vector3 newVelocity = player.Velocity;

        Vector3 Direction = GetInputDirection();

        if (Direction != Vector3.Zero)
        {
            newVelocity.X = Direction.X * moveSpeed;
            newVelocity.Z = Direction.Z * moveSpeed;
        }
        else
        {
            newVelocity.X = 0;
           	newVelocity.Z = 0;
        }
		//Mathf.Lerp(newVelocity.X, Direction.X * moveSpeed, (float)delta * 2)
		//Mathf.Lerp(newVelocity.Z, Direction.Z * moveSpeed, (float)delta * 2)


        return newVelocity;
	}
}
