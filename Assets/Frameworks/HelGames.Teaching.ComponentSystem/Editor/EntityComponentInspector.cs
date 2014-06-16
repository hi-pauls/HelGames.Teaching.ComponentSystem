// -----------------------------------------------------------------------
// <copyright file="EntityComponentInspector.cs" company="Paul Schulze (HelGames)">
// Copyright 2014 Paul Schulze (HelGames). All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem.Editor
{
    using HelGames.Teaching.ComponentSystem;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Defines the EntityComponent inspector.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EntityComponent))]
    public class EntityComponentInspector : Editor
    {
        /// <summary>
        /// The entered current ID.
        /// </summary>
        private int currentId;

        /// <summary>
        /// The value indicating whether the ID assignment options are open.
        /// </summary>
        private bool newIdAssignmentOpen = true;

        /// <summary>
        /// Draw the inspector GUI. This is called each frame as long as a
        /// a EntityComponent is selected.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();

            this.newIdAssignmentOpen = EditorGUILayout.Foldout(this.newIdAssignmentOpen, "Assign new ID");
            if (this.newIdAssignmentOpen)
            {
                this.currentId = EditorGUILayout.IntField("First ID", this.currentId);
                if (GUILayout.Button("Assign ID to all components"))
                {
                    IComponent component;
                    EntityComponent entity;
                    foreach (UnityEngine.Object obj in this.targets)
                    {
                        entity = (EntityComponent)obj;

                        foreach (Component unityComponent in entity.gameObject.GetComponents<MonoBehaviour>())
                        {
                            if (unityComponent is IComponent)
                            {
                                component = (IComponent)unityComponent;
                                if (component.EntityId != this.currentId)
                                {
                                    component.EntityId = this.currentId;
                                    EditorUtility.SetDirty(unityComponent);
                                }
                            }
                        }

                        this.currentId++;
                    }
                }
            }
        }
    }
}