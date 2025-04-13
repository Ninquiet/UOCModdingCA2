#define ENABLE_PI_IN_DEFAULT_INSPECTOR

#if DEV_MODE && ENABLE_PI_IN_DEFAULT_INSPECTOR
using System;
using UnityEngine;
using UnityEditor;

namespace Sisus
{
	[CustomEditor(typeof(GameObject))]
	[CanEditMultipleObjects]
	public class PIGameObjectEditor : Editor
	{
		private EmbeddedInspectorDrawer inspectorDrawer;
		private Editor baseEditor;
		private static Type gameObjectInspectorType;

		[NonSerialized]
		private bool setupDone;

		public IInspectorManager Manager
		{
			get
			{
				return InspectorManager.Instance();
			}
		}

		public IInspector Inspector
		{
			get
			{
				return inspectorDrawer.inspector;
			}
		}

		public bool MouseIsOver
		{
			get
			{
				return inspectorDrawer.MouseIsOver;
			}
		}

		public bool HasFocus
		{
			get
			{
				return inspectorDrawer.HasFocus;
			}
		}

		protected override void OnHeaderGUI()
		{
			if(!setupDone)
			{
				Setup();
			}

			if(HasFocus)
			{
				if(Manager.SelectedInspector != Inspector)
				{
					Manager.Select(Inspector, InspectorPart.Viewport, ReasonSelectionChanged.GainedFocus);
				}
			}
			else if(Manager.SelectedInspector == Inspector)
			{
				Manager.Select(null, InspectorPart.None, ReasonSelectionChanged.LostFocus);
			}

			inspectorDrawer.OnBeginOnGUI();

			//GUILayout.Space(-18f);

			inspectorDrawer.DrawToolbar();

			if(baseEditor == null)
			{
				Debug.LogWarning("baseEditor null. setupDone="+ setupDone);
				InspectorUtility.EndInspector(Inspector);
				return;
			}

			baseEditor.DrawHeader();

			DrawGUI.IndentLevel = 0;

			InspectorUtility.EndInspector(Inspector);
		}

		public override void OnInspectorGUI()
		{
			if(!setupDone)
			{
				Setup();
			}

			//Debug.Log(ToString() + ".OnInspectorGUI with Event=" + StringUtils.ToString(Event.current));

			baseEditor.OnInspectorGUI();

			//fix for weird gap and double line when overriding default GameObject editor
			GUILayout.Space(-5f);
		}

		private void Setup()
		{
			#if DEV_MODE
			Debug.Assert(!setupDone);
			#endif

			setupDone = true;

			EmbeddedInspectorDrawer.Get(out inspectorDrawer, this);

			if(gameObjectInspectorType == null)
			{
				gameObjectInspectorType = Types.GetInternalEditorType("UnityEditor.GameObjectInspector");
				#if DEV_MODE
				Debug.Assert(gameObjectInspectorType != null);
				#endif
			}

			try
			{
				baseEditor = CreateEditor(targets, gameObjectInspectorType);
				#if DEV_MODE
				Debug.Assert(baseEditor.GetType() != GetType());
				#endif
			}
			#if DEV_MODE
			catch(Exception e)
			{
				Debug.LogError("Editor.CreateEditor for targets " + StringUtils.TypesToString(targets) + " and GameObjectInspector " + e);
			#else
			catch
			{
			#endif
				return;
			}

			#if DEV_MODE
			Debug.Assert(baseEditor.GetType() != this.GetType()); // this fails for some reason with Editors.GetEditor!!!!
			Debug.Assert(baseEditor.GetType().Name == "GameObjectInspector");
			Debug.Assert(Types.GetInternalEditorType("UnityEditor.GameObjectInspector") != null);
			#endif
		}

		//private void OnDisable()
		//{
		//	setupDone = false;
		//}
	}
}
#endif