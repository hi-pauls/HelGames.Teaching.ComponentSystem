// -----------------------------------------------------------------------
// <copyright file="ComponentSystemEvents.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    /// <summary>
    /// Defines the ComponentSystemEvents.
    /// </summary>
    public enum ComponentSystemEvents
    {
        /// <summary>
        /// The event to queue, whenever a component is created. This event should be queued
        /// by the game logic when a specific component is created for an entity.
        /// </summary>
        ComponentCreated,

        /// <summary>
        /// The event to queue, whenever a component is destroyed. This event should be queued
        /// by the game logic when a specific component is destroyed for an entity.
        /// </summary>
        ComponentDestroyed,

        /// <summary>
        /// The event to queue, whenever an entity is destroyed. This event should be queued
        /// by the game logic, whenever an entity is destroyed. This will be used by all
        /// component managers to check, whether one of their tracked components is
        /// destroyed and to queue ComponentDestroyed events for those components.
        /// </summary>
        EntityDestroyed,

        /// <summary>
        /// The event to queue, whenever all component managers should update their component
        /// lists. This event may result in more than one ComponentCreated event being queued
        /// for the same component, if a new component manager, handling that type of component
        /// is added after this event is processed and this event being queued to initialize
        /// its list of components.
        /// </summary>
        UpdateComponentManagers
    }
}