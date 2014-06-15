// -----------------------------------------------------------------------
// <copyright file="LevelSpawnState.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the LevelSpawnState.
    /// </summary>
    public class LevelSpawnState : LevelStateBase
    {
        /// <summary>
        /// Initializes a new instance of the LevelSpawnState class.
        /// </summary>
        /// <param name="system">
        /// The <see cref="LevelSystem"/> system, the state will be
        /// part of. This system is used to also derive the property
        /// <see cref="LevelStateBase.Game"/> from.
        /// </param>
        public LevelSpawnState(LevelSystem system) : base(system)
        {
        }

        /// <summary>
        /// Gets or sets the event to use when there are balls left. This event is
        /// made configurable, because that way, it becomes a bit more independant.
        /// </summary>
        public object BallsLeftEvent { get; set; }

        /// <summary>
        /// Gets or sets the event to use when there are no balls left. This event
        /// is also configurable, to allow for a more flexible setup.
        /// </summary>
        public object GameOverEvent { get; set; }

        /// <summary>
        /// The state got entered and needs to be initialized or reset to perform
        /// its function properly.
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Entering " + this.GetType().Name);
            int ballCount = 0;

            // Since we could have more than one player, spawn a ball for all players. The component
            // system does not offer a convenient way for only fetching the first player without
            // knowing his ID. This is by design! Either make the ID configurable (for example in
            // the LevelSystem), or do it for all players, which actually isn't more difficult to
            // code but much more extensible.
            foreach (PlayerComponent player in this.Game.ComponentSystem.Components<PlayerComponent>())
            {
                if (player.Lives > 0)
                {
                    // In a component system, state machines will usually wait for an event or fetch
                    // component information and potentially queue a new event, as is done here.
                    this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.SpawnBallsForPlayer, player));
                    ballCount++;
                }
            }

            // Send the appropriate event for changing the state.
            if (ballCount > 0)
            {
                this.StateMachine.SendEvent(this.BallsLeftEvent);
            }
            else
            {
                this.StateMachine.SendEvent(this.GameOverEvent);
            }
        }

        /// <summary>
        /// Update the internal data of the state.
        /// </summary>
        public override void OnUpdate()
        {
            // This method will be empty, as this state does all its transitions
            // in the OnEnter() method and does not need to run continuous checks.
        }

        /// <summary>
        /// The state will be left, tear it down.
        /// </summary>
        public override void OnExit()
        {
            // There is nothing to tear down here, so the implementation can be empty.
        }
    }
}