// -----------------------------------------------------------------------
// <copyright file="MovableSystem.cs" company="HelGames Company Identifier">
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
    /// Defines the MovableSystem. This system handles components of type
    /// <see cref="MovableComponent"/> and moves them, if the appropriate
    /// event is received and the system is enabled.
    /// </summary>
    public class MovableSystem : UnitySystem
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
            this.Game.ComponentSystem.RegisterComponentType<MovableComponent>();
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentCreated, this.OnComponentCreated);
            this.Game.EventManager.RegisterListener(BreakoutEvents.MoveEntityTo, this.OnMoveEntityTo);
            this.Game.EventManager.RegisterListener(BreakoutEvents.Pause, this.OnPause);
            this.Game.EventManager.RegisterListener(BreakoutEvents.Continue, this.OnContinue);
            this.Enabled = true;
        }

        /// <summary>
        /// Destroy this system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentCreated, this.OnComponentCreated);
            this.Game.EventManager.RemoveListener(BreakoutEvents.MoveEntityTo, this.OnMoveEntityTo);
            this.Game.EventManager.RemoveListener(BreakoutEvents.Pause, this.OnPause);
            this.Game.EventManager.RemoveListener(BreakoutEvents.Continue, this.OnContinue);
            base.Destroy();
        }

        /// <summary>
        /// Handle ComponentCreated events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnComponentCreated(IEvent evt)
        {
            if (evt.EventData is MovableComponent)
            {
                MovableComponent movable = (MovableComponent)evt.EventData;
                movable.Rigidbody = movable.gameObject.GetComponent<Rigidbody>();
            }
        }

        /// <summary>
        /// Handle MoveEntityTo events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnMoveEntityTo(IEvent evt)
        {
            MoveEntityToEventData data = (MoveEntityToEventData)evt.EventData;
            if ((! this.Enabled) && (! data.IgnorePaused))
            {
                return;
            }

            MovableComponent movable;
            if (! this.Game.ComponentSystem.TryGetComponent<MovableComponent>(data.EntityId, out movable))
            {
                return;
            }

            if (movable == null)
            {
                // This checks, whether the component is still considered alive by Unity.
                // The check is very important, as we are accessing components from Unity
                // now, instead of just C# context components. If the object is Unity-dead,
                // don't do anything here.
                return;
            }

            Vector3 target = data.Position;
            Transform transform = movable.gameObject.transform;
            Vector3 movementVector = target - transform.position;
            Collider collider = movable.gameObject.collider;
            Vector3 colliderExtents = collider.bounds.extents;
            float movableRange = Mathf.Max(Mathf.Max(colliderExtents.x, colliderExtents.y), colliderExtents.z) * 2.0f;
            RaycastHit hit;

            // Test to see, if there are any colliders in the movement direction and stop close to the hit point.
            // This is done for all movables here to illustrate the shared use of characteristics (Components) by
            // entities of different types (the Ball and the paddle in this case). The implementation is a hacky,
            // imperformant and buggy though and it should not be used for any real game. In those cases, you may
            // want to use different movement strategies for the paddle and the ball, as their requirements differ.
            if (Physics.Raycast(transform.position, movementVector, out hit, movableRange + movementVector.sqrMagnitude, movable.CollisionMask))
            {
                // There is a collider near by, that might be in the way at the hit point.
                Vector3 collisionPoint = hit.point;

                // Calculate a point outside the collider to cast from, using the maximum
                // extent of the collider along an axis times two, as that should definitely
                // be outside the collider.
                Vector3 movableCastPoint = movementVector.normalized;
                movableCastPoint *= movableRange * 2.0f;
                movableCastPoint += transform.position;

                // Check to find a point in the movement direction, that may collide.
                Ray ray = new Ray(movableCastPoint, movementVector * -1);
                if (collider.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // There is a point, that would actually collide. Substract
                    // its position from that of the blocking collider to get the
                    // new center of the movable. Because the collision vector starts
                    // at twice the distance, the actual distance is distance * 0.5, which
                    // in case of using square magnitude checks needs to also be squared.
                    if ((collisionPoint - hit.point).sqrMagnitude < movementVector.sqrMagnitude)
                    {
                        target = collisionPoint - (hit.point - transform.position);
                    }
                }
            }

            // Set the new target position using the entities transform (as opposed to
            // Rigidbody.MovePosition, as it seems to behave a bit weird).
            transform.position = target;
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