using System.Collections.Generic;
using Godot;
using System;

public partial class StateMachine : Node
{

	[Export] public NodePath initialState;

	private Dictionary<string, State> _states;
	private State _currentState;
	private State _prevState;


	public override void _Ready()
	{
		_states = new Dictionary<string, State>();
		foreach (Node node in GetChildren())
		{
			if (node is State s)
			{
				_states[node.Name] = s;
				s.fsm = this;
				s.Ready();
				s.Exit(); //resets all states
			}
		}

		_currentState = GetNode<State>(initialState); 
		_currentState.Enter();
	}

	public override void _Process(double delta)
	{
		_currentState.Update((float) delta);
	}

    public override void _PhysicsProcess(double delta)
    {
        _currentState.PhysicsUpdate((float) delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _currentState.HandleInput(@event);
    }

	public void TransitionTo(string key)
	{
		if (!_states.ContainsKey(key) || _currentState == _states[key])
		{
			GD.Print(Owner.Name + ": Trying to transition to state " + _currentState + " but it does not exist.");
			return;
		}
		
		
		_prevState = _currentState;
		_currentState.Exit();
		_currentState = _states[key];
		_currentState.Enter();
	}

}
