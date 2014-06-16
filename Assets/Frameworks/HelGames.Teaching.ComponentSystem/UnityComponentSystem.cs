// -----------------------------------------------------------------------
// <copyright file="UnityComponentSystem.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Defines the UnityComponentSystem interface. It is responsible for fetching
    /// components, based on a given type. As this system is the data store of the
    /// Unity game, usually, there should only be one instance of this system in
    /// the game, so to not spread out the data and make access unnecessarily hard.
    /// <para>
    /// To make this system aware of a component type, other systems need to register
    /// the types of components, they are interested in. This is done by calling the
    /// <see cref="UnityComponentSystem.RegisterComponentType"/> method with the
    /// appropriate type. The system will create a new component manager of type
    /// <see cref="UnityComponentManager"/> for each registered type. The event
    /// <see cref="ComponentSystemEvents.UpdateComponentManagers"/> as soon as all
    /// all relevant component types are registered, to prompt all the component
    /// managers to update their internal collections of components.
    /// </para>
    /// <para>
    /// The implementation of this system imposes some design and implementation
    /// restrictions in terms of how components may be organized and accessed. As
    /// it uses the <see cref="UnityComponentManager"/> implementation as a store
    /// for all components of a given type, there can be at maximum one component
    /// of a type per entity. If an entity would need multiple components of the
    /// same type, the logic for managing that component would need to account
    /// for that and as such, the component can just as easily contain a list of
    /// data sets, that would otherwise be contained in the components.
    /// </para>
    /// <para>
    /// Also, the restriction of one component of each type also applies to sub-
    /// classed components. If there is a component type A and component type B
    /// and C are sub-classes of that type, then an entity can have a component
    /// of type B and a component of type C, as long as those component types are
    /// registered as B and C. Registering type A as a component type automatically
    /// restricts any entity to only having either a component of type B or a
    /// component of type C. The reason for this is, that the component manager
    /// <see cref="UnityComponentManager"/> will store components in a dictionary,
    /// mapped by the ID of the entity, the component belongs to. In a component
    /// manager, managing components of type A, a component of type B and a component
    /// of type C, belonging to the same entity, would have the same entity ID.
    /// This will result in an exception, as the entity ID key will already exist
    /// in the dictionary, when the second component is added.
    /// </para>
    /// </summary>
    public class UnityComponentSystem : UnitySystem, IComponentSystem
    {
        /// <summary>
        /// The last queued unique ID for an entity.
        /// </summary>
        private int lastUniqueId = 0;

        /// <summary>
        /// Hosts the Dictionary of component managers, keyed by the type of
        /// component, they are handling.
        /// </summary>
        private Dictionary<Type, IComponentManager> componentManagers = new Dictionary<Type, IComponentManager>();

        /// <summary>
        /// Register the given type of component. This will cause the component system
        /// to create the appropriate internal structure for managing all components
        /// of the specified type.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of components to register.
        /// </typeparam>
        public void RegisterComponentType<TYPE>() where TYPE : IComponent
        {
            IComponentManager manager;
            if (this.componentManagers.TryGetValue(typeof(TYPE), out manager))
            {
                return;
            }

            IComponentManager<TYPE> newManager = new UnityComponentManager<TYPE>();
            newManager.Initialize(this.Game);
            this.componentManagers.Add(typeof(TYPE), newManager);
        }

        /// <summary>
        /// Get a unique ID for a new entity.
        /// <para>
        /// Please note, that as opposed to popular opinion, this ID is negative.
        /// The reason being, that during editing time, the developer will assign
        /// positive IDs to components and entities, so using negative IDs for new
        /// entities during runtime will just partition the available ID value range.
        /// This could just as easily be done in the positive range by dividing
        /// <see cref="int.MaxValue"/> by two, but it's not worth the effort and
        /// it helps distinguish automatically assigned IDs from manually assigned
        /// IDs at runtime, which can be important for debugging purposes.
        /// </para>
        /// </summary>
        /// <returns>
        /// The <see cref="int"/> unique ID for a new entity. The ID will be negative,
        /// as that is the partition of IDs to use for entities, created during runtime.
        /// </returns>
        public int GetUniqueEntityId()
        {
            this.lastUniqueId--;
            return this.lastUniqueId;
        }

        /// <summary>
        /// Get the component with the given ID and the given type and return <c>true</c>
        /// upon success, setting the component for the given reference. <c>False</c> will
        /// be returned, if there is no component for the given type and ID, managed by
        /// the system. In that case, the component reference will contain an invalid value.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of component to get. This type should have been registered for management
        /// by this system using <see cref="UnityComponentSystem.RegisterComponentType"/>, or
        /// the return value will be <c>false</c> and the reference will be invalid.
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
        public bool TryGetComponent<TYPE>(int entityId, out TYPE component) where TYPE : IComponent, new()
        {
            IComponentManager manager;
            if (! this.componentManagers.TryGetValue(typeof(TYPE), out manager))
            {
                component = default(TYPE);
                return false;
            }

            IComponentManager<TYPE> typedManager = (IComponentManager<TYPE>)manager;
            return typedManager.TryGetComponent(entityId, out component);
        }

        /// <summary>
        /// Get the number of components managed by this system for the given type.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of component to get the count for. If the given type was not registered
        /// using <see cref="UnityComponentSystem.RegisterComponentType"/>, the returned value
        /// will be 0.
        /// </typeparam>
        /// <returns>
        /// The <see cref="int"/> number of components of the given type, managed by this system.
        /// </returns>
        public int ComponentCount<TYPE>()
        {
            IComponentManager manager;
            if (! this.componentManagers.TryGetValue(typeof(TYPE), out manager))
            {
                return 0;
            }

            return ((IComponentManager<TYPE>)manager).Count;
        }

        /// <summary>
        /// Get all components of the given type. If the type of the components was
        /// not registered using <see cref="UnityComponentSystem.RegisterComponentType"/>,
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
        public IEnumerable<TYPE> Components<TYPE>() where TYPE : IComponent, new()
        {
            IComponentManager manager;
            if (! this.componentManagers.TryGetValue(typeof(TYPE), out manager))
            {
                // Generate a warning and return an empty enumerator,
                // if the component manager isn't registered yet.
                Debug.LogWarning("Component type " + typeof(TYPE).FullName + " isn't registered.");
                return Enumerable.Empty<TYPE>();
            }

            return ((IComponentManager<TYPE>)manager).GetComponents();
        }

        /// <summary>
        /// Destroy this system, freeing all its resources.
        /// </summary>
        public override void Destroy()
        {
            // Destroy the component manager
            foreach (KeyValuePair<Type, IComponentManager> managerPair in this.componentManagers)
            {
                managerPair.Value.Destroy();
            }
        }
    }
}