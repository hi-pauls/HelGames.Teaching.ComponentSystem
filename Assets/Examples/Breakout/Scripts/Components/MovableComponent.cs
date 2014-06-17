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
        /// The value indicating whether the movable is moving.
        /// </summary>
        [SerializeField]
        private bool isMoving = false;

        /// <summary>
        /// The desired location of the movable.
        /// </summary>
        [SerializeField]
        private Vector3 desiredPosition;

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
        /// Gets or sets the value indicating whether the movable is moving.
        /// </summary>
        public bool IsMoving
        {
            get
            {
                return this.isMoving;
            }

            set
            {
                this.isMoving = value;
            }
        }

        /// <summary>
        /// Gets or sets the desired location of the movable.
        /// </summary>
        public Vector3 DesiredPosition
        {
            get
            {
                return this.desiredPosition;
            }

            set
            {
                this.desiredPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the rigidbody of the movable entity.
        /// </summary>
        public Rigidbody Rigidbody { get; set; }
    }
}