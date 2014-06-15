// -----------------------------------------------------------------------
// <copyright file="UnityGame.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the UnityGame behaviour.
    /// </summary>
    public class UnityGame : MonoBehaviour, IGame
    {
        /// <summary>
        /// The component system to use.
        /// </summary>
        [SerializeField]
        private UnityComponentSystem componentSystem = null;

        /// <summary>
        /// The list of systems to execute, in the order of execution.
        /// </summary>
        [SerializeField]
        private UnitySystem[] systems;

        /// <summary>
        /// Gets the component system to use. The component system is the store
        /// of all components (meaning all the data) of the game and it can be
        /// used to access any data of any entity in the game.
        /// </summary>
        public IComponentSystem ComponentSystem
        {
            get
            {
                return this.componentSystem;
            }
        }

        /// <summary>
        /// Gets or sets the list of systems to execute, in the order of execution.
        /// </summary>
        public UnitySystem[] Systems
        {
            get
            {
                return this.systems;
            }

            set
            {
                this.systems = value;
            }
        }

        /// <summary>
        /// Gets or sets the event manager. It is used to pass around messages between systems.
        /// </summary>
        public EventManager EventManager { get; protected set; }

        /// <summary>
        /// Start the Game shortly before the first update
        /// </summary>
        public virtual void Start()
        {
            this.EventManager = new EventManager();
            foreach (UnitySystem system in this.Systems)
            {
                if (system != null)
                {
                    system.Initialize(this);

                    // Disable the component, so unity doesn't update it.
                    // We want to choose the point on which our systems
                    // are updated ourself, in this game.
                    system.enabled = false;
                }
            }
        }

        /// <summary>
        /// Update the Game, once a frame at the most.
        /// </summary>
        public virtual void Update()
        {
            this.EventManager.ProcessEvents();
            foreach (UnitySystem system in this.Systems)
            {
                if (system != null)
                {
                    system.Update();
                }
            }
        }

        /// <summary>
        /// Make sure all systems quit properly.
        /// </summary>
        public void OnApplicationQuit()
        {
            foreach (UnitySystem system in this.Systems)
            {
                if (system != null)
                {
                    system.Destroy();
                    GameObject.Destroy(system);
                }
            }

            GameObject.Destroy(this);
        }
    }
}