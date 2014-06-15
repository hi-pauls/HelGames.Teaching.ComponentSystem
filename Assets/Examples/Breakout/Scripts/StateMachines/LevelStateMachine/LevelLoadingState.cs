// -----------------------------------------------------------------------
// <copyright file="LevelLoadingState.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the LevelLoadingState.
    /// </summary>
    public class LevelLoadingState : LevelStateBase
    {
        /// <summary>
        /// Initializes a new instance of the LevelLoadingState class.
        /// </summary>
        /// <param name="system">
        /// The <see cref="LevelSystem"/> system, the state will be
        /// part of. This system is used to also derive the property
        /// <see cref="LevelStateBase.Game"/> from.
        /// </param>
        public LevelLoadingState(LevelSystem system) : base(system)
        {
        }

        /// <summary>
        /// Gets or sets the event to fire when level loading finishes.
        /// </summary>
        public object LevelLoadedEvent { get; set; }

        /// <summary>
        /// The state got entered and needs to be initialized or reset to perform
        /// its function properly.
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Entering " + this.GetType().Name);

            this.Game.EventManager.RegisterListener(BreakoutEvents.LevelLoaded, this.OnLevelLoaded);
        }

        /// <summary>
        /// The state will be left, tear it down.
        /// </summary>
        public override void OnExit()
        {
            this.Game.EventManager.RemoveListener(BreakoutEvents.LevelLoaded, this.OnLevelLoaded);
        }

        /// <summary>
        /// Update the internal data of the state.
        /// </summary>
        public override void OnUpdate()
        {
        }

        /// <summary>
        /// Handle LevelLoaded events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnLevelLoaded(IEvent evt)
        {
            this.StateMachine.SendEvent(this.LevelLoadedEvent);
        }
    }
}