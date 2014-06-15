// -----------------------------------------------------------------------
// <copyright file="IComponent.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    /// <summary>
    /// Defines the IComponent.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Gets or sets the ID of the entity, the component belongs to.
        /// </summary>
        int EntityId { get; set; }
    }
}