using System;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
	// Token: 0x02000007 RID: 7
	internal class TirednessGui : MonoBehaviour
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002DBD File Offset: 0x00000FBD
		public void Awake()
		{
			TirednessGui._Instance = this;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public void Update()
		{
			bool flag = Player.main != null;
			if (flag)
			{
				float tiredness = Tiredness.tiredness;
				TirednessGui.currentTiredness = "Tir=" + tiredness.ToString();
				TirednessGui.currentTirednessV = ((tiredness > 100f) ? 100f : tiredness);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002E18 File Offset: 0x00001018
		public void OnGUI()
		{
			bool flag = !TirednessGui.isShow;
			if (flag)
			{
			}
			bool flag2 = Tiredness.isIngame && !uGUI.main.loading.isLoading && !uGUI.main.intro.showing;
			if (flag2)
			{
				GUIStyle guistyle = new GUIStyle();
				guistyle.normal.textColor = Color.white;
				guistyle.fontSize = 16;
				bool flag3 = Main.isDev && TirednessGui.showDevValues;
				if (flag3)
				{
					GUI.Label(new Rect(100f, 100f, 400f, 100f), TirednessGui.currentTiredness, guistyle);
				}
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.wrapMode = TextureWrapMode.Repeat;
				texture2D.SetPixel(0, 0, new Color(0f, 125f, 130f, 255f));
				texture2D.Apply();
				Texture2D texture2D2 = new Texture2D(1, 1);
				texture2D2.wrapMode = TextureWrapMode.Repeat;
				texture2D2.SetPixel(0, 0, new Color(0f, 0f, 0f, 255f));
				texture2D2.Apply();
				GUI.DrawTexture(new Rect((float)Screen.width - Main.config.minusfromwidth, (float)Screen.height - Main.config.minusfromheight, 100f, 20f), texture2D2);
				GUI.DrawTexture(new Rect((float)Screen.width - Main.config.minusfromwidth, (float)Screen.height - Main.config.minusfromheight, 1f * TirednessGui.currentTirednessV, 20f), texture2D);
				GUIStyle guistyle2 = new GUIStyle();
				guistyle2.alignment = TextAnchor.MiddleCenter;
				bool flag4 = TirednessGui.currentTirednessV >= 50f;
				if (flag4)
				{
					guistyle2.normal.textColor = Color.black;
				}
				else
				{
					guistyle2.normal.textColor = Color.white;
				}
				GUI.Label(new Rect((float)Screen.width - Main.config.minusfromwidth, (float)Screen.height - Main.config.minusfromheight, 100f, 20f), ((float)Math.Floor((double)TirednessGui.currentTirednessV)).ToString() ?? "", guistyle2);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000305C File Offset: 0x0000125C
		public static void Load()
		{
			GameObject gameObject = new GameObject("TirednessGUI").AddComponent<TirednessGui>().gameObject;
		}

		// Token: 0x04000030 RID: 48
		public static TirednessGui _Instance = null;

		// Token: 0x04000031 RID: 49
		public static string currentTiredness = "";

		// Token: 0x04000032 RID: 50
		public static float currentTirednessV = 0f;

		// Token: 0x04000033 RID: 51
		public static bool isShow = false;

		// Token: 0x04000034 RID: 52
		public static bool showDevValues = false;
	}
}
