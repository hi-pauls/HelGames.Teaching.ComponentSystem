// -----------------------------------------------------------------------
// <copyright file="LevelStateBase.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using HelGames.Teaching.StateMachine;

    /// <summary>
    /// Defines the LevelStateBase. This is the base class for all
    /// states, contained in the level system state machine.
    /// </summary>
    public abstract class LevelStateBase : StateBase
    {
        /// <summary>
        /// Initializes a new instance of the LevelStateBase class.
        /// </summary>
        /// <param name="system">
        /// The <see cref="LevelSystem"/> system, the state will be
        /// part of. This system is used to also derive the property
        /// <see cref="LevelStateBase.Game"/> from.
        /// </param>
        public LevelStateBase(LevelSystem system) : base()
        {
            this.LevelSystem = system;
            this.Game = system.Game;
        }

        /// <summary>
        /// Gets the game, the state is part of.
        /// </summary>
        public IGame Game { get; private set; }

        /// <summary>
        /// Gets the LevelSystem, the state belongs to.
        /// </summary>
        public LevelSystem LevelSystem { get; private set; }
    }
}