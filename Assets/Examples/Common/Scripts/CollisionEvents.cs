// -----------------------------------------------------------------------
// <copyright file="CollisionEvents.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Common
{
    /// <summary>
    /// Defines the CollisionEvents.
    /// </summary>
    public enum CollisionEvents
    {
        /// <summary>
        /// Fired whenever a collision was entered.
        /// </summary>
        CollisionEntered,

        /// <summary>
        /// Fired whenever a collision was left.
        /// </summary>
        CollisionLeft,

        /// <summary>
        /// Fired whenever a trigger was entered.
        /// </summary>
        TriggerEntered,

        /// <summary>
        /// Fired whenever a trigger was left.
        /// </summary>
        TriggerLeft
    }
}