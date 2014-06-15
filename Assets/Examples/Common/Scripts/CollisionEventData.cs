// -----------------------------------------------------------------------
// <copyright file="CollisionEventData.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Common
{
    /// <summary>
    /// Defines the CollisionEventData. This class is used to encapsulate the relevant
    /// data for collisions, reported by Unity and sent as an event into a game.
    /// </summary>
    public class CollisionEventData
    {
        /// <summary>
        /// Initializes a new instance of the CollisionEventData class.
        /// </summary>
        /// <param name="colliderId">
        /// The <see cref="int"/> ID of the entity, reporting the collision.
        /// </param>
        /// <param name="collideeId">
        /// The <see cref="int"/> ID of the entity, the collider entity is colliding with.
        /// </param>
        /// <param name="context">
        /// The <see cref="object"/> additional information about the collision, like
        /// <see cref="UnityEngine.Collision"/> collision information or the
        /// <see cref="UnityEngine.Collider"/> collider object of the collidee.
        /// </param>
        public CollisionEventData(int colliderId, int collideeId, object context)
        {
            this.ColliderId = colliderId;
            this.CollideeId = collideeId;
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the ID of the collider entity.
        /// </summary>
        public int ColliderId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the other entity, the entity is colliding with.
        /// </summary>
        public int CollideeId { get; set; }

        /// <summary>
        /// Gets or sets the context of the collision, which may either be the other
        /// collider in case of a trigger or the collision info in case of a collision.
        /// </summary>
        public object Context { get; set; }
    }
}