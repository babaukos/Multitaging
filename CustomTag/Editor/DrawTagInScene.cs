using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace Multitaging
{
[CustomEditor(typeof(CustomTag)), CanEditMultipleObjects]
public class DrawTagInScene : SceneView 
{
	public static List<CustomTag> customTagObj = new List<CustomTag>();
	private static GUIStyle content = new GUIStyle();

	[InitializeOnLoadMethod]
	private static void Startup()
	{
		onSceneGUIDelegate += OnSceneGUI;
		EditorApplication.hierarchyWindowChanged += PickTagHolders;
	}
	private static void OnDestroy() 
	{
		onSceneGUIDelegate -= OnSceneGUI;
		EditorApplication.hierarchyWindowChanged -= PickTagHolders;
	}

    //
	static void OnSceneGUI (SceneView sceneView)
	{
		Event e = Event.current;
		if (e.type == EventType.keyDown && e.keyCode == Prefs.primalKey && e.modifiers == Prefs.modifKey)
		{
			Prefs.showTags = !Prefs.showTags;
			e.Use();
		}

		if (Prefs.showTags && customTagObj.Count > 0)
		{
			for (int i = 0; i < customTagObj.Count; i++)
            {           
                float spasing = HandleUtility.GetHandleSize(customTagObj[i].transform.position) / 6;
                float offset = customTagObj[i].tags.Count * spasing;
                Vector3 position = customTagObj[i].transform.position + new Vector3(0, customTagObj[i].offset + offset, 0);
                for (int j = 0; j < customTagObj[i].tags.Count; j++)
                {
                    DrawText(customTagObj[i].tags[j], position - Vector3.up * j * spasing, Color.green);
                }
            }
		}
	}
    static void PickTagHolders()
	{
		customTagObj.Clear();
		customTagObj = FindObjectsOfType<CustomTag>().ToList();
	}

    //
	private static void DrawText(string text, Vector3 worldPos, Color textColor)
    {
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
           return;
        }

        content.normal.textColor = Color.black;
		content.contentOffset = new Vector2(2, 2);
        Handles.Label(worldPos, text, content);

		content.contentOffset = Vector2.zero;
        content.normal.textColor = textColor;
        Handles.Label(worldPos, text, content);
    }
}
}
