// -----------------------------------------------------------------------
// <copyright file="UnityComponentManager.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using System;
    using System.Collections.Generic;
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the UnityComponentManager. It will be
    /// </summary>
    /// <typeparam name="TYPE">
    /// The type of component, this manager is responsible for.
    /// </typeparam>
    public class UnityComponentManager<TYPE> : IComponentManager<TYPE> where TYPE : IComponent
    {
        /// <summary>
        /// Hosts the Dictionary of IDs of entities, mapped to the instance of
        /// a component of the type, the manager needs to handle.
        /// </summary>
        private Dictionary<int, TYPE> components = new Dictionary<int, TYPE>();

        /// <summary>
        /// Gets the game, the manager belongs to.
        /// </summary>
        public IGame Game { get; private set; }

        /// <summary>
        /// Gets the number of components, contained in this manager.
        /// </summary>
        public int Count
        {
            get
            {
                return this.components.Count;
            }
        }

        /// <summary>
        /// Initialize the component manager, allowing it to set up a valid internal state
        /// and to register for events using the <see cref="IGame.EventManager"/> event
        /// manager of the given <see cref="IGame"/>.
        /// </summary>
        /// <param name="game">
        /// The <see cref="IGame"/> game, the component manager will belong to.
        /// </param>
        public void Initialize(IGame game)
        {
            this.Game = game;
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.UpdateComponentManagers, this.OnUpdateComponentManagers);
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.EntityDestroyed, this.OnEntityDestroyed);
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentCreated, this.OnComponentCreated);
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
        }

        /// <summary>
        /// Destroy the component manager. Calling this method will cause the component manager
        /// to clear its list of components and to remove it self as an event listener from the
        /// <see cref="IGame.EventManager"/> of the <see cref="IGame"/>, it was running in.
        /// </summary>
        public void Destroy()
        {
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.UpdateComponentManagers, this.OnUpdateComponentManagers);
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.EntityDestroyed, this.OnEntityDestroyed);
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentCreated, this.OnComponentCreated);
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
            this.components.Clear();
            this.Game = null;
        }

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
        public bool TryGetComponent(int entityId, out TYPE component)
        {
            return this.components.TryGetValue(entityId, out component);
        }

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
        public IEnumerable<TYPE> GetComponents()
        {
            foreach (KeyValuePair<int, TYPE> componentPair in this.components)
            {
                yield return componentPair.Value;
            }
        }

        /// <summary>
        /// Handle ComponentCreated events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnComponentCreated(IEvent evt)
        {
            if (evt.EventData is TYPE)
            {
                TYPE component = (TYPE)evt.EventData;

                if (this.components.ContainsKey(component.EntityId))
                {
                    this.components[component.EntityId] = component;
                }
                else
                {
                    this.components.Add(component.EntityId, component);
                }
            }
        }

        /// <summary>
        /// Handle ComponentDestroyed events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnComponentDestroyed(IEvent evt)
        {
            if (evt.EventData is TYPE)
            {
                TYPE component = (TYPE)evt.EventData;
                this.components.Remove(component.EntityId);
            }
        }

        /// <summary>
        /// Handle UpdateComponentManagers events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnUpdateComponentManagers(IEvent evt)
        {
            // This event should probably only be used after all component managers are
            // fully initialized. The reason for this is, that this call will result in
            // ComponentCreated events. Any component manager, created after this event
            // is processed, will create ComponentCreated events again, for components,
            // that were already processed by component managers and more importantly,
            // may already have been initialized or even changed by systems. In those
            // cases, the components may be initialized again and may potentially loose
            // their state data.
            HashSet<int> encounteredIds = new HashSet<int>();

            // Get a list of currently active components of TYPE and add the new ones.
            IComponent untypedComponent;
            TYPE component;
            TYPE registeredComponent;
            int createdCount = 0;
            int updatedCount = 0;
            foreach (UnityEngine.Object obj in GameObject.FindObjectsOfType(typeof(TYPE)))
            {
                untypedComponent = (IComponent)obj;
                component = (TYPE)untypedComponent;
                encounteredIds.Add(component.EntityId);

                if (this.components.TryGetValue(component.EntityId, out registeredComponent))
                {
                    if (object.ReferenceEquals(component, registeredComponent))
                    {
                        continue;
                    }

                    // This is an updated component, destroy the old one put in the new one.
                    this.Game.EventManager.FireEvent(
                                                new HelGamesEvent(
                                                        ComponentSystemEvents.ComponentDestroyed,
                                                        registeredComponent));
                    updatedCount++;
                }

                // This is a new one, fire component created. This will only be done by the
                // first component manager to encounter the new component, any component
                // managers for superclasses or subclasses will be informed along with the
                // manager itself, causing those component managers to not even reach this
                // call, because they will already contain the component. However, as noted
                // above, this is only valid for component managers, that are already fully
                // initialized.
                this.Game.EventManager.FireEvent(
                                            new HelGamesEvent(
                                                    ComponentSystemEvents.ComponentCreated,
                                                    component));
                createdCount++;
            }

            // Remove components, that aren't active any more.
            List<TYPE> destroyedComponents = new List<TYPE>();
            foreach (KeyValuePair<int, TYPE> componentPair in this.components)
            {
                if (encounteredIds.Contains(componentPair.Key))
                {
                    continue;
                }

                // Just add destroyed components to the list here, instead of
                // firing the ComponentDestroyed event. This is necessary, as
                // we are iterating over the components dictionary here and
                // firing the event would result in it being modified.
                destroyedComponents.Add(componentPair.Value);
            }

            foreach (TYPE destroyedComponent in destroyedComponents)
            {
                // This is an old one, fire component destroyed. This will only be done by the
                // first component manager to encounter the new component, any component
                // managers for superclasses or subclasses will be informed along with the
                // manager itself, causing those component managers to not even reach this
                // call, because they will no longer contain the component.
                this.Game.EventManager.FireEvent(
                                            new HelGamesEvent(
                                                    ComponentSystemEvents.ComponentDestroyed,
                                                    destroyedComponent));
            }

            Debug.Log(
                    "Updated " + this.GetType().GetGenericArguments()[0].Name +
                    " created " + createdCount +
                    " destroyed " + (destroyedComponents.Count + updatedCount));
        }

        /// <summary>
        /// Handle EntityDestroyed events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnEntityDestroyed(IEvent evt)
        {
            int entityId = (int)evt.EventData;

            TYPE component;
            if (! this.components.TryGetValue(entityId, out component))
            {
                return;
            }

            this.Game.EventManager.FireEvent(
                                        new HelGamesEvent(
                                                ComponentSystemEvents.ComponentDestroyed,
                                                component));
        }
    }
}