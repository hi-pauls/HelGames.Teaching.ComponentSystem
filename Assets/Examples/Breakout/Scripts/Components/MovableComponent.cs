// -----------------------------------------------------------------------
// <copyright file="MovableComponent.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEngine;

    /// <summary>
    /// Defines the MovableComponent.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class MovableComponent : UnityComponent
    {
        /// <summary>
        /// The layer mask for collisions.
        /// </summary>
        [SerializeField]
        private LayerMask collisionMask;

        /// <summary>
        /// Gets or sets the layer mask for collisions.
        /// </summary>
        public LayerMask CollisionMask
        {
            get
            {
                return this.collisionMask;
            }

            set
            {
                this.collisionMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the rigidbody of the movable entity.
        /// </summary>
        public Rigidbody Rigidbody { get; set; }
    }
}