using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Harmony.AI
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow window;
        private BehaviourTreeView graphView;

        private Texture2D indentationIcon;

        public void Configure(EditorWindow window, BehaviourTreeView graphView)
        {
            this.window = window;
            this.graphView = graphView;
            
            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Add Node"), 0));

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Action"), 1));
            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var type in types)
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(ObjectNames.NicifyVariableName(type.Name),indentationIcon))
                    {
                        level = 2, userData = type
                    });
                }
            }

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Composite"), 1));
            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var type in types)
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(ObjectNames.NicifyVariableName(type.Name), indentationIcon))
                    {
                        level = 2,
                        userData = type
                    });
                }
            }

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Decorator"), 1));
            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var type in types)
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(ObjectNames.NicifyVariableName(type.Name), indentationIcon))
                    {
                        level = 2,
                        userData = type
                    });
                }
            }

            tree.AddRange(new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Script..."), 1),
                new SearchTreeEntry(new GUIContent("New Action Node", indentationIcon))
                {
                    level = 2,
                    userData = graphView.scriptFileAssets[0]
                },
                new SearchTreeEntry(new GUIContent("New Composite Node", indentationIcon))
                {
                    level = 2,
                    userData = graphView.scriptFileAssets[1]
                },
                new SearchTreeEntry(new GUIContent("New Decorator Node", indentationIcon))
                {
                    level = 2,
                    userData = graphView.scriptFileAssets[2]
                }
            });

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var mousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent,
                context.screenMousePosition - window.position.position);
            var graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

            switch (SearchTreeEntry.userData)
            {
                case Type type:
                    graphView.CreateNode(type, graphMousePosition);
                    return true;
                case BehaviourTreeView.ScriptTemplate template:
                    graphView.CreateNewScript(template);
                    return true;
            }
            return false;
        }
    }
}

