// -----------------------------------------------------------------------
// <copyright file="LevelFailedState.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the LevelFailedState.
    /// </summary>
    public class LevelFailedState : LevelStateBase
    {
        /// <summary>
        /// Initializes a new instance of the LevelFailedState class.
        /// </summary>
        /// <param name="system">
        /// The <see cref="LevelSystem"/> system, the state will be
        /// part of. This system is used to also derive the property
        /// <see cref="LevelStateBase.Game"/> from.
        /// </param>
        public LevelFailedState(LevelSystem system) : base(system)
        {
        }

        /// <summary>
        /// Gets or sets the event to fire when level loading finishes.
        /// </summary>
        public object LevelLoadingEvent { get; set; }

        /// <summary>
        /// The state got entered and needs to be initialized or reset to perform
        /// its function properly.
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Entering " + this.GetType().Name);

            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.Pause, null));
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LevelFailed, null));
        }

        /// <summary>
        /// The state will be left, tear it down.
        /// </summary>
        public override void OnExit()
        {
        }

        /// <summary>
        /// Update the internal data of the state.
        /// </summary>
        public override void OnUpdate()
        {
            if (Input.GetButtonUp(this.LevelSystem.PlayPauseButtonName))
            {
                this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LoadLevel, this.LevelSystem.StartLevelIndex));
                this.StateMachine.SendEvent(this.LevelLoadingEvent);
            }
        }
    }
}