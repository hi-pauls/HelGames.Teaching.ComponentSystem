// -----------------------------------------------------------------------
// <copyright file="UnityComponent.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using UnityEngine;

    /// <summary>
    /// Defines the UnityComponent.
    /// </summary>
    public class UnityComponent : MonoBehaviour, IComponent
    {
        /// <summary>
        /// The ID of the entity, the component belongs to.
        /// </summary>
        [SerializeField]
        private int entityId;

        /// <summary>
        /// Gets or sets the ID of the entity, the component belongs to.
        /// </summary>
        public int EntityId
        {
            get
            {
                return this.entityId;
            }

            set
            {
                this.entityId = value;
            }
        }
    }
}