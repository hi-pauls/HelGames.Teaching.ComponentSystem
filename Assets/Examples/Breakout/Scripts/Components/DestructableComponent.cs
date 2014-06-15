// -----------------------------------------------------------------------
// <copyright file="DestructableComponent.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEngine;

    /// <summary>
    /// Defines the DestructableComponent.
    /// </summary>
    public class DestructableComponent : UnityComponent
    {
        /// <summary>
        /// The number of collisions, needed to destroy the entity.
        /// </summary>
        [SerializeField]
        private int health = 1;

        /// <summary>
        /// The points, this destructable is worth.
        /// </summary>
        [SerializeField]
        private int points = 10;

        /// <summary>
        /// Gets or sets the points, this destructable is worth.
        /// </summary>
        public int Points
        {
            get
            {
                return this.points;
            }

            set
            {
                this.points = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of collisions, needed to destroy the entity.
        /// </summary>
        public int Health
        {
            get
            {
                return this.health;
            }

            set
            {
                this.health = value;
            }
        }
    }
}