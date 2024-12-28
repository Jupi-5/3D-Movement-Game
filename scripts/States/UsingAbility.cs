using Godot;
using System;

public partial class UsingAbility : PlayerState
{

	private AnimationPlayer coachgunAP;
	public Vector3 direction;
	
	public override void Enter()
	{
		//coachgunAP = GetNode<AnimationPlayer>("Head/Camera3D/CoachGun/AnimationPlayer");
		//coachgunAP.Play("CoachGun"); //some fucking how this is a null reference okay i guess
		//genuinely dont know how this fuck ass program decides this but this is a null reference so i guess ill just go fuck myself
		_coachGunTimer();
	}

    public override void PhysicsUpdate(float delta)
    {
		velocity = player.Velocity;
		
		player.Velocity = velocity;
		direction = GetInputDirection();
		player.MoveAndSlide();
        if (direction != Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * player.walkSpeed, delta *2);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * player.walkSpeed, delta *2);
		}
		
    }

	public void _coachGunTimer()
	{
		GD.Print("starttimer");
		SceneTreeTimer CoachTimer = GetTree().CreateTimer(0.87);
		CoachTimer.Timeout += _coachGunLaunch;
	}

	public void _coachGunLaunch()
	{
		GD.Print("bang!");
		//this one is retired forever. fly high </3
		//velocity += new Vector3((-Head.Transform.Basis.Column0.Z *85) - Camera.Rotation.X * oppositeY *(-Head.Transform.Basis.Column0.Z *85 / 1.5707964f), -(Camera.Rotation.X *11.5f), (Head.Transform.Basis.Column2.Z *85) - Camera.Rotation.X * oppositeY *(Head.Transform.Basis.Column2.Z *85 / 1.5707964f));
		velocity = new Vector3(player.Head.Basis.Z.X *player.coachGunPower.X, player.Head.Basis.Z.Y *player.coachGunPower.Y, player.Head.Basis.Z.Z *player.coachGunPower.X);
		player.MoveAndSlide();
	}
}
