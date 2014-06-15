// -----------------------------------------------------------------------
// <copyright file="LevelPlayingState.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the LevelPlayingState.
    /// </summary>
    public class LevelPlayingState : LevelStateBase
    {
        /// <summary>
        /// Initializes a new instance of the LevelPlayingState class.
        /// </summary>
        /// <param name="system">
        /// The <see cref="LevelSystem"/> system, the state will be
        /// part of. This system is used to also derive the property
        /// <see cref="LevelStateBase.Game"/> from.
        /// </param>
        public LevelPlayingState(LevelSystem system) : base(system)
        {
        }

        /// <summary>
        /// Gets or sets the event to send, when the player wants to pause the game.
        /// The event is made configurable, for some independance from the actual
        /// state machine setup.
        /// </summary>
        public object PauseButtonEvent { get; set; }

        /// <summary>
        /// Gets or sets the event to send, when there are no balls in the game.
        /// </summary>
        public object NoBallsLeftEvent { get; set; }

        /// <summary>
        /// Gets or sets the event to send, when there are no blocks in the game.
        /// </summary>
        public object NoBlocksLeftEvent { get; set; }

        /// <summary>
        /// The state got entered and needs to be initialized or reset to perform
        /// its function properly.
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Entering " + this.GetType().Name);

            // Make sure, movement starts up when entering this state.
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.Continue, null));

            // Here is another usual suspect for states in component systems. The state registers
            // itself for specific events when entered and removes itself as a listener once the state
            // is left. It is a very compact way of handling state in systems and can be used, as the
            // state implementation is part of the system implementation.
            // This could also be done directly in the system, using a proxy event handler, which would
            // lead to a clearer view on the dependancies of the system. It would however also mean,
            // that there would have to be some awareness of the state built into the system. Then, the
            // system could send state machine events into the state machine, causing states to change.
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
        }

        /// <summary>
        /// The state will be left, tear it down.
        /// </summary>
        public override void OnExit()
        {
            // Always remove the state as a listener for each event, it registered for before.
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
        }

        /// <summary>
        /// Update the internal data of the state.
        /// </summary>
        public override void OnUpdate()
        {
            if (Input.GetButtonUp(this.LevelSystem.PlayPauseButtonName))
            {
                this.StateMachine.SendEvent(this.PauseButtonEvent);
            }
        }

        /// <summary>
        /// Handle ComponentDestroyed events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnComponentDestroyed(IEvent evt)
        {
            // This could also be done once per frame instead, in the OnUpdate method.
            // However, that would sacrifice performance while also moving away from
            // the event-driven approach for inter-connecting systems.
            if ((evt.EventData is BallComponent) && (this.Game.ComponentSystem.ComponentCount<BallComponent>() < 1))
            {
                this.StateMachine.SendEvent(this.NoBallsLeftEvent);
            }

            if ((evt.EventData is DestructableComponent) && this.Game.ComponentSystem.ComponentCount<DestructableComponent>() < 1)
            {
                this.StateMachine.SendEvent(this.NoBlocksLeftEvent);
            }
        }
    }
}