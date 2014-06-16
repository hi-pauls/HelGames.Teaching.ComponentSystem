// -----------------------------------------------------------------------
// <copyright file="LevelSystem.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Breakout
{
    using System.Collections.Generic;
    using HelGames.Teaching.Common;
    using HelGames.Teaching.ComponentSystem;
    using HelGames.Teaching.EventManager;
    using HelGames.Teaching.StateMachine;
    using UnityEngine;

    /// <summary>
    /// Defines the LevelSystem. It defines the level logic and operates on components
    /// of type <see cref="LevelComponent"/>. It defines the state machine, that contains
    /// the different states, the game goes through during gameplay and is responsible for
    /// loading levels.
    /// </summary>
    public class LevelSystem : UnitySystem
    {
        /// <summary>
        /// The name of the button, used to respawn the ball.
        /// </summary>
        [SerializeField]
        private string playPauseButtonName;

        /// <summary>
        /// The index of the first level.
        /// </summary>
        [SerializeField]
        private int startLevelIndex;

        /// <summary>
        /// Gets or sets the name of the button, used to respawn the ball.
        /// </summary>
        public string PlayPauseButtonName
        {
            get
            {
                return this.playPauseButtonName;
            }

            set
            {
                this.playPauseButtonName = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the first level.
        /// </summary>
        public int StartLevelIndex
        {
            get
            {
                return this.startLevelIndex;
            }

            set
            {
                this.startLevelIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the state machine, used for this systems logic.
        /// </summary>
        public StateMachine StateMachine { get; set; }

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
            this.Game.ComponentSystem.RegisterComponentType<LevelComponent>();
            this.Game.ComponentSystem.RegisterComponentType<BallComponent>();
            this.Game.ComponentSystem.RegisterComponentType<DestructableComponent>();
            this.Game.ComponentSystem.RegisterComponentType<PlayerComponent>();

            // Register the events, required by this system
            this.Game.EventManager.RegisterListener(BreakoutEvents.GameStarted, this.OnGameStarted);
            this.Game.EventManager.RegisterListener(BreakoutEvents.LoadLevel, this.OnLoadLevel);

            // Set up the state machine, top-down, as described in the model design
            // (Documents/Breakout Game StateMachine.png). This is mostly boilerplate,
            // but it represents the modeled behaviour and can be written down very
            // fast and with a lower error rate than defining the whole state machine
            // in this system. Check out LevelSystemWithoutStateMachine.cs for a
            // comparison.
            this.StateMachine = new StateMachine();

            LevelSpawnState spawnState = new LevelSpawnState(this);
            spawnState.BallsLeftEvent = LevelEvents.Finished;
            spawnState.AddTransition(LevelEvents.Finished, LevelStates.Paused);
            spawnState.GameOverEvent = LevelEvents.NoLivesLeft;
            spawnState.AddTransition(LevelEvents.NoLivesLeft, LevelStates.LevelFailed);
            this.StateMachine.AddState(LevelStates.Spawn, spawnState);

            LevelPlayingState playingState = new LevelPlayingState(this);
            playingState.PauseButtonEvent = LevelEvents.Pause;
            playingState.AddTransition(LevelEvents.Pause, LevelStates.Paused);
            playingState.NoBlocksLeftEvent = LevelEvents.NoBlocksLeft;
            playingState.AddTransition(LevelEvents.NoBlocksLeft, LevelStates.LevelFinished);
            playingState.NoBallsLeftEvent = LevelEvents.NoBallsLeft;
            playingState.AddTransition(LevelEvents.NoBallsLeft, LevelStates.Spawn);
            this.StateMachine.AddState(LevelStates.Playing, playingState);

            LevelPausedState pausedState = new LevelPausedState(this);
            pausedState.PlayButtonEvent = LevelEvents.Finished;
            pausedState.AddTransition(LevelEvents.Finished, LevelStates.Playing);
            this.StateMachine.AddState(LevelStates.Paused, pausedState);

            LevelFinishedState finishedState = new LevelFinishedState(this);
            finishedState.LevelLoadingEvent = LevelEvents.Finished;
            finishedState.AddTransition(LevelEvents.Finished, LevelStates.Loading);
            this.StateMachine.AddState(LevelStates.LevelFinished, finishedState);

            LevelFailedState failedState = new LevelFailedState(this);
            failedState.LevelLoadingEvent = LevelEvents.Finished;
            failedState.AddTransition(LevelEvents.Finished, LevelStates.Loading);
            this.StateMachine.AddState(LevelStates.LevelFailed, failedState);

            LevelLoadingState loadingState = new LevelLoadingState(this);
            loadingState.LevelLoadedEvent = LevelEvents.Finished;
            loadingState.AddTransition(LevelEvents.Finished, LevelStates.Spawn);
            this.StateMachine.AddState(LevelStates.Loading, loadingState);

            if (GameObject.FindObjectOfType<LevelComponent>() == null)
            {
                // This is a bit of a dirty hack, but it works in conjunction with the SystemSceneLoader component.
                this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LoadLevel, this.StartLevelIndex));
            }
        }

        /// <summary>
        /// Destroy the system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(BreakoutEvents.LoadLevel, this.OnLoadLevel);
            base.Destroy();
        }

        /// <summary>
        /// Update this system.
        /// </summary>
        public override void Update()
        {
            // This is the only thing, that needs to happen for the system to perform its job.
            // You can compare it to LevelSystemWithoutStateMachine for a monolithic implementation.
            this.StateMachine.Update();
        }

        /// <summary>
        /// Handle GameStarted events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnGameStarted(IEvent evt)
        {
            // Set the initial state.
            this.StateMachine.SetState(LevelStates.Spawn);
        }

        /// <summary>
        /// Handle LoadLevel events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnLoadLevel(IEvent evt)
        {
            int data = (int)evt.EventData;

            List<LevelComponent> currentLevels = new List<LevelComponent>(this.Game.ComponentSystem.Components<LevelComponent>());

            Application.LoadLevelAdditive(data);

            foreach (LevelComponent level in currentLevels)
            {
                GameObject.Destroy(level.gameObject);
            }

            this.Game.EventManager.QueueEvent(new HelGamesEvent(ComponentSystemEvents.UpdateComponentManagers, null));
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LevelLoaded, null));
        }
    }
}