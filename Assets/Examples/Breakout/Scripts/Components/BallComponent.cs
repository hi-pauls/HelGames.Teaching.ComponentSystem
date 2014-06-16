// -----------------------------------------------------------------------
// <copyright file="BallComponent.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEngine;

    /// <summary>
    /// Defines the BallComponent.
    /// </summary>
    public class BallComponent : UnityComponent
    {
        /// <summary>
        /// The velocity of the ball.
        /// </summary>
        [SerializeField]
        private Vector3 defaultVelocity = Vector3.zero;

        /// <summary>
        /// The velocity of the ball.
        /// </summary>
        [SerializeField]
        private Vector3 velocity = Vector3.zero;

        /// <summary>
        /// Gets or sets the velocity of the ball.
        /// </summary>
        public Vector3 DefaultVelocity
        {
            get
            {
                return this.defaultVelocity;
            }

            set
            {
                this.defaultVelocity = value;
            }
        }

        /// <summary>
        /// Gets or sets the velocity of the ball.
        /// </summary>
        public Vector3 Velocity
        {
            get
            {
                return this.velocity;
            }

            set
            {
                this.velocity = value;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the player, who last touched the ball.
        /// </summary>
        public int PlayerId { get; set; }
    }
}