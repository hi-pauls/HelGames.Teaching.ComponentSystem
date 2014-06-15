// -----------------------------------------------------------------------
// <copyright file="LevelStates.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    /// <summary>
    /// Defines the LevelStates.
    /// </summary>
    public enum LevelStates
    {
        /// <summary>
        /// The loading state.
        /// </summary>
        Loading,

        /// <summary>
        /// The (re)spawn state.
        /// </summary>
        Spawn,

        /// <summary>
        /// The paused state.
        /// </summary>
        Paused,

        /// <summary>
        /// The playing state.
        /// </summary>
        Playing,

        /// <summary>
        /// The level finished state.
        /// </summary>
        LevelFinished,

        /// <summary>
        /// The level failed state.
        /// </summary>
        LevelFailed
    }
}