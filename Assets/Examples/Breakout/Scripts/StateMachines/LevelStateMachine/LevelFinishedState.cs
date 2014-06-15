// -----------------------------------------------------------------------
// <copyright file="LevelFinishedState.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the LevelFinishedState.
    /// </summary>
    public class LevelFinishedState : LevelStateBase
    {
        /// <summary>
        /// Initializes a new instance of the LevelFinishedState class.
        /// </summary>
        /// <param name="system">
        /// The <see cref="LevelSystem"/> system, the state will be
        /// part of. This system is used to also derive the property
        /// <see cref="LevelStateBase.Game"/> from.
        /// </param>
        public LevelFinishedState(LevelSystem system) : base(system)
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
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LevelFinished, null));
        }

        /// <summary>
        /// Update the internal data of the state.
        /// </summary>
        public override void OnUpdate()
        {
            if (Input.GetButtonUp(this.LevelSystem.PlayPauseButtonName))
            {
                // Load the new level additive
                int nextLevelIndex = Application.loadedLevel + 1;
                if (nextLevelIndex >= Application.levelCount)
                {
                    nextLevelIndex = this.LevelSystem.StartLevelIndex;
                }

                // Now queue the component manager update and send the transition event.
                this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LoadLevel, nextLevelIndex));
                this.StateMachine.SendEvent(this.LevelLoadingEvent);
            }
        }

        /// <summary>
        /// The state will be left, tear it down.
        /// </summary>
        public override void OnExit()
        {
        }
    }
}