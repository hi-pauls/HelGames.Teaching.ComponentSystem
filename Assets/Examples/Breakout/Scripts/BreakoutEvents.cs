// -----------------------------------------------------------------------
// <copyright file="BreakoutEvents.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    /// <summary>
    /// Defines the BreakoutEvents.
    /// </summary>
    public enum BreakoutEvents
    {
        /// <summary>
        /// The event, queued as soon as the game started.
        /// </summary>
        GameStarted,

        /// <summary>
        /// Move the entity to a certain position.
        /// </summary>
        MoveEntityTo,

        /// <summary>
        /// Spawn a ball for the given player.
        /// </summary>
        SpawnBallsForPlayer,

        /// <summary>
        /// The event, queued whenever a ball is lost by a player.
        /// </summary>
        BallLostByPlayer,

        /// <summary>
        /// Pause all movement.
        /// </summary>
        Pause,

        /// <summary>
        /// Continue all movement.
        /// </summary>
        Continue,

        /// <summary>
        /// The event queued, when the player fails a level.
        /// </summary>
        LevelFinished,

        /// <summary>
        /// The event queued, when the player fails a level.
        /// </summary>
        LevelFailed,

        /// <summary>
        /// Load a new level.
        /// </summary>
        LoadLevel,

        /// <summary>
        /// The event, queued when a level is loaded.
        /// </summary>
        LevelLoaded
    }
}