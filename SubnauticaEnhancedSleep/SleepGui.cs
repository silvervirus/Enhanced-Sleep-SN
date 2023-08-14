using System;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
	// Token: 0x02000006 RID: 6
	internal class SleepGui : MonoBehaviour
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002836 File Offset: 0x00000A36
		public void Awake()
		{
			SleepGui._Instance = this;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002840 File Offset: 0x00000A40
		public void Update()
		{
			bool flag = Player.main != null;
			if (flag)
			{
				int num = (int)Math.Floor((double)Mathf.Repeat(SleepPatcher.wentToSleep / 0.041666668f, 24f));
				int num2 = (int)Math.Floor((double)Mathf.Repeat(SleepPatcher.wentToSleep / 0.041666668f * 60f, 60f));
				int num3 = (int)Math.Floor((double)SleepPatcher.wentToSleepD);
				SleepGui.wentBedTime = num.ToString("00") + ":" + num2.ToString("00");
				SleepGui.wentBedDay = "Day " + num3.ToString();
				float num4 = DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
				int num5 = (int)Math.Floor((double)Mathf.Repeat(num4 * 24f, 24f));
				int num6 = (int)Math.Floor((double)Mathf.Repeat(num4 * 24f * 60f, 60f));
				int num7 = (int)Math.Floor((double)num4);
				SleepGui.cTime = num5.ToString("00") + ":" + num6.ToString("00");
				SleepGui.cDay = "Day " + num7.ToString();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002990 File Offset: 0x00000B90
		public void OnGUI()
		{
			bool flag = !SleepGui.isShow;
			if (flag)
			{
			}
			bool flag2 = Tiredness.isIngame && !uGUI.main.loading.isLoading && !uGUI.main.intro.showing;
			if (flag2)
			{
				bool isSleeping = SleepPatcher.isSleeping;
				if (isSleeping)
				{
					float num = (float)(Screen.width / 2);
					float num2 = (float)(Screen.height / 2);
					float num3 = 80f;
					GUIStyle guistyle = new GUIStyle();
					guistyle.normal.textColor = Color.white;
					guistyle.fontSize = 64;
					guistyle.alignment = TextAnchor.LowerCenter;
					GUIStyle guistyle2 = new GUIStyle();
					guistyle2.normal.textColor = Color.white;
					guistyle2.fontSize = 32;
					guistyle2.alignment = TextAnchor.UpperCenter;
					GUIStyle guistyle3 = new GUIStyle();
					guistyle3.normal.textColor = Color.white;
					guistyle3.fontSize = 16;
					guistyle3.alignment = TextAnchor.LowerRight;
					GUIStyle guistyle4 = new GUIStyle();
					guistyle4.normal.textColor = new Color(255f, 125f, 0f, 255f);
					guistyle4.fontSize = 16;
					guistyle4.alignment = TextAnchor.MiddleCenter;
					GUIStyle guistyle5 = new GUIStyle();
					guistyle5.normal.textColor = new Color(0f, 125f, 255f, 255f);
					guistyle5.fontSize = 16;
					guistyle5.alignment = TextAnchor.MiddleCenter;
					GUIStyle guistyle6 = new GUIStyle();
					guistyle6.normal.textColor = new Color(255f, 125f, 125f, 255f);
					guistyle6.fontSize = 16;
					guistyle6.alignment = TextAnchor.MiddleCenter;
					float x = num - num3 - 200f;
					float num4 = num2 - 40f - 20f;
					GUI.Label(new Rect(x, num4, 200f, 40f), SleepGui.wentBedDay, guistyle);
					GUI.Label(new Rect(x, num4 + 40f + 10f, 200f, 40f), SleepGui.wentBedTime, guistyle2);
					float x2 = num + num3;
					float num5 = num2 - 40f - 20f;
					GUI.Label(new Rect(x2, num5, 200f, 40f), SleepGui.cDay, guistyle);
					GUI.Label(new Rect(x2, num5 + 40f + 10f, 200f, 40f), SleepGui.cTime, guistyle2);
					Survival component = Player.main.GetComponent<Survival>();
					GUI.Label(new Rect(0f, num5 + 80f + 10f, (float)Screen.width, 40f), Player.main.liveMixin.health.ToString(), guistyle6);
					GUI.Label(new Rect(0f, num5 + 120f + 10f, (float)Screen.width, 40f), component.food.ToString(), guistyle4);
					GUI.Label(new Rect(0f, num5 + 160f + 10f, (float)Screen.width, 40f), component.water.ToString(), guistyle5);
					float x3 = (float)(Screen.width - 100);
					float y = (float)(Screen.height - 30);
					GUI.Label(new Rect(x3, y, 90f, 20f), "Press " + Main.config.Stopsleepkey.ToString() + " to wake up.", guistyle3);
				}
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002D30 File Offset: 0x00000F30
		public static void Load()
		{
			GameObject gameObject = new GameObject("SleepGUI").AddComponent<SleepGui>().gameObject;
		}

		// Token: 0x04000026 RID: 38
		public static SleepGui _Instance = null;

		// Token: 0x04000027 RID: 39
		private static string currentTiredness = "";

		// Token: 0x04000028 RID: 40
		private static float currentTirednessV = 0f;

		// Token: 0x04000029 RID: 41
		private static string wentBedTime = "";

		// Token: 0x0400002A RID: 42
		private static string wentBedDay = "";

		// Token: 0x0400002B RID: 43
		private static string cTime = "";

		// Token: 0x0400002C RID: 44
		private static string cDay = "";

		// Token: 0x0400002D RID: 45
		private static string CurrTime = "";

		// Token: 0x0400002E RID: 46
		private static string CurrDay = "";

		// Token: 0x0400002F RID: 47
		public static bool isShow = false;
	}
}
