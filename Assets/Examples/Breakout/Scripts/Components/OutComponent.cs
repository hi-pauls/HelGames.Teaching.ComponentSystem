// -----------------------------------------------------------------------
// <copyright file="OutComponent.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEngine;

    /// <summary>
    /// Defines the OutComponent.
    /// </summary>
    public class OutComponent : UnityComponent
    {
        /// <summary>
        /// The ID of the player, this out is connected to.
        /// </summary>
        [SerializeField]
        private int playerId;

        /// <summary>
        /// Gets or sets the ID of the player, this out is connected to.
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
    }
}