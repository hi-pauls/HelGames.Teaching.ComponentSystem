// -----------------------------------------------------------------------
// <copyright file="DestructableSystem.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.Common;
    using HelGames.Teaching.ComponentSystem;
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the DestructableSystem. This system handles <see cref="DestructableComponent"/>
    /// components and contains the logic to react to collision events, decreasing the health
    /// of any destructable, that is involved in a collision, by one. A destructable, that has
    /// a health of less than one, will be destroyed.
    /// </summary>
    public class DestructableSystem : UnitySystem
    {
        /// <summary>
        /// Initialize this system to a working state.
        /// </summary>
        /// <param name="game">
        /// The <see cref="IGame"/> game, that requested the initialization. That is the game,
        /// the system will be running in.
        /// </param>
        public override void Initialize(IGame game)
        {
            base.Initialize(game);
            this.Game.ComponentSystem.RegisterComponentType<DestructableComponent>();

            this.Game.EventManager.RegisterListener(CollisionEvents.CollisionEntered, this.OnCollisionEntered);
        }

        /// <summary>
        /// Destroy this system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(CollisionEvents.CollisionEntered, this.OnCollisionEntered);
            base.Destroy();
        }

        /// <summary>
        /// Handle CollisionEntered events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnCollisionEntered(IEvent evt)
        {
            CollisionEventData data = (CollisionEventData)evt.EventData;
            this.DecreaseHealth(data.ColliderId);
            this.DecreaseHealth(data.CollideeId);
        }

        /// <summary>
        /// Decrease the health of a destructable. Should the health of the destructable
        /// drop to zero or below, it will be destroyed.
        /// </summary>
        /// <param name="destructableId">
        /// The <see cref="int"/> ID of the destructable entity.
        /// </param>
        private void DecreaseHealth(int destructableId)
        {
            DestructableComponent destructable;
            if (! this.Game.ComponentSystem.TryGetComponent<DestructableComponent>(destructableId, out destructable))
            {
                return;
            }

            destructable.Health--;
            if (destructable.Health < 1)
            {
                // Destroy the entity.
                this.Game.EventManager.QueueEvent(
                                        new HelGamesEvent(
                                                ComponentSystemEvents.EntityDestroyed,
                                                destructable.EntityId));

                // Note, that this only works reliably, because Unity may destroy the unmanaged
                // context right here, but the component manager still holds a reference to the
                // managed context until the component is actually removed. Accessing any properties
                // of the un-managed context, like the transform of the entity, in an EntityDestroyed
                // or ComponentDestroyed event handler will fail with a NullReferenceException!
                // A safer way to do this would be to disable the game object. That however leaves
                // the components attached to said object and an UpdateComponentManagers event could
                // lead to those components being found again, depending on its implementation.
                GameObject.Destroy(destructable.gameObject);
            }
        }
    }
}