// -----------------------------------------------------------------------
// <copyright file="IComponentSystem.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the IComponentSystem interface. Systems, implementing this interface,
    /// are responsible for fetching components, based on a given type. As systems that
    /// implement this interface are the data store of the game, usually, there should
    /// only be one such system in the game, so to not spread out the data and make
    /// access unnecessarily hard.
    /// <para>
    /// This includes getting all components of a specific type, getting a component of
    /// a specific type for a given ID of an entity and determining the total number of
    /// components of a given type, that are part of the game. Any type of component,
    /// that should be handled by the system, need to be registered with the system
    /// using <see cref="IComponentSystem.RegisterComponentType"/>. This should be done
    /// during initialization of each system, that requires a certain type of compoent.
    /// </para>
    /// </summary>
    public interface IComponentSystem
    {
        /// <summary>
        /// Register the given type of component. This will cause the component system
        /// to create the appropriate internal structure for managing all components
        /// of the specified type.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of components to register.
        /// </typeparam>
        void RegisterComponentType<TYPE>() where TYPE : IComponent;

        /// <summary>
        /// Get a unique ID for a new entity.
        /// <para>
        /// Please note, that as opposed to popular opinion, this ID is negative.
        /// The reason being, that during editing time, the developer will assign
        /// positive IDs to components and entities, so using negative IDs for new
        /// entities during runtime will just partition the available ID value range.
        /// This could just as easily be done in the positive range by dividing
        /// <see cref="Int32.MaxValue"/> by two, but it's not worth the effort and
        /// it helps distinguish automatically assigned IDs from manually assigned
        /// IDs at runtime, which can be important for debugging purposes.
        /// </para>
        /// </summary>
        /// <returns>
        /// The <see cref="int"/> unique ID for a new entity. The ID will be negative,
        /// as that is the partition of IDs to use for entities, created during runtime.
        /// </returns>
        int GetUniqueEntityId();

        /// <summary>
        /// Get the component with the given ID and the given type and return <c>true</c>
        /// upon success, setting the component for the given reference. <c>False</c> will
        /// be returned, if there is no component for the given type and ID, managed by
        /// the system. In that case, the component reference will contain an invalid value.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of component to get. This type should have been registered for management
        /// by this system using <see cref="IComponentSystem.RegisterComponentType"/>, or the
        /// return value will be <c>false</c> and the reference will be invalid.
        /// </typeparam>
        /// <param name="entityId">
        /// The <see cref="int"/> ID of the entity, to fetch the component for.
        /// </param>
        /// <param name="component">
        /// The typed reference to set the component for the given ID, if there is a
        /// component for the given ID. Otherwise, the reference will be set to an
        /// invalid value and may not be used.
        /// </param>
        /// <returns>
        /// <c>True</c>, if there is a component available for the given type and ID.
        /// The component reference will be set to the instance of the component with
        /// the given ID. Otherwise, <c>false</c> will be returned.
        /// </returns>
        bool TryGetComponent<TYPE>(int entityId, out TYPE component) where TYPE : IComponent, new();

        /// <summary>
        /// Get the number of components managed by this system for the given type.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of component to get the count for. If the given type was not registered
        /// using <see cref="IComponentSystem.RegisterComponentType"/>, the returned value
        /// will be 0.
        /// </typeparam>
        /// <returns>
        /// The <see cref="int"/> number of components of the given type, managed by this system.
        /// </returns>
        int ComponentCount<TYPE>();

        /// <summary>
        /// Get all components of the given type. If the type of the components was
        /// not registered using <see cref="IComponentSystem.RegisterComponentType"/>,
        /// an empty enumerator will be returned, so it is safe to use this method
        /// directly in a for-loop.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of components to get.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/> over all components of the given type, managed
        /// by this system.
        /// </returns>
        IEnumerable<TYPE> Components<TYPE>() where TYPE : IComponent, new();
    }
}