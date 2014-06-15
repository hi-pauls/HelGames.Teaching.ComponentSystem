// -----------------------------------------------------------------------
// <copyright file="BallSystem.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
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
    /// Defines the BallSystem. It is responsible for the logic concerning the
    /// <see cref="BallComponent"/> components. This includes starting, stopping
    /// and moving balls. It however will not spawn balls, as that is a task,
    /// that is done on a per player basis and is therefor handled by the
    /// <see cref="PlayerSystem"/>.
    /// </summary>
    public class BallSystem : UnitySystem
    {
        /// <summary>
        /// Gets or sets a value indicating whether this system is enabled.
        /// </summary>
        public bool Enabled { get; set; }

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
            this.Game.ComponentSystem.RegisterComponentType<BallComponent>();
            this.Game.ComponentSystem.RegisterComponentType<CollidableComponent>();
            this.Game.ComponentSystem.RegisterComponentType<PaddleComponent>();
            this.Game.ComponentSystem.RegisterComponentType<OutComponent>();

            this.Game.EventManager.RegisterListener(CollisionEvents.CollisionEntered, this.OnCollisionEntered);
            this.Game.EventManager.RegisterListener(CollisionEvents.TriggerEntered, this.OnTriggerEntered);
            this.Game.EventManager.RegisterListener(BreakoutEvents.Pause, this.OnPause);
            this.Game.EventManager.RegisterListener(BreakoutEvents.Continue, this.OnContinue);

            // This is my personal best practice. Always enable a system by default or make it
            // configurable. I have slammed my head against walls because systems didn't update
            // and one of the main reasons was because some property defaulted to false.
            this.Enabled = true;
        }

        /// <summary>
        /// Destroy this system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(CollisionEvents.CollisionEntered, this.OnCollisionEntered);
            this.Game.EventManager.RemoveListener(CollisionEvents.TriggerEntered, this.OnTriggerEntered);
            this.Game.EventManager.RemoveListener(BreakoutEvents.Pause, this.OnPause);
            this.Game.EventManager.RemoveListener(BreakoutEvents.Continue, this.OnContinue);
            base.Destroy();
        }

        /// <summary>
        /// Update this systemm. Should the <see cref="BallSystem.Enabled"/> flag be set to <c>true</c>,
        /// this will move all the <see cref="BallComponent"/> components, using their current velocity.
        /// </summary>
        public override void Update()
        {
            if (! this.Enabled)
            {
                return;
            }

            Vector3 position;
            Vector3 target;
            foreach (BallComponent ball in this.Game.ComponentSystem.Components<BallComponent>())
            {
                if (ball == null)
                {
                    // Since we are accessing Unity components here, make sure they are still
                    // alive, as we are treating dead Unity components rather loosely in the
                    // component managers.
                    continue;
                }

                position = ball.gameObject.transform.position;
                target = position + (ball.Velocity * Time.deltaTime);

                MoveEntityToEventData data = new MoveEntityToEventData(ball.EntityId, target);
                this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.MoveEntityTo, data));
            }
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

            BallComponent ball;
            if (! this.Game.ComponentSystem.TryGetComponent<BallComponent>(data.ColliderId, out ball))
            {
                return;
            }

            // Why check here again? Because the check before was performed by a Unity bridge component, based on
            // the component being attached to a game object. You will want to check, whether the component is actually
            // being tracked by a component manager and probably access its values later.
            CollidableComponent collidable;
            if (! this.Game.ComponentSystem.TryGetComponent<CollidableComponent>(data.CollideeId, out collidable))
            {
                return;
            }

            // Remember the last player, who touched the ball.
            PaddleComponent paddle;
            if (this.Game.ComponentSystem.TryGetComponent<PaddleComponent>(data.CollideeId, out paddle))
            {
                ball.PlayerId = paddle.PlayerId;
            }

            // Reflect the ball off the collidable object.
            Collision collisionInfo = (Collision)data.Context;
            Vector3 velocity = Vector3.Reflect(ball.Velocity, collisionInfo.contacts[0].normal);

            // Make sure the velocity in Z direction is always zero. There should actually not ever be a value
            // zero value in Z as a result of reflecting a vector with value 0 the collision normal first place,
            // when considering a perfect spherical object, but ours is not a perfect sphere.
            velocity.z = 0.0f;

            // Make sure the ball is always moving vertically, as the game may be locked otherwise.
            velocity.x = Mathf.Max(0.5f, Mathf.Abs(velocity.x)) * Mathf.Sign(velocity.x);
            velocity.y = Mathf.Max(0.5f, Mathf.Abs(velocity.y)) * Mathf.Sign(velocity.y);
            ball.Velocity = velocity;
        }

        /// <summary>
        /// Handle TriggerEntered events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnTriggerEntered(IEvent evt)
        {
            CollisionEventData data = (CollisionEventData)evt.EventData;

            BallComponent ball;
            if (! this.Game.ComponentSystem.TryGetComponent<BallComponent>(data.ColliderId, out ball))
            {
                return;
            }

            OutComponent outZone;
            if (! this.Game.ComponentSystem.TryGetComponent<OutComponent>(data.CollideeId, out outZone))
            {
                return;
            }

            // Destroy the ball.
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.BallLostByPlayer, outZone.PlayerId));
            this.Game.EventManager.QueueEvent(
                                        new HelGamesEvent(
                                                ComponentSystemEvents.EntityDestroyed,
                                                ball.EntityId));
            GameObject.Destroy(ball.gameObject);
        }

        /// <summary>
        /// Handle Pause events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnPause(IEvent evt)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// Handle Continue events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnContinue(IEvent evt)
        {
            this.Enabled = true;
        }
    }
}