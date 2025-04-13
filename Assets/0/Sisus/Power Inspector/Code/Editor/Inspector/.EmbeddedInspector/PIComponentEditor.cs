#define ENABLE_PI_IN_DEFAULT_INSPECTOR

#if DEV_MODE && ENABLE_PI_IN_DEFAULT_INSPECTOR
using System;
using UnityEngine;
using UnityEditor;

namespace Sisus
{
	[CustomEditor(typeof(Component), true, isFallback = true)]
	[CanEditMultipleObjects]
	public class PIComponentEditor : Editor
	{
		private IComponentDrawer drawer;

		[NonSerialized]
		private bool setupDone;

		public override void OnInspectorGUI()
		{
			#if DEV_MODE //TEMP FOR TESTING
			if(Event.current.control && Event.current.alt)
			{
				base.OnInspectorGUI();
				return;
			}
			#endif

			if(!setupDone)
			{
				Setup();
			}

			if(drawer == null)
			{
				Debug.LogWarning("PIComponentEditor.drawer was null.");
				return;
			}

			//DrawGUI.BeginOnGUI(drawer.Inspector.Preferences, true);

			EmbeddedInspectorDrawer embeddedDrawer;
			EmbeddedInspectorDrawer.Get(out embeddedDrawer, this);
			embeddedDrawer.OnBeginOnGUI();

			drawer.Inspector.NowDrawingPart = InspectorPart.Viewport;

			if(drawer.Unfolded != ComponentUnfoldedUtility.GetIsUnfolded(drawer.UnityObject))
			{
				bool setUnfolded = !drawer.Unfolded;
				drawer.SetUnfolded(setUnfolded);
				//drawer.Unfoldedness = setUnfolded ? 1f : 0f;
			}

			if(drawer.Unfolded)
			{
				if(Event.current.control && Event.current.type == EventType.ScrollWheel && Event.current.delta.y != 0f)
				{
					y += Event.current.delta.y;
				}

				DrawGUI.IndentLevel = 1;
				//var position = new Rect(DrawGUI.LeftPadding, DrawGUI.RightPadding, Screen.width - DrawGUI.LeftPadding - DrawGUI.RightPadding, drawer.Height - drawer.HeaderHeight);
				var position = new Rect(0f, y, Screen.width, drawer.Height - drawer.HeaderHeight);
				//var position = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label, GUILayout.ExpandWidth(true), GUILayout.Height(drawer.Height - drawer.HeaderHeight));
				//position.x = 0f;
				//position.width = Screen.width;

				var members = drawer.MembersBuilt;

				Debug.Log(drawer+".DrawBodyMultiRow @ "+position+" with "+ members.Length+ " members, "+drawer.VisibleMembers.Length+ " visibleMembers, MembersAreVisible=" + drawer.MembersAreVisible+".");

				drawer.DrawBodyMultiRow(position);

				GUILayout.Space(position.height);

				DrawGUI.IndentLevel = 0;

				if(drawer.Selected)
				{
					drawer.DrawSelectionRect();
				}
			}

			//DrawGUI.LayoutSpace(position.height);

			//drawer.Inspector.NowDrawingPart = InspectorPart.None;

			InspectorUtility.EndInspector(drawer.Inspector);
		}

		private float y = 0f;

		private void Setup()
		{
			setupDone = true;

			drawer = GetDrawer();

			#if DEV_MODE && PI_ASSERTATIONS
			Debug.Assert(drawer != null);
			#endif
		}

		private IComponentDrawer GetDrawer()
		{
			EmbeddedInspectorDrawer embeddedDrawer;
			EmbeddedInspectorDrawer.Get(out embeddedDrawer, this);
			var rootDrawers = embeddedDrawer.inspector.State.drawers;

			for(int n = rootDrawers.Length - 1; n >= 0; n--)
			{
				var gameObjectDrawer = rootDrawers[n] as IGameObjectDrawer;
				if(gameObjectDrawer != null)
				{
					foreach(var componentDrawer in gameObjectDrawer)
					{
						if(componentDrawer.UnityObjects.ContentsMatch(targets))
						{
							#if DEV_MODE
							Debug.Log("Found component drawer: "+componentDrawer.GetType().Name);
							#endif
							return componentDrawer;
						}
					}
					continue;
				}

				var rootComponentDrawer = rootDrawers[n] as IComponentDrawer;
				if(rootComponentDrawer != null && rootComponentDrawer.UnityObjects.ContentsMatch(targets))
				{
					#if DEV_MODE
					Debug.Log("Found component drawer: "+rootComponentDrawer.GetType().Name);
					#endif
					return rootComponentDrawer;
				}
			}

			#if DEV_MODE
			Debug.LogWarning("Failed to find drawer for "+GetType().Name+" among "+ rootDrawers.Length + " root drawers.");
			#endif

			return null;
		}
	}
}
#endif