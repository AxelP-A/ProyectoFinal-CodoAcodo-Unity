using UnityEngine;
using UnityEditor;

public class SlimUIWindow : EditorWindow {


	[MenuItem("Window/SlimUI Online Documentation")]
	public static void ShowWindow(){
		Application.OpenURL("https://www.slimui.com/documentation");
	}
}
