using System;
using UnityEngine;
using UnityEditor;

namespace Multitaging
{
    internal static class Prefs 
    {
		private static Vector2 scroll;
		public static KeyCode primalKey = KeyCode.T;
		public static EventModifiers modifKey = EventModifiers.Control;
		public static bool showTags { get; set;}


	    private static readonly GUIContent enabledTagContent;
		private static readonly GUIContent primalKeyContent;
		private static readonly GUIContent modifKeyContent;



        static Prefs() 
        {
            enabledTagContent = new GUIContent("Show Tags", "Enable or disable showing objects tags");
			primalKeyContent = new GUIContent("Activation Key:", "");
			modifKeyContent = new GUIContent("Key Modifire:", "");

			ReloadPrefs();
		}

		[PreferenceItem("Scene Multitag")]
        private static void OnPreferencesGUI() 
        {
			try {
				// Preferences GUI
				scroll = EditorGUILayout.BeginScrollView(scroll, false, false);
				{
					EditorGUILayout.Separator();
					showTags = EditorGUILayout.Toggle(enabledTagContent, showTags);
					primalKey = (KeyCode)EditorGUILayout.EnumPopup(primalKeyContent, primalKey);
					modifKey = (EventModifiers)EditorGUILayout.EnumPopup(modifKeyContent, modifKey);
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndScrollView();

				// Reset Defoult
				if(GUILayout.Button("Use Defaults", GUILayout.Width(120f))) 
				{
					EditorPrefs.DeleteKey("ShowTags");
					EditorPrefs.DeleteKey("ActivationKey");
					EditorPrefs.DeleteKey("ModifireKey");
	
					ReloadPrefs();
				}
				EditorApplication.RepaintHierarchyWindow();
				
				// Save the preferences
				if (GUI.changed)
				{	
					EditorPrefs.SetBool("ShowTags", showTags);
					EditorPrefs.SetString("ActivationKey", primalKey.ToString());
					EditorPrefs.SetString("ModifireKey", modifKey.ToString());

					EditorApplication.RepaintHierarchyWindow();
				}
			}
			catch(Exception e) 
			{
				EditorGUILayout.HelpBox(e.ToString(), MessageType.Error);
			}
		}
		
		private static void ReloadPrefs() 
		{
            showTags = EditorPrefs.GetBool("UseTooltip", false);
			primalKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), EditorPrefs.GetString("ActivationKey") );
			modifKey = (EventModifiers)System.Enum.Parse(typeof(EventModifiers), EditorPrefs.GetString("ModifireKey") );
		}
	}
}
