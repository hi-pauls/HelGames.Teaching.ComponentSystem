// -----------------------------------------------------------------------
// <copyright file="LevelSystemWithoutStateMachine.cs" company="Paul Schulze (HelGames)">
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
    /// Defines the LevelSystemWithoutStateMachine. It defines the level logic and operates
    /// on components of type <see cref="LevelComponent"/>. It defines the state machine,
    /// that contains the different states, the game goes through during gameplay and is
    /// responsible for loading levels.
    /// <para>
    /// Note, that the name is technically wrong, as this system also uses a state
    /// machine approach, but it does so in its own implementation and not using
    /// the generalized HelGames.Teaching.StateMachine implementation. Apart from
    /// the boilerplate overhead in the StateMachine-based implementation and some
    /// additional state checks of event handlers in this implementation, the two
    /// approches use the same logic. With this small state machine, the system is
    /// still reasonably compact, but more complicated state machines will profit
    /// from the StateMachine abstraction in terms of readability.
    /// </para>
    /// </summary>
    public class LevelSystemWithoutStateMachine : UnitySystem
    {
        /// <summary>
        /// The current state, the system is in.
        /// </summary>
        private LevelStates state;

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
            this.Game.EventManager.RegisterListener(BreakoutEvents.LoadLevel, this.OnLoadLevel);

            // These events are contained in state implementations, when using the
            // StateMachine-based implementation. They are registered and removed
            // when entering and exiting the LevelPlayingState respectively. Because
            // the registration is done here, the event handlers need to check their
            // state of course.
            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
            this.Game.EventManager.RegisterListener(BreakoutEvents.LevelLoaded, this.OnLevelLoaded);

            // And finally, set the initial state
            this.state = LevelStates.Spawn;
        }

        /// <summary>
        /// Destroy the system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(BreakoutEvents.LoadLevel, this.OnLoadLevel);
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
            this.Game.EventManager.RemoveListener(BreakoutEvents.LevelLoaded, this.OnLevelLoaded);
            base.Destroy();
        }

        /// <summary>
        /// Update this system.
        /// </summary>
        public override void Update()
        {
            // This is an implementation of the state machine directly in the system,
            // instead of a separate, dedicated state machine structure. Compare it to
            // the LevelSystem to see the differences. Both approaches have their merrits.
            // Also note, that the spawn state was omitted here in the updates, as it does
            // not have any update logic.
            switch (this.state)
            {
                case LevelStates.Paused:
                    this.UpdatePausedState();
                    break;
                case LevelStates.Playing:
                    this.UpdatePlayingState();
                    break;
                case LevelStates.LevelFinished:
                    this.UpdateLevelFinishedState();
                    break;
                case LevelStates.LevelFailed:
                    this.UpdateLevelFailedState();
                    break;
                default:
                    // Don't do anything.
                    break;
            }
        }

        /// <summary>
        /// Update the system in paused state.
        /// </summary>
        private void EnterSpawnState()
        {
            this.state = LevelStates.Spawn;

            int ballCount = 0;

            foreach (PlayerComponent player in this.Game.ComponentSystem.Components<PlayerComponent>())
            {
                if (player.Lives > 0)
                {
                    this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.SpawnBallsForPlayer, player));

                    ballCount++;
                }
            }

            if (ballCount > 0)
            {
                this.EnterPausedState();
            }
            else
            {
                this.EnterLevelFailedState();
            }
        }

        /// <summary>
        /// Enter the paused state.
        /// </summary>
        private void EnterPausedState()
        {
            this.state = LevelStates.Paused;
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.Pause, null));
        }

        /// <summary>
        /// Update the system in paused state.
        /// </summary>
        private void UpdatePausedState()
        {
            if (Input.GetButtonUp(this.PlayPauseButtonName))
            {
                this.EnterPlayingState();
            }
        }

        /// <summary>
        /// Enter the paused state.
        /// </summary>
        private void EnterPlayingState()
        {
            this.state = LevelStates.Playing;
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.Continue, null));
        }

        /// <summary>
        /// Update the system in paused state.
        /// </summary>
        private void UpdatePlayingState()
        {
            if (Input.GetButtonUp(this.PlayPauseButtonName))
            {
                this.EnterPausedState();
            }
        }

        /// <summary>
        /// Enter the level finished state.
        /// </summary>
        private void EnterLevelFinishedState()
        {
            this.state = LevelStates.LevelFinished;
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.Pause, null));
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LevelFinished, null));
        }

        /// <summary>
        /// Update the system in paused state.
        /// </summary>
        private void UpdateLevelFinishedState()
        {
            if (Input.GetButtonUp(this.PlayPauseButtonName))
            {
                // Load the new level additive
                int nextLevelIndex = Application.loadedLevel + 1;
                if (nextLevelIndex >= Application.levelCount)
                {
                    nextLevelIndex = this.StartLevelIndex;
                }

                // Now queue the component manager update and send the transition event.
                this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LoadLevel, nextLevelIndex));

                // Since there is no explicit call to enter the loading state (because it doesn't
                // require any additional logic), the state is set directly. This could easily
                // be done in a separate method, but since it is only one line, it is acceptable
                // to just do it here directly. Should any logic be added to entering the failed
                // state, it would need its own method.
                this.state = LevelStates.Loading;
            }
        }

        /// <summary>
        /// Enter the level finished state.
        /// </summary>
        private void EnterLevelFailedState()
        {
            this.state = LevelStates.LevelFailed;
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.Pause, null));
            this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LevelFailed, null));
        }

        /// <summary>
        /// Update the system in paused state.
        /// </summary>
        private void UpdateLevelFailedState()
        {
            if (Input.GetButtonUp(this.PlayPauseButtonName))
            {
                this.Game.EventManager.QueueEvent(new HelGamesEvent(BreakoutEvents.LoadLevel, this.StartLevelIndex));
                this.state = LevelStates.Loading;
            }
        }

        /// <summary>
        /// Handle LevelLoaded events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnLevelLoaded(IEvent evt)
        {
            if (this.state.Equals(LevelStates.Loading))
            {
                this.EnterSpawnState();
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
            if (! this.state.Equals(LevelStates.Playing))
            {
                // This shouldn't happen, but if it does, ignore it.
                return;
            }

            // This handling assumes, that the BallComponent was already removed
            // from the component manager, as otherwise the count would still be
            // higher than 0 with no additional ball being destroyed and the game
            // locking. This is reasonable to assume, because the systems registers
            // its component types before registering for events itself, making the
            // component managers definitely register for the event first and
            // presumably also handling the event first. That is dangerous terrain
            // however and it may be better to introduce an additional event here,
            // that is queued after removal of the component from the component
            // manager.
            if ((evt.EventData is BallComponent) && (this.Game.ComponentSystem.ComponentCount<BallComponent>() < 1))
            {
                this.EnterSpawnState();
            }

            if ((evt.EventData is DestructableComponent) && this.Game.ComponentSystem.ComponentCount<DestructableComponent>() < 1)
            {
                this.EnterLevelFinishedState();
            }
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