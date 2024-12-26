using Godot;
using System;
using System.Threading.Tasks;


public partial class Player : CharacterBody3D
{
	//Player attributes
	public float Speed = 5.0f;
	public float walkSpeed = 9.0f;
	public float sprintScalar = 1.63f;
	public float mouseSensitivity = 0.0006f;
	public float maxSpeed = 10.0f;

	//Jump attributes
	public float jumpVelocity;
	public float jumpGravity;
	public float fallGravity;
	public float jumpHeight = 10f;
	public float jumpTimeToPeak = 2.75f;
	public float jumpTimeToDescent = 2.0f;

	//Shortnamed nodes for readability and ease of editing
	public Node3D Head;
	public Camera3D Camera;
	public CollisionShape3D playerCollision;
	public TextureProgressBar chargeBar;
	public CollisionShape3D collision;
	public AnimationPlayer AP;
	

	//Int to increment when jump is charged then access to alter jump height depending on it
	public float jumpCharge;
	public float oppositeY;
	public Vector3 velocity;
	public float _rotationX = 0f;
	public float _rotationY = 0f;
	public Vector2 coachGunPower;
	public Vector3 newVelocity = Vector3.Zero;
	public bool Charging = false;



    public override void _Ready()
    {
		collision = GetNode<CollisionShape3D>("CollisionShape3D");
        Head = GetNode<Node3D>("Head");
		AP = GetNode<AnimationPlayer>("AnimationPlayer");
		Camera = GetNode<Camera3D>("Head/Camera3D");
		coachGunPower = new Vector2(45f, 15f);
		chargeBar = GetNode<TextureProgressBar>("TextureProgressBar");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Mathf.Clamp(_rotationX, Mathf.DegToRad(-90f), Mathf.DegToRad(90f));
		jumpGravity = -2.0f * jumpHeight / (jumpTimeToPeak * jumpTimeToPeak);
		fallGravity = -2.0f * jumpHeight / (jumpTimeToDescent * jumpTimeToDescent);
		
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
			//code via https://docs.godotengine.org/en/4.0/tutorials/3d/using_transforms.html
			//modify the rotation based on mouse movement
			_rotationX -= mouseMotion.Relative.X * mouseSensitivity;
			_rotationY -= mouseMotion.Relative.Y * mouseSensitivity;

			_rotationY = _rotationY > Mathf.Pi / 2 ? Mathf.Pi / 2 : _rotationY;
			_rotationY = _rotationY < -Mathf.Pi / 2 ? -Mathf.Pi / 2 : _rotationY;

			//reset the rotation of head basis
			//x, y, z = [1,0,0] [0,1,0] [0,0,1]
			Transform3D headTransform = Head.Transform;
			headTransform.Basis = Basis.Identity;
			Head.Transform = headTransform;

			Head.RotateObjectLocal(Vector3.Up, _rotationX);
			Head.RotateObjectLocal(Vector3.Right, _rotationY);

			Transform3D collisionTransform = collision.Transform;
			collisionTransform.Basis = Basis.Identity;
			collision.Transform = collisionTransform;

			collision.RotateObjectLocal(Vector3.Up, _rotationX);
		}
    }
    public override void _Input(InputEvent @event)
    {
		
    }


    public override void _PhysicsProcess(double delta)
	{
		
	//velocity = Velocity;
	//Mathf.Clamp(velocity.X, -maxSpeed, maxSpeed);
	//Mathf.Clamp(velocity.Z, -maxSpeed, maxSpeed);
    //Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
	//Vector3 direction = Head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y);

	//velocity.Y += _getRealGravity() * (float)delta;

	//if (!IsOnFloor())
	//{
	//	velocity += GetGravity() * (float)delta;
	//}

	//if (Input.IsActionJustPressed("esc"))
	//{
	//	GetTree().Quit();
	//}

	//if (jumpCharge > 10)
	//{
	//	chargeBar.Visible = true;
	//	chargeBar.Value = jumpCharge;
	//}
	//if (newVelocity != velocity && newVelocity != Vector3.Zero)
	//{
	//	velocity = newVelocity;
	//	newVelocity = Vector3.Zero;
	//}
	//Velocity = velocity;
	//MoveAndSlide();
	}

	public void _coachGunTimer()
	{	
		SceneTreeTimer CoachTimer = GetTree().CreateTimer(0.87);
		CoachTimer.Timeout += _coachGunLaunch;
	}

	public void _coachGunLaunch()
	{
		//this one is retired forever. fly high </3
		//velocity += new Vector3((-Head.Transform.Basis.Column0.Z *85) - Camera.Rotation.X * oppositeY *(-Head.Transform.Basis.Column0.Z *85 / 1.5707964f), -(Camera.Rotation.X *11.5f), (Head.Transform.Basis.Column2.Z *85) - Camera.Rotation.X * oppositeY *(Head.Transform.Basis.Column2.Z *85 / 1.5707964f));
		newVelocity = new Vector3(Head.Basis.Z.X *coachGunPower.X, Head.Basis.Z.Y *coachGunPower.Y, Head.Basis.Z.Z *coachGunPower.X);
	}
	public void _printTest()
	{
		GD.Print("working");
	}

    public void _chargeJump()
	{
		Charging = true;
		SceneTreeTimer ChargeTimer = GetTree().CreateTimer(0.035);
		ChargeTimer.Timeout += _jumpIncrememnt;
	}

	public void _jumpIncrememnt()
	{
		if (jumpCharge < 100)
		{
			jumpCharge += 5f;
			
		}
		Charging = false;
	}

	public void _jumpChargeReset()
	{
		jumpCharge = 0;
		chargeBar.Visible = false;
	}

	public float _getRealGravity()
	{
		if (Velocity.Y < 0)
		{
			return jumpGravity;
		}
		else
		{
			return fallGravity;
		}
	}

	public void Jump()
	{
		if (jumpCharge == 0)
		{
			velocity.Y = jumpVelocity;
		}
		else
		{
			velocity.Y = jumpVelocity + (jumpCharge / 10);
		}
		GD.Print(velocity.Y);
		_jumpChargeReset();
	}
}
