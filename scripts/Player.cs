using Godot;
using System;


public partial class Player : CharacterBody3D
{
	//Player attributes
	public float Speed = 5.0f;
	public float walkSpeed = 9.0f;
	public float sprintScalar = 1.63f;
	public float mouseSensitivity = 0.0006f;
	public float maxSpeed = 5.0f;

	//Jump attributes
	public float jumpVelocity;
	public float jumpGravity;
	public float fallGravity;
	public float jumpHeight = 10f;
	public float jumpTimeToPeak = 2.75f;
	public float jumpTimeToDescent = 3.0f;

	//Shortnamed nodes for readability and ease of editing
	public Node3D Head;
	public Camera3D Camera;
	public bool Charging = false;
	public TextureProgressBar chargeBar;
	
	
	//Enum for finite state machine
	public enum States 
	{

		Idle,

		Walking,

		Crouching, 

		Jumping,

		Sprinting
		
	};

	//Variable for assignment of States from enum set
	public States state;
	public AnimationPlayer AP;

	//Int to increment when jump is charged then access to alter jump height depending on it
	public float jumpCharge;
	public float oppositeY;
	public Vector3 velocity;
	public float _rotationX = 0f;
	public float _rotationY = 0f;
	public Vector2 coachGunPower;
	public Vector3 newVelocity = Vector3.Zero;



    public override void _Ready()
    {
        Head = GetNode<Node3D>("Head");
		AP = GetNode<AnimationPlayer>("AnimationPlayer");
		Camera = GetNode<Camera3D>("Head/Camera3D");
		coachGunPower = new Vector2(45f, 15f);
		chargeBar = GetNode<TextureProgressBar>("TextureProgressBar");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		state = States.Idle;
		jumpVelocity = 2.0f * jumpHeight / jumpTimeToPeak;
		jumpGravity = -2.0f * jumpHeight / (jumpTimeToPeak * jumpTimeToPeak);
		fallGravity = -2.0f * jumpHeight / (jumpTimeToDescent * jumpTimeToDescent);
		Mathf.Clamp(_rotationX, Mathf.DegToRad(-90f), Mathf.DegToRad(90f));
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
		}
    }
    public override void _Input(InputEvent @event)
    {
		
    }


    public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;
		Mathf.Clamp(velocity.X, -maxSpeed, maxSpeed);
		Mathf.Clamp(velocity.Z, -maxSpeed, maxSpeed);
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = Head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y);
		
		velocity.Y += _getRealGravity() * (float)delta;
		switch (state)
		{
			case States.Idle:
			{
				AP.Play("Idle");
				break;
			}

			case States.Crouching:
			{
				AP.Play("CrouchGrounded");
				if (Charging == false && IsOnFloor())
				{
					_chargeJump();
					GD.Print(jumpCharge);
				}
				if (Input.IsActionJustReleased("Crouch"))
				{
					Charging = false;
					break;
				}
				break;
			}

			case States.Jumping:
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
				AP.Play("Idle");
				break;
			}

			case States.Sprinting:
			{
				if (Speed == walkSpeed)
				{
					Speed *= sprintScalar;
				}
				break;
			}

			case States.Walking:
			{
				Speed = walkSpeed;
				break;
			}
		}
		if (direction != Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(0, direction.X * Speed, 1f);
			velocity.Z = Mathf.Lerp(0, direction.Z * Speed, 1f);
		}
		if (direction == Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(Velocity.X, 0, 0.1f);
			velocity.Z = Mathf.Lerp(Velocity.Z, 0, 0.1f);
			if (state != States.Crouching)
			{
				state = States.Idle;
			}
			
		}


		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (IsOnFloor() && state == States.Jumping)
		{
			state = States.Idle;
		}

		if (Input.IsActionPressed("crouch") && IsOnFloor() && state == States.Idle)
		{
			state = States.Crouching;
		}

		if (Input.IsActionJustReleased("crouch"))
		{
			state = States.Idle;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			state = States.Jumping;
		}
		if (Input.IsActionJustPressed("esc"))
		{
			GetTree().Quit();
		}
		if (Input.IsActionPressed("sprint"))
		{
			state = States.Sprinting;
		}
		if (Input.IsActionJustReleased("sprint"))
		{
			state = States.Walking;
		}
		if (jumpCharge > 10)
		{
			chargeBar.Visible = true;
			chargeBar.Value = jumpCharge;
		}
		if (Input.IsActionJustPressed("coachgun"))
		{
			GetNode<AnimationPlayer>("Head/Camera3D/CoachGun/AnimationPlayer").Play("CoachGun");
			//velocity += new Vector3(Head.Basis.Z.X *coachGunPower.X, Head.Basis.Z.Y *coachGunPower.Y, Head.Basis.Z.Z *coachGunPower.X);
			_coachGunTimer();
			
		}
		_YpositiveNegative(Camera.Rotation.X);
		if (newVelocity != velocity && newVelocity != Vector3.Zero)
		{
			velocity = newVelocity;
			newVelocity = Vector3.Zero;
		}
		Velocity = velocity;
		MoveAndSlide();
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
	public void _YpositiveNegative(float InputValue)
	{
		if (InputValue < 0)
		{
			oppositeY = -1;
		}
		else
		{
			oppositeY = 1;
		}
	}
}
