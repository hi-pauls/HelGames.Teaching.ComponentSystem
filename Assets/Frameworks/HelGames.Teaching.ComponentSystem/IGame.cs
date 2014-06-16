// -----------------------------------------------------------------------
// <copyright file="IGame.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using HelGames.Teaching.EventManager;

    /// <summary>
    /// Defines the Game.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets the <see cref="IComponentSystem"/> component system to use
        /// for getting any component, currently available in the game.
        /// </summary>
        IComponentSystem ComponentSystem { get; }

        /// <summary>
        /// Gets the <see cref="EventManager"/> event manager to use in the
        /// game for passing around event messages.
        /// </summary>
        EventManager EventManager { get; }
    }
}