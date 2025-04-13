using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Sisus
{
	[Serializable]
	public class EmbeddedInspectorDrawer : IInspectorDrawer
	{
		private static Editors editors = new Editors();

		private static Dictionary<EditorWindow, EmbeddedInspectorDrawer> cachedEmbbeddedDrawers = new Dictionary<EditorWindow, EmbeddedInspectorDrawer>();
		private static Dictionary<EditorKey, Editor> cachedEditors = new Dictionary<EditorKey, Editor>(20);

		private static Type inspectorWindowType;
		private static Type propertyEditorType;
		private static FieldInfo allInspectorsField;
		private static IList allInspectors;
		private static PropertyInfo activeEditorsProperty;
		private static FieldInfo inspectorWindowEditorTrackerField;
		private static FieldInfo propertyEditorEditorTrackerField;
		private static PropertyInfo inspectorIsLocked;
		private static MethodInfo onHeaderGUI;

		private static Type gameObjectInspectorType;
		private static Type genericInspectorType;

		public EmbeddedInspector inspector;
		public Editor editor;
		public Object[] inspected;
		public LockedSelectionManager selectionManager;
		public UpdateEvent onUpdate;
		public float onGUIDeltaTime;

		public EditorWindow window;

		//public Editor baseEditor;
		private bool isPropertyEditor;

		public bool CanSplitView
		{
			get
			{
				return false;
			}
		}

		public bool NowClosing
		{
			get
			{
				return window == null;
			}
		}

		public UpdateEvent OnUpdate
		{
			get
			{
				return onUpdate;
			}

			set
			{
				onUpdate = value;
			}
		}

		public ISelectionManager SelectionManager
		{
			get
			{
				return selectionManager;
			}
		}

		public bool UpdateAnimationsNow
		{
			get
			{
				return true;
			}
		}

		public float AnimationDeltaTime
		{
			get
			{
				return onGUIDeltaTime;
			}
		}

		public IInspectorManager Manager
		{
			get
			{
				return InspectorManager.Instance();
			}
		}

		public IInspector MainView
		{
			get
			{
				return inspector;
			}
		}

		public Rect position
		{
			get
			{
				return window.position;
			}
		}

		public bool MouseIsOver
		{
			get
			{
				return window == EditorWindow.mouseOverWindow;
			}
		}

		public bool HasFocus
		{
			get
			{
				return window == EditorWindow.focusedWindow;
			}
		}

		public Object UnityObject
		{
			get
			{
				return window;
			}
		}

		public InspectorTargetingMode InspectorTargetingMode
		{
			get
			{
				return InspectorTargetingMode.All;
			}
		}

		public Editors Editors
		{
			get
			{
				return editors;
			}
		}

		public static EmbeddedInspectorDrawer Get(out EmbeddedInspectorDrawer embeddedDrawer, Editor editor)
		{
			var window = FindContainingWindow(editor);

			if(!cachedEmbbeddedDrawers.TryGetValue(window, out embeddedDrawer))
			{
				#if DEV_MODE
				Debug.Log("Creating new EmbeddedInspectorDrawer for " + (editor.target != null ? editor.target.GetType().Name : "null") + ".");
				#endif

				new EmbeddedInspectorDrawer(out embeddedDrawer, editor, window);
				cachedEmbbeddedDrawers[window] = embeddedDrawer;
			}
			else
			{
				#if DEV_MODE
				Debug.Log("Found existing EmbeddedInspectorDrawer for " + (editor.target != null ? editor.target.GetType().Name : "null") + ".");
				#endif

				embeddedDrawer.inspector.State.ViewIsLocked = GetWindowIsLocked(window);
				//embeddedDrawer.inspector.RebuildDrawers(editor.targets, false);
			}

			return embeddedDrawer;
		}

		public static bool GetWindowIsLocked(EditorWindow window)
		{
			if(window.GetType() == inspectorWindowType)
			{
				return (bool)inspectorIsLocked.GetValue(window, null);
			}

			// PropertyEditor is always locked
			return true;
		}

		public EmbeddedInspectorDrawer() { }

		private EmbeddedInspectorDrawer(out EmbeddedInspectorDrawer embeddedDrawer, Editor setEditor, EditorWindow containingWindow)
		{
			embeddedDrawer = this;

			editor = setEditor;
			window = containingWindow;
			inspected = setEditor.targets;
			CreateInspector(inspected, GetWindowIsLocked(containingWindow));
			selectionManager = new LockedSelectionManager(inspected);
			
			isPropertyEditor = containingWindow.GetType() == propertyEditorType;

			/*
			var editorKey = new EditorKey(inspected, false);
			if(cachedEditors.TryGetValue(editorKey, out baseEditor) && baseEditor != null && !Editors.DisposeIfInvalid(ref baseEditor))
			{
				Editors.OnBecameActive(baseEditor);
				return;
			}

			Type baseEditorType;
			if(setEditor.target != null && setEditor.target.GetType() == typeof(GameObject))
			{
				if(gameObjectInspectorType == null)
				{
					gameObjectInspectorType = Types.GetInternalEditorType("UnityEditor.GameObjectInspector");
					#if DEV_MODE
					Debug.Assert(gameObjectInspectorType != null);
					#endif
				}
				baseEditorType = gameObjectInspectorType;
				
			}
			else
			{
				if(genericInspectorType == null)
				{
					genericInspectorType = Types.GetInternalEditorType("UnityEditor.GenericInspector");
					#if DEV_MODE
					Debug.Assert(genericInspectorType != null);
					#endif
				}
				baseEditorType = genericInspectorType;
			}

			try
			{
				baseEditor = Editor.CreateEditor(inspected, baseEditorType);
			}
			#if DEV_MODE
			catch(Exception e)
			{
				Debug.LogError("Editor.CreateEditor for targets " + StringUtils.TypesToString(inspected) + " and editor type "+ (baseEditorType == null ? "null" : baseEditorType.Name)+" " + e);
			#else
			catch
			{
			#endif
				return;
			}

			cachedEditors[editorKey] = baseEditor;

			#if DEV_MODE
			Debug.Assert(baseEditor.GetType().Name == "GameObjectInspector" || baseEditor.GetType().Name == "GenericInspector");
			#endif
			*/
		}

		private static readonly Dictionary<Editor, EditorWindow> editorWindows = new Dictionary<Editor, EditorWindow>();

		public static EditorWindow FindContainingWindow(Editor editor)
		{
			EditorWindow window;
			if(editorWindows.TryGetValue(editor, out window))
			{
				return window;
			}

			if(inspectorWindowType == null)
			{
				inspectorWindowType = Types.GetInternalEditorType("UnityEditor.InspectorWindow");

				propertyEditorType = Types.GetInternalEditorType("UnityEditor.PropertyEditor");
				propertyEditorEditorTrackerField = inspectorWindowType.GetField("m_Tracker", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

				allInspectorsField = inspectorWindowType.GetField("m_AllInspectors", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

				inspectorWindowEditorTrackerField = inspectorWindowType.GetField("m_Tracker", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				

				activeEditorsProperty = inspectorWindowEditorTrackerField.FieldType.GetProperty("activeEditors", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

				allInspectors = allInspectorsField.GetValue(null) as IList;

				inspectorIsLocked = inspectorWindowType.GetProperty("isLocked", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			}

			if(propertyEditorType != null)
			{
				if(EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType() == propertyEditorType)
				{
					#if DEV_MODE
					Debug.LogWarning("Returning focused PropertyEditor window...");
					#endif

					editorWindows[editor] = EditorWindow.focusedWindow;
					return EditorWindow.focusedWindow;
				}

				//if(EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow.GetType() == propertyEditorType)
				//{
				//	#if DEV_MODE
				//	Debug.LogWarning("Returning mouseovered PropertyEditor window...");
				//	#endif
				//	editorWindows[editor] = EditorWindow.focusedWindow;
				//	return EditorWindow.mouseOverWindow;
				//}
			}

			if(allInspectors.Count == 0)
			{
				#if DEV_MODE
				Debug.Log("No inspectors seem to be open. Returning null.");
				#endif
				return null;
			}

			foreach(EditorWindow inspectorWindow in allInspectors)
			{
				var tracker = inspectorWindowEditorTrackerField.GetValue(inspectorWindow);
				foreach(var activeEditor in activeEditorsProperty.GetValue(tracker, null) as IEnumerable<Editor>)
				{
					if(activeEditor == editor)
					{
						editorWindows[editor] = inspectorWindow;
						return inspectorWindow;
					}
				}
			}

			if(EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType() == inspectorWindowType)
			{
				#if DEV_MODE
				Debug.LogWarning("Returning focused InspectorWindow...");
				#endif
				return EditorWindow.focusedWindow;
			}

			if(EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow.GetType() == inspectorWindowType)
			{
				#if DEV_MODE
				Debug.LogWarning("Returning mouseovered InspectorWindow...");
				#endif
				return EditorWindow.mouseOverWindow;
			}

			#if DEV_MODE
			Debug.LogWarning("Returning allInspectors["+(allInspectors.Count)+"], i.e. the last inspector...");
			#endif

			return allInspectors[allInspectors.Count - 1] as EditorWindow;
		}

		private void CreateInspector(Object[] inspect, bool lockView = false)
		{
			InspectorManager.Instance().Create(out inspector, this, InspectorUtility.Preferences, inspect, default(Vector2), lockView);

			#if DEV_MODE && PI_ASSERTATIONS
			var state = inspector.State;
			Debug.Assert(state.ViewIsLocked == lockView, ToString() +" state.ViewIsLocked ("+StringUtils.ToColorizedString(state.ViewIsLocked)+") did not match lockView ("+StringUtils.ToColorizedString(lockView)+") after Create");
			if(!state.inspected.ContentsMatch(inspect.RemoveNullObjects())) { Debug.LogError("state.inspected ("+StringUtils.TypesToString(state.inspected)+") != inspect.RemoveNullObjects ("+StringUtils.TypesToString(inspect.RemoveNullObjects())+")"); }
			#endif
		}

		public void OnBeginOnGUI()
		{
			Manager.ActiveInspector = inspector;
			DrawGUI.BeginOnGUI(inspector.Preferences, true);
			InspectorUtility.BeginInspectorDrawer(this, null);
			bool anyInspectorPartMouseovered = MouseIsOver;
			InspectorUtility.BeginInspector(inspector, ref anyInspectorPartMouseovered);
		}

		public void RefreshView()
		{
			GUI.changed = true;
			window.Repaint();
		}

		public void Repaint()
		{
			window.Repaint();
		}

		public bool SendEvent(Event e)
		{
			return window.SendEvent(e);
		}

		public void FocusWindow()
		{
			window.Focus();
		}

		public void CloseTab()
		{
			cachedEditors.Remove(new EditorKey(editor.targets, false));
			window.Close();
		}

		public void Message(GUIContent message, Object context = null, MessageType messageType = MessageType.Info, bool alsoLogToConsole = true)
		{
			switch(messageType)
			{
				default:
					Debug.Log(message.text, context);
					return;
				case MessageType.Warning:
					Debug.LogWarning(message.text, context);
					break;
				case MessageType.Error:
					Debug.LogError(message.text, context);
					break;
			}
		}

		public void DrawToolbar()
		{
			var toolbar = inspector.Toolbar;
			if(toolbar != null)
			{
				inspector.NowDrawingPart = InspectorPart.Toolbar;
				var toolbarRect = new Rect(0f, 0f, Screen.width, toolbar.Height);

				var currentEvent = Event.current;
				switch(currentEvent.type)
				{
					case EventType.MouseDown:
						if(Manager.MouseoveredInspectorPart == InspectorPart.Toolbar && MouseIsOver)
						{
							switch(currentEvent.button)
							{
								case 0:
									toolbar.OnClick(Event.current);
									break;
								case 1:
									toolbar.OnRightClick(Event.current);
									break;
								case 2:
									toolbar.OnMiddleClick(Event.current);
									break;
							}
						}
						break;
					case EventType.Layout:
						inspector.State.nextUpdateCachedValues--;
						if(inspector.State.nextUpdateCachedValues <= 0)
						{
							inspector.UpdateCachedValuesFromFields();
						}
						inspector.OnCursorPositionOrLayoutChanged();
						break;
					case EventType.MouseMove:
					case EventType.MouseDrag:
					case EventType.DragUpdated:
						if(inspector.IgnoreViewportMouseInputs())
						{
							#if DEV_MODE
							//Debug.Log("ignoring "+ currentEvent.type+"...");
							#endif
							break;
						}

						inspector.OnCursorPositionOrLayoutChanged();
						RefreshView();
						break;
				}

				if(MouseIsOver)
				{
					if(toolbarRect.Contains(Event.current.mousePosition))
					{
						Manager.SetMouseoveredInspector(inspector, InspectorPart.Toolbar);
					}
					else
					{
						Manager.SetMouseoveredInspector(inspector, InspectorPart.Viewport);
					}
				}
				else if(Manager.MouseoveredInspector == inspector)
				{
					Manager.SetMouseoveredInspector(null, InspectorPart.None);
				}
				
				toolbar.Draw(toolbarRect);
				inspector.NowDrawingPart = InspectorPart.None;
				GUILayout.Space(toolbarRect.height);
			}
		}
	}
}