// -----------------------------------------------------------------------
// <copyright file="MoveEntityToEventData.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using UnityEngine;

    /// <summary>
    /// Defines the MoveEntityToEventData. This type of event data is used to
    /// send movement information for entities through the game to all systems,
    /// that are registered for the <see cref="BreakoutEvents.MoveEntityTo"/>
    /// event.
    /// </summary>
    public class MoveEntityToEventData
    {
        /// <summary>
        /// Initializes a new instance of the MoveEntityToEventData class.
        /// </summary>
        /// <param name="entityId">
        /// The <see cref="int"/> ID of the entity to move to the specified position.
        /// </param>
        /// <param name="position">
        /// The <see cref="Vector3"/> position to move the entity to.
        /// </param>
        public MoveEntityToEventData(int entityId, Vector3 position)
        {
            this.EntityId = entityId;
            this.Position = position;
            this.IgnorePaused = false;
        }

        /// <summary>
        /// Gets or sets the ID of the entity to move.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the position in world space coordinates to move
        /// the entity to.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the movement
        /// should be applied in paused mode as well.
        /// </summary>
        public bool IgnorePaused { get; set; }
    }
}