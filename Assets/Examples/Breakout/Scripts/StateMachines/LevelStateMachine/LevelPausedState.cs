// -----------------------------------------------------------------------
// <copyright file="LevelPausedState.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the LevelPausedState.
    /// </summary>
    public class LevelPausedState : LevelStateBase
    {
        /// <summary>
        /// Initializes a new instance of the LevelPausedState class.
        /// </summary>
        /// <param name="system">
        /// The <see cref="LevelSystem"/> system, the state will be
        /// part of. This system is used to also derive the property
        /// <see cref="LevelStateBase.Game"/> from.
        /// </param>
        public LevelPausedState(LevelSystem system) : base(system)
        {
        }

        /// <summary>
        /// Gets or sets the event to send, when the player wants to continue the game.
        /// The event is made configurable, for some independance from the actual state
        /// machine setup.
        /// </summary>
        public object PlayButtonEvent { get; set; }

        /// <summary>
        /// The state got entered and needs to be initialized or reset to perform
        /// its function properly.
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Entering " + this.GetType().Name);

            // Make sure, the game is in the paused state. Queueing an event when entering states
            // to propagate specific properties of the state through the game is very common.
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.Pause, null));
        }

        /// <summary>
        /// Update the internal data of the state.
        /// </summary>
        public override void OnUpdate()
        {
            // The only task of this state is to wait for the player to start the ball.
            // For some added preasure, a timer could be added here and it could be
            // hooked into the GUI system to display the count-down.
            if (Input.GetButtonUp(this.LevelSystem.PlayPauseButtonName))
            {
                this.StateMachine.SendEvent(this.PlayButtonEvent);
            }
        }

        /// <summary>
        /// The state will be left, tear it down.
        /// </summary>
        public override void OnExit()
        {
            // No need to tear down anything, as nothing was set up.
        }
    }
}