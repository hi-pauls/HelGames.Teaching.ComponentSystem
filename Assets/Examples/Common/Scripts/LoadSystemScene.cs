// -----------------------------------------------------------------------
// <copyright file="LoadSystemScene.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.Common
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEngine;

    /// <summary>
    /// Defines the LoadSystemScene behaviour.
    /// </summary>
    public class LoadSystemScene : MonoBehaviour
    {
        /// <summary>
        /// The name of the system scene.
        /// </summary>
        [SerializeField]
        private string systemSceneName = "Startup";

        /// <summary>
        /// Gets or sets the name of the system scene.
        /// </summary>
        public string SystemSceneName
        {
            get
            {
                return this.systemSceneName;
            }

            set
            {
                this.systemSceneName = value;
            }
        }

        /// <summary>
        /// Start the LoadSystemScene shortly before the first update
        /// </summary>
        public void Start()
        {
            UnityGame game = GameObject.FindObjectOfType<UnityGame>();
            if (game == null)
            {
                Application.LoadLevelAdditive(this.SystemSceneName);
            }
        }
    }
}