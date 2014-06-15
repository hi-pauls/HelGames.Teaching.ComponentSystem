// -----------------------------------------------------------------------
// <copyright file="IComponentManager.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the IComponentManager interface. This is the non-generic form of the
    /// interface, describing the methods, component managers need to implement for
    /// being managed by an <see cref="IComponentSystem"/> without considering the
    /// type of components or the components themselfes, the manager may handle.
    /// </summary>
    public interface IComponentManager
    {
        /// <summary>
        /// Initialize the component manager. The component manager will need to
        /// register for all events using <see cref="EventManager.RegisterListener"/>
        /// here, that impact the collection of typed components, it is managing.
        /// </summary>
        /// <param name="game">
        /// The <see cref="IGame"/> game, this component manager belongs to. This is
        /// necessary to allow the component manager access to game-specific "global"
        /// resources like the <see cref="IGame.EventManager"/>.
        /// </param>
        void Initialize(IGame game);

        /// <summary>
        /// Destroy the component manager. The component manager will have to remove
        /// any references to itself from other parts of the component system, that
        /// it set by itself. In particular, this means removing itself as a listener
        /// for all events, it is registered for. This is done using the method
        /// <see cref="EventManager.RemoveListener"/>. It also has to clear its
        /// collection of components.
        /// </summary>
        void Destroy();
    }

    /// <summary>
    /// Defines the generic IComponentManager interface. This interface specifies the
    /// methods, a component manager should provide for dealing with the typed instances
    /// of the components, it is managing.
    /// </summary>
    /// <typeparam name="TYPE">
    /// The base type of components, the component manager will handle. Any components of
    /// this type as well as component types, derived from this type, will be handled by
    /// the component manager.
    /// </typeparam>
    public interface IComponentManager<TYPE> : IComponentManager
    {
        /// <summary>
        /// Gets the number of components, managed by this manager.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Try to get the component for the given ID. If there is no component for
        /// the given ID, managed by this component manager, the component reference
        /// will be set to an invalid value and <c>false</c> will be returned. The
        /// component reference should then not be used under any circumstances. Should
        /// the component manager contain a component for the given ID, <c>true</c>
        /// is returned and the component reference will be a valid component.
        /// </summary>
        /// <param name="entityId">
        /// The <see cref="int"/> ID of the entity, to fetch the component for.
        /// </param>
        /// <param name="component">
        /// The typed reference to set the component for the given ID, if there is a
        /// component for the given ID. Otherwise, the reference will be set to an
        /// invalid value and may not be used.
        /// </param>
        /// <returns>
        /// <c>True</c>, if the component manager is managing a component for the given ID.
        /// The component reference will be set to the instance of the component with the
        /// given ID. Otherwise, <c>false</c> will be returned.
        /// </returns>
        bool TryGetComponent(int entityId, out TYPE component);

        /// <summary>
        /// Get a typed enumerable over all components, this manager is currently managing.
        /// This enumerator can be used for example in for-loops to perform specific actions
        /// on all the components of the type, the component manager is handling. However,
        /// special care must be taken to not modify the collection of components, that are
        /// being handled by the component manager, while the enumerable is used.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/> of typed components, managed by the component manager.
        /// </returns>
        IEnumerable<TYPE> GetComponents();
    }
}