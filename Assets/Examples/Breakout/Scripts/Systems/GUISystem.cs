// -----------------------------------------------------------------------
// <copyright file="GUISystem.cs" company="HelGames Company Identifier">
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
    /// Defines the GUISystem. This system contains the logic to react to different
    /// game events and display or update the appropriate messages and score values.
    /// It does not have any component types, directly associated with it.
    /// </summary>
    public class GUISystem : UnitySystem
    {
        /// <summary>
        /// The text mesh to display the player score in.
        /// </summary>
        [SerializeField]
        private TextMesh scoreText;

        /// <summary>
        /// The text mesh to display the players lives in.
        /// </summary>
        [SerializeField]
        private TextMesh livesText;

        /// <summary>
        /// The text mesh to display success or failure of a level.
        /// </summary>
        [SerializeField]
        private TextMesh stateText;

        /// <summary>
        /// Gets or sets the text mesh to display the player score in.
        /// </summary>
        public TextMesh ScoreText
        {
            get
            {
                return this.scoreText;
            }

            set
            {
                this.scoreText = value;
            }
        }

        /// <summary>
        /// Gets or sets the text mesh to display the players lives in.
        /// </summary>
        public TextMesh LivesText
        {
            get
            {
                return this.livesText;
            }

            set
            {
                this.livesText = value;
            }
        }

        /// <summary>
        /// Gets or sets the text mesh to display success or failure of a level.
        /// </summary>
        public TextMesh StateText
        {
            get
            {
                return this.stateText;
            }

            set
            {
                this.stateText = value;
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
            this.Game.ComponentSystem.RegisterComponentType<PlayerComponent>();

            this.Game.EventManager.RegisterListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
            this.Game.EventManager.RegisterListener(BreakoutEvents.LevelFinished, this.OnLevelFinished);
            this.Game.EventManager.RegisterListener(BreakoutEvents.LevelFailed, this.OnLevelFailed);
            this.Game.EventManager.RegisterListener(BreakoutEvents.LevelLoaded, this.OnLevelLoaded);
            this.Game.EventManager.RegisterListener(BreakoutEvents.GameStarted, this.OnLevelLoaded);
            this.Game.EventManager.RegisterListener(BreakoutEvents.SpawnBallsForPlayer, this.OnUpdateGuiEvent);
        }

        /// <summary>
        /// Destroy this system.
        /// </summary>
        public override void Destroy()
        {
            this.Game.EventManager.RemoveListener(ComponentSystemEvents.ComponentDestroyed, this.OnComponentDestroyed);
            this.Game.EventManager.RemoveListener(BreakoutEvents.LevelFinished, this.OnLevelFinished);
            this.Game.EventManager.RemoveListener(BreakoutEvents.LevelFailed, this.OnLevelFailed);
            this.Game.EventManager.RemoveListener(BreakoutEvents.LevelLoaded, this.OnLevelLoaded);
            this.Game.EventManager.RemoveListener(BreakoutEvents.GameStarted, this.OnLevelLoaded);
            this.Game.EventManager.RemoveListener(BreakoutEvents.SpawnBallsForPlayer, this.OnUpdateGuiEvent);

            base.Destroy();
        }

        /// <summary>
        /// Handle ComponentDestroyed events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnComponentDestroyed(IEvent evt)
        {
            if ((evt.EventData is BallComponent) || (evt.EventData is DestructableComponent))
            {
                this.UpdateGUI();
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
            this.UpdateGUI();
            this.StateText.gameObject.SetActive(true);
            this.StateText.color = new Color(0.7f, 0.7f, 1.0f);
            this.StateText.text = "Success";
        }

        /// <summary>
        /// Handle LevelFailed events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnLevelFailed(IEvent evt)
        {
            this.UpdateGUI();
            this.StateText.gameObject.SetActive(true);
            this.StateText.color = new Color(1.0f, 0.7f, 0.7f);
            this.StateText.text = "Game Over";
        }

        /// <summary>
        /// Handle LevelLoaded events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnLevelLoaded(IEvent evt)
        {
            this.StateText.gameObject.SetActive(false);
            this.UpdateGUI();
        }

        /// <summary>
        /// Handle UpdateGuiEvent events.
        /// </summary>
        /// <param name="evt">
        /// The <see cref="IEvent"/> event to handle.
        /// </param>
        private void OnUpdateGuiEvent(IEvent evt)
        {
            this.UpdateGUI();
        }

        /// <summary>
        /// Update the UI of the game. This will fetch the score and lives
        /// values for all players and display it. Currently, only the values
        /// of the last player are displayed.
        /// </summary>
        private void UpdateGUI()
        {
            foreach (PlayerComponent player in this.Game.ComponentSystem.Components<PlayerComponent>())
            {
                this.LivesText.text = player.Lives.ToString();
                this.ScoreText.text = player.Score.ToString();
            }
        }
    }
}