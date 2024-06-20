using Godot;
using System;

public class PlayerStateMachine : Node
{
    [Signal] public delegate void Transitioned(NodePath newState);

    [Export] private NodePath initialState;
    private PlayerState currentState;

    public override void _Ready()
    {
        //yield(owner, "ready") in GDScript

        foreach (PlayerState s in GetChildren())
        {
            s.stateMachine = this;
        }
        currentState = GetNode<PlayerState>(initialState);
        currentState.Enter(Vector3.Zero);
    }

    // Since we are dealing with continuous key presses/holds to move/jump, here I will only use the Input singleton class instead.
    /**
    public override void _UnhandledInput(InputEvent inputEvent)
    {
        // InputEvent -> Input.IsActionPressed??
        // on types of Input-stuff, for example: https://www.reddit.com/r/godot/comments/dh0n6n/eli5_difference_between_input_inputevent/
        currentState.HandleInput(inputEvent);
    }
    /**/

    public override void _PhysicsProcess(float delta)
    {
        currentState.PhysicsUpdate(delta);
    }

    // # This function calls the current state's exit() function, then changes the active state,
    // # and calls its enter function.
    // # It optionally takes a `msg` dictionary to pass to the next state's enter() function.
    public void TransitionTo(NodePath targetState, Vector3 currentStateVelocity /*, object message*/ /* = {} Dictionary in GDScript */)
    {
        // "# Safety check, you could use an assert() here to report an error if the state name is incorrect." [etc]
        if (!HasNode(targetState)) return;

        currentState.Exit();
        currentState = GetNode<PlayerState>(targetState);
        currentState.Enter(currentStateVelocity);
        EmitSignal(nameof(Transitioned), currentState.Name);
    }
}
