// -----------------------------------------------------------------------
// <copyright file="LevelEvents.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    /// <summary>
    /// Defines the LevelEvents.
    /// </summary>
    public enum LevelEvents
    {
        /// <summary>
        /// The event to send when there are no balls left.
        /// </summary>
        NoBallsLeft,

        /// <summary>
        /// The event to send when there are no blocks left.
        /// </summary>
        NoBlocksLeft,

        /// <summary>
        /// The event to send, when there are no lives left.
        /// </summary>
        NoLivesLeft,

        /// <summary>
        /// The event to send when the game should be paused.
        /// </summary>
        Pause,

        /// <summary>
        /// The event to send when a state finished its work.
        /// </summary>
        Finished
    }
}