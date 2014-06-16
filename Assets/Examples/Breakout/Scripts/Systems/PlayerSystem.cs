// -----------------------------------------------------------------------
// <copyright file="PlayerSystem.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using HelGames.Teaching.ComponentSystem;
    using HelGames.Teaching.EventManager;
    using UnityEngine;

    /// <summary>
    /// Defines the PlayerSystem. This system contains the logic for handling components
    /// of type <see cref="PlayerComponent"/>. It handles scoring, as well as spawning
    /// of balls for each player.
    /// </summary>
    public class PlayerSystem : UnitySystem
    {
        /// <summary>
        /// The prefab to instanciate the ball.
        /// </summary>
        [SerializeField]
        private GameObject ballPrefab;

        /// <summary>
        /// The number of lives, a player gets.
        /// </summary>
        [SerializeField]
        private int defaultLives;

        /// <summary>
        /// Gets or sets the prefab to instanciate the ball.
        /// </summary>
        public GameObject BallPrefab
        {
            get
            {
                return this.ballPrefab;
            }

            set
            {
                this.ballPrefab = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of lives, a player gets.
        /// </summary>
        public int DefaultLives
        {
            get
            {
                return this.defaultLives;
            }

            set
            {
                this.defaultLives = value;
            }
        }

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

            // Register the component types, required by this system. Since this
            // system uses a state machine, it also needs to register all types
            // of components, required by the individual states.
            this.Game.ComponentSystem.RegisterComponentType<BallComponent>();
            this.Game.ComponentSystem.RegisterComponentType<PlayerComponent>();
            this.Game.ComponentSystem.RegisterComponentType<PaddleComponent>();

            // Register the events, required by this system
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentCreated, this.OnComponentCreated);
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
            this.Game.EventManager.RegisterListener(BreakoutEvents.SpawnBallsForPlayer, this.OnSpawnBallsForPlayer);
            this.Game.EventManager.RegisterListener(BreakoutEvents.LevelFinished, this.OnLevelFinished);
            this.Game.EventManager.RegisterListener(BreakoutEvents.LevelFailed, this.OnLevelFailed);
        }

        /// <summary>
        /// Destroy the system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentCreated, this.OnComponentCreated);
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
            this.Game.EventManager.RemoveListener(BreakoutEvents.SpawnBallsForPlayer, this.OnSpawnBallsForPlayer);
            this.Game.EventManager.RemoveListener(BreakoutEvents.LevelFinished, this.OnLevelFinished);
            this.Game.EventManager.RemoveListener(BreakoutEvents.LevelFailed, this.OnLevelFailed);
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
            if (evt.EventData is PlayerComponent)
            {
                PlayerComponent data = (PlayerComponent)evt.EventData;
                Debug.Log("Setting lives for player " + data.EntityId);
                data.Lives = this.DefaultLives;
                data.Score = 0;
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
            if (evt.EventData is DestructableComponent)
            {
                foreach (PlayerComponent player in this.Game.ComponentSystem.Components<PlayerComponent>())
                {
                    if (player.Lives > 0)
                    {
                        player.Score += ((DestructableComponent)evt.EventData).Points;
                    }
                }
            }
        }

        /// <summary>
        /// Handle SpawnBallsForPlayer events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnSpawnBallsForPlayer(IEvent evt)
        {
            PlayerComponent player = (PlayerComponent)evt.EventData;
            if (! this.Game.ComponentSystem.TryGetComponent(player.EntityId, out player))
            {
                return;
            }

            Debug.Log("Spawning balls for player " + player.EntityId);

            player.Lives--;
            Transform ballTransform;
            Transform paddleTransform;
            Vector3 paddleUpVector;
            BallComponent ball;
            int ballId;

            foreach (PaddleComponent paddle in this.Game.ComponentSystem.Components<PaddleComponent>())
            {
                if ((paddle == null) || (paddle.PlayerId != player.EntityId))
                {
                    continue;
                }

                // Instanciate the prefab and position it in the paddle
                GameObject ballObject = (GameObject)GameObject.Instantiate(this.BallPrefab);
                ballTransform = ballObject.transform;
                paddleTransform = paddle.gameObject.transform;
                ballTransform.parent = paddleTransform.parent;
                ballTransform.position = paddleTransform.position;

                // Move the ball on top of the paddle by using the y extent of
                // its collider and that of the paddle, rotated by the paddles
                // rotation, blindly assuming, the collider has an offset of (0, 0, 0).
                paddleUpVector = new Vector3(0.0f, ballObject.collider.bounds.extents.y, 0.0f);
                paddleUpVector += new Vector3(0.0f, paddle.gameObject.collider.bounds.extents.y, 0.0f);
                paddleUpVector = paddleTransform.rotation * paddleUpVector;

                // Make the vector a bit larger, so there isn't an
                // immediate collision with the paddle. Cheap hack.
                paddleUpVector *= 1.001f;
                ballTransform.position += paddleUpVector;

                // Set the velocity of the ball by rotating its default velocity
                // by the rotation of the paddle. It will start to move as soon
                // as the game continues.
                ball = ballObject.GetComponent<BallComponent>();
                ball.Velocity = paddleTransform.rotation * ball.DefaultVelocity;

                // And finally, assign an ID to all components. This assumes, that
                // the ball is a simple object, without any children.
                ballId = this.Game.ComponentSystem.GetUniqueEntityId();
                foreach (Component unityComponent in ballObject.GetComponents(typeof(Component)))
                {
                    if (unityComponent is IComponent)
                    {
                        ((IComponent)unityComponent).EntityId = ballId;
                        this.Game.EventManager.QueueEvent(new HelGamesEvent(ComponentSystemEvents.ComponentCreated, unityComponent));
                    }
                }
            }
        }

        /// <summary>
        /// Handle LevelFinished events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnLevelFinished(IEvent evt)
        {
            foreach (PlayerComponent player in this.Game.ComponentSystem.Components<PlayerComponent>())
            {
                if (player.Lives > 0)
                {
                    // Increase the lives of all players, that are still alive, by 1.
                    player.Lives += 2;
                }
            }
        }

        /// <summary>
        /// Handle LevelFailed events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnLevelFailed(IEvent evt)
        {
            foreach (PlayerComponent player in this.Game.ComponentSystem.Components<PlayerComponent>())
            {
                player.Lives = this.DefaultLives;
                player.Score = 0;
            }
        }
    }
}