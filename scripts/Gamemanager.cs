using Godot;
using System;

public partial class Gamemanager : Node
{
	
	private static PackedScene player = GD.Load<PackedScene>("res://scenes/player.tscn");

	public Vector3 spawnLocation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnLocation.X = -65;
		spawnLocation.Y = 7;
		_spawnPlayer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("reset"))
		{
			GetChild<Player>(1).Position = spawnLocation;
		}
	}

	public void _spawnPlayer()
	{
		Player instance = player.Instantiate<Player>();
		instance.Position = spawnLocation;
		AddChild(instance);
	}
}
