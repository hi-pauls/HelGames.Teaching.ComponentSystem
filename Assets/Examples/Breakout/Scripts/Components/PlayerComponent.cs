// -----------------------------------------------------------------------
// <copyright file="PlayerComponent.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEngine;

    /// <summary>
    /// Defines the PlayerComponent.
    /// </summary>
    public class PlayerComponent : UnityComponent
    {
        /// <summary>
        /// The name of the player.
        /// </summary>
        [SerializeField]
        private string playerName = "John";

        /// <summary>
        /// The score, the player has.
        /// </summary>
        [SerializeField]
        private int score = 0;

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string PlayerName
        {
            get
            {
                return this.playerName;
            }

            set
            {
                this.playerName = value;
            }
        }

        /// <summary>
        /// Gets or sets the score, the player has.
        /// </summary>
        public int Score
        {
            get
            {
                return this.score;
            }

            set
            {
                this.score = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of lives, the player has left.
        /// </summary>
        public int Lives { get; set; }
    }
}