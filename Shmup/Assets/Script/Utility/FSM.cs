using System;
using System.Collections.Generic;
using System.Diagnostics;


// To make it a little easier to reference the state machine's context we make the FSM
// a generic class that is parameterized on the context type. This way states in the
// machine won't have to cast the context when accessing it.
public class FSM<TContext>
{
    private readonly TContext _context;
    private readonly Dictionary<Type, State> _stateCache = new Dictionary<Type, State>();
    public State CurrentState { get; private set; }

    private State _pendingState;
    public FSM(TContext context)
    {
        _context = context;
    }
    public void Update()
    {
        PerformPendingTransition();
        Debug.Assert(CurrentState != null, "Updating FSM with null current state. Did you forget to transition to a starting state?");
        CurrentState.Update();
        PerformPendingTransition();
    }

    public void TransitionTo<TState>() where TState : State
    {
        _pendingState = GetOrCreateState<TState>();
    }

    private void PerformPendingTransition()
    {
        if (_pendingState != null)
        {
            if (CurrentState != null) CurrentState.OnExit();
            CurrentState = _pendingState;
            CurrentState.OnEnter();
            _pendingState = null;
        }
    }
    private TState GetOrCreateState<TState>() where TState : State
    {
        State state;
        if (_stateCache.TryGetValue(typeof (TState), out state))
        {
            return (TState) state;
        }
        else
        {
            var newState = Activator.CreateInstance<TState>();
            newState.Parent = this;
            newState.Init();
            _stateCache[typeof(TState)] = newState;
            return newState;
        }
    }
    public void Clear()
    {
        foreach (var state in _stateCache.Values)
        {
            state.CleanUp();
        }
        _stateCache.Clear();
    }
    public abstract class State
    {
        internal FSM<TContext> Parent { get; set; }

        protected TContext Context { get { return Parent._context; } }

        protected void TransitionTo<TState>() where TState : State
        {
            Parent.TransitionTo<TState>();
        }

        public virtual void Init() { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update() { }

        public virtual void CleanUp() { }
    }

}
