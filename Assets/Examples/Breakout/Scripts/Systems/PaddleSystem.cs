// -----------------------------------------------------------------------
// <copyright file="PaddleSystem.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the PaddleSystem. This system contains the logic for components of
    /// type <see cref="PaddleComponent"/>. If the system is enabled, it will move
    /// entities with a paddle component according to the input, received from
    /// Unity according to the axis, configured in <see cref="PaddleComponent.InputAxis"/>.
    /// </summary>
    public class PaddleSystem : UnitySystem
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
            this.Game.ComponentSystem.RegisterComponentType<PaddleComponent>();

            this.Game.EventManager.RegisterListener(BreakoutEvents.Pause, this.OnPause);
            this.Game.EventManager.RegisterListener(BreakoutEvents.Continue, this.OnContinue);
            this.Enabled = true;
        }

        /// <summary>
        /// Destroy this system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(BreakoutEvents.Pause, this.OnPause);
            this.Game.EventManager.RemoveListener(BreakoutEvents.Continue, this.OnContinue);
            base.Destroy();
        }

        /// <summary>
        /// Update this system. If <see cref="PaddleSystem.Enabled"/> is set to <c>true</c>,
        /// all paddles will be moved by the input value of their configured axis.
        /// </summary>
        public override void Update()
        {
            if (! this.Enabled)
            {
                return;
            }

            float axisValue;
            Vector3 position;
            Vector3 target;

            foreach (PaddleComponent paddle in this.Game.ComponentSystem.Components<PaddleComponent>())
            {
                axisValue = Input.GetAxis(paddle.InputAxis);
                position = paddle.gameObject.transform.position;
                target = position + (paddle.MovementVector * axisValue * Time.deltaTime);

                MoveEntityToEventData data = new MoveEntityToEventData(paddle.EntityId, target);
                this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.MoveEntityTo, data));
            }
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