// -----------------------------------------------------------------------
// <copyright file="CollisionEventDispatcher.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Common
{
    using HelGames.Teaching.Common;
    using HelGames.Teaching.ComponentSystem;
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the CollisionEventDispatcher.
    /// </summary>
    [RequireComponent(typeof(EntityComponent))]
    public class CollisionEventDispatcher : MonoBehaviour
    {
        /// <summary>
        /// The game, the event dispatcher belongs to.
        /// </summary>
        [SerializeField]
        private UnityGame game = null;

        /// <summary>
        /// Gets or sets the game, the event dispatcher belongs to.
        /// </summary>
        public UnityGame Game
        {
            get
            {
                return this.game;
            }

            set
            {
                this.game = value;
            }
        }

        /// <summary>
        /// Initialize the event dispatcher.
        /// </summary>
        public void Start()
        {
            // If this is null, let there be exception carnage.
            this.Game = GameObject.FindObjectOfType<UnityGame>();
        }

        /// <summary>
        /// Send an event, if the object is entering a collision.
        /// </summary>
        /// <param name="collisionInfo">
        /// The <see cref="Collision"/> collision information, reported by Unity.
        /// </param>
        public void OnCollisionEnter(Collision collisionInfo)
        {
            CollidableComponent collidee = collisionInfo.collider.GetComponent<CollidableComponent>();
            if (collidee == null)
            {
                return;
            }

            int entityId = this.GetComponent<EntityComponent>().EntityId;
            CollisionEventData data = new CollisionEventData(entityId, collidee.EntityId, collisionInfo);
            this.Game.EventManager.FireEvent(
                                    new HelGamesEvent(
                                            CollisionEvents.CollisionEntered,
                                            data));
        }

        /// <summary>
        /// Send an event, if the object is entering a collision.
        /// </summary>
        /// <param name="collisionInfo">
        /// The <see cref="Collision"/> collision information, reported by Unity.
        /// </param>
        public void OnCollisionExit(Collision collisionInfo)
        {
            CollidableComponent collidee = collisionInfo.collider.GetComponent<CollidableComponent>();
            if (collidee == null)
            {
                return;
            }

            int entityId = this.GetComponent<EntityComponent>().EntityId;
            CollisionEventData data = new CollisionEventData(entityId, collidee.EntityId, collisionInfo);
            this.Game.EventManager.FireEvent(
                                    new HelGamesEvent(
                                            CollisionEvents.CollisionLeft,
                                            data));
        }

        /// <summary>
        /// Send an event, if the object is entering a collision.
        /// </summary>
        /// <param name="collider">
        /// The <see cref="Collider"/> collider of the trigger object, reported by Unity.
        /// </param>
        public void OnTriggerEnter(Collider collider)
        {
            CollidableComponent collidee = collider.GetComponent<CollidableComponent>();
            if (collidee == null)
            {
                return;
            }

            int entityId = this.GetComponent<EntityComponent>().EntityId;
            CollisionEventData data = new CollisionEventData(entityId, collidee.EntityId, collider);
            this.Game.EventManager.FireEvent(
                                    new HelGamesEvent(
                                            CollisionEvents.TriggerEntered,
                                            data));
        }

        /// <summary>
        /// Send an event, if the object is entering a collision.
        /// </summary>
        /// <param name="collider">
        /// The <see cref="Collider"/> collider of the trigger object, reported by Unity.
        /// </param>
        public void OnTriggerExit(Collider collider)
        {
            CollidableComponent collidee = collider.GetComponent<CollidableComponent>();
            if (collidee == null)
            {
                return;
            }

            int entityId = this.GetComponent<EntityComponent>().EntityId;
            CollisionEventData data = new CollisionEventData(entityId, collidee.EntityId, collider);
            this.Game.EventManager.FireEvent(
                                    new HelGamesEvent(
                                            CollisionEvents.TriggerLeft,
                                            data));
        }
    }
}