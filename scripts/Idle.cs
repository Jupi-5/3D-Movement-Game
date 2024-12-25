using Godot;
using System;

public partial class Idle : State
{
	public AnimationPlayer AP;
    
    public override void Enter()
    {
        AP = GetNode<AnimationPlayer>("AnimationPlayer");
    }
}
