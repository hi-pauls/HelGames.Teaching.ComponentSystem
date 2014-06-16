// -----------------------------------------------------------------------
// <copyright file="PaddleComponent.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEngine;

    /// <summary>
    /// Defines the PaddleComponent.
    /// </summary>
    [RequireComponent(typeof(MovableComponent))]
    public class PaddleComponent : UnityComponent
    {
        /// <summary>
        /// The ID of the player, the paddle belongs to.
        /// </summary>
        [SerializeField]
        private int playerId;

        /// <summary>
        /// The name of the input axis, this game object uses to move.
        /// </summary>
        [SerializeField]
        private string inputAxis;

        /// <summary>
        /// The vector of movement to apply.
        /// </summary>
        [SerializeField]
        private Vector3 movementVector;

        /// <summary>
        /// Gets or sets the ID of the player, the paddle belongs to.
        /// </summary>
        public int PlayerId
        {
            get
            {
                return this.playerId;
            }

            set
            {
                this.playerId = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the input axis, this game object uses to move.
        /// </summary>
        public string InputAxis
        {
            get
            {
                return this.inputAxis;
            }

            set
            {
                this.inputAxis = value;
            }
        }

        /// <summary>
        /// Gets or sets the vector of movement to apply.
        /// </summary>
        public Vector3 MovementVector
        {
            get
            {
                return this.movementVector;
            }

            set
            {
                this.movementVector = value;
            }
        }
    }
}