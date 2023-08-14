using System;
using System.IO;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
	// Token: 0x02000005 RID: 5
	public class Tiredness : MonoBehaviour
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002588 File Offset: 0x00000788
		public static void onEnable()
		{
			Tiredness.isIngame = true;
			Tiredness.tiredness = 0f;
			Tiredness.lastUpdate = 0f;
			Tiredness.LoadTiredness();
			Main.EnhancedSleepComp.Load();
			TirednessGui.Load();
			SleepGui.Load();
			bool flag = !Tiredness.wasSleepIntroSent;
			if (flag)
			{
				Tiredness.wasSleepIntroSent = true;
				Tiredness.gameStartSleepIntroNotification01.Play();
				Tiredness.gameStartSleepIntroNotification02.Play();
				Tiredness.gameStartSleepIntroNotification03.Play();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025FD File Offset: 0x000007FD
		public static void onDisable()
		{
			Tiredness.isIngame = false;
			Tiredness.tiredness = 0f;
			Tiredness.lastUpdate = 0f;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000261C File Offset: 0x0000081C
		public static void initAssets()
		{
			Tiredness.gameStartSleepIntroNotification01 = new PDANotification();
			Tiredness.gameStartSleepIntroNotification01.sound = null;
			Tiredness.gameStartSleepIntroNotification01.text = "For your safety, you have been injected with a sleep suppressing serum.";
			Tiredness.gameStartSleepIntroNotification02 = new PDANotification();
			Tiredness.gameStartSleepIntroNotification02.sound = null;
			Tiredness.gameStartSleepIntroNotification02.text = "Your need for sleep has been removed for 12 days.";
			Tiredness.gameStartSleepIntroNotification03 = new PDANotification();
			Tiredness.gameStartSleepIntroNotification03.sound = null;
			Tiredness.gameStartSleepIntroNotification03.text = "Please try to find a way to sleep as soon as possible.";
			Tiredness.tiredNotification = new PDANotification();
			Tiredness.tiredNotification.sound = null;
			Tiredness.tiredNotification.text = "You are getting tired. Getting some sleep is highly recommended.";
			Tiredness.tiredWarningNotification = new PDANotification();
			Tiredness.tiredWarningNotification.sound = null;
			Tiredness.tiredWarningNotification.text = "You are really tired. Find some sleep immediately.";
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000026E0 File Offset: 0x000008E0
		public static void SaveTiredness()
		{
			Tiredness.TirednessSaveData tirednessSaveData = new Tiredness.TirednessSaveData
			{
				_tiredness = Tiredness.tiredness,
				_lastUpdate = Tiredness.lastUpdate,
				_wasSleepIntroSent = Tiredness.wasSleepIntroSent
			};
			string contents = JsonConvert.SerializeObject(tirednessSaveData);
			Directory.CreateDirectory(Main.GetSaveGameDir());
			File.Create(Main.GetSaveGameDir() + "/tiredness.json").Close();
			File.WriteAllText(Main.GetSaveGameDir() + "/tiredness.json", contents);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000275C File Offset: 0x0000095C
		public static void LoadTiredness()
		{
			bool flag = File.Exists(Main.GetSaveGameDir() + "/tiredness.json");
			if (flag)
			{
				string text = File.ReadAllText(Main.GetSaveGameDir() + "/tiredness.json");
				Tiredness.TirednessSaveData tirednessSaveData = JsonConvert.DeserializeObject<Tiredness.TirednessSaveData>(text);
				Tiredness.tiredness = tirednessSaveData._tiredness;
				Tiredness.lastUpdate = tirednessSaveData._lastUpdate;
				Tiredness.wasSleepIntroSent = tirednessSaveData._wasSleepIntroSent;
			}
			else
			{
				Tiredness.tiredness = 0f;
				Tiredness.lastUpdate = DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
				Tiredness.wasSleepIntroSent = false;
			}
		}

		// Token: 0x0400001D RID: 29
		public static float tiredness;

		// Token: 0x0400001E RID: 30
		public static float lastUpdate;

		// Token: 0x0400001F RID: 31
		public static bool isIngame = false;

		// Token: 0x04000020 RID: 32
		public static bool wasSleepIntroSent = false;

		// Token: 0x04000021 RID: 33
		public static PDANotification gameStartSleepIntroNotification01 = new PDANotification();

		// Token: 0x04000022 RID: 34
		public static PDANotification gameStartSleepIntroNotification02 = new PDANotification();

		// Token: 0x04000023 RID: 35
		public static PDANotification gameStartSleepIntroNotification03 = new PDANotification();

		// Token: 0x04000024 RID: 36
		public static PDANotification tiredNotification = new PDANotification();

		// Token: 0x04000025 RID: 37
		public static PDANotification tiredWarningNotification = new PDANotification();

		// Token: 0x0200000E RID: 14
		[HarmonyPatch(typeof(Player))]
		[HarmonyPatch("Update")]
		public class PlayerTirednessPatch
		{
			// Token: 0x0600002F RID: 47 RVA: 0x00003768 File Offset: 0x00001968
			public static void Postfix(Player __instance)
			{
				bool flag = Tiredness.isIngame && !uGUI.main.loading.isLoading && !uGUI.main.intro.showing;
				if (flag)
				{
					bool flag2 = (float)Math.Floor(DayNightCycle.main.GetDay()) >= Main.config.timetosleep && Tiredness.lastUpdate <= DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
					if (flag2)
					{
						float tiredness = Tiredness.tiredness;
						float lastUpdate = Tiredness.lastUpdate;
						float num = DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
						float num2 = num - lastUpdate;
						float num3 = (float)((double)num2 / (1.0 / (double)Main.config.timePassedHours));
						float num4 = num3 * Main.config.timePassedMinutes;
						float sleeplooseFactor = Main.config.sleeplooseFactor;
						float sleeprecoverFactor = Main.config.sleeprecoverFactor;
						bool isSleeping = SleepPatcher.isSleeping;
						if (isSleeping)
						{
							bool flag3 = num4 >= 1f;
							if (flag3)
							{
								float num5 = (float)((double)num4 / (43.2 / (double)sleeprecoverFactor));
								float num6 = tiredness - num5;
								bool flag4 = num6 > 100f;
								if (flag4)
								{
									num6 = 100f;
								}
								else
								{
									bool flag5 = num6 < 0f;
									if (flag5)
									{
										num6 = 0f;
									}
								}
								Tiredness.tiredness = num6;
								Tiredness.lastUpdate = num;
							}
						}
						else
						{
							bool flag6 = num4 >= 2f;
							if (flag6)
							{
								float num7 = (float)((double)num4 / (43.2 / (double)sleeplooseFactor));
								float num8 = tiredness + num7;
								bool flag7 = num8 > 100f;
								if (flag7)
								{
									num8 = 100f;
								}
								else
								{
									bool flag8 = num8 < 0f;
									if (flag8)
									{
										num8 = 0f;
									}
								}
								Tiredness.tiredness = num8;
								Tiredness.lastUpdate = num;
								float num9 = 25f;
								float num10 = 50f;
								bool flag9 = tiredness < num9 && num8 >= num9 && num8 < num10;
								if (!flag9)
								{
									bool flag10 = tiredness < num10 && num8 >= num9;
									if (flag10)
									{
									}
								}
								bool flag11 = Tiredness.tiredness < 0f;
								if (flag11)
								{
									Tiredness.tiredness = 0f;
								}
								bool flag12 = Tiredness.tiredness >= 0f;
								if (flag12)
								{
									__instance.liveMixin.SendMessage("well rested");
								}
								bool flag13 = Tiredness.tiredness >= 50f;
								if (flag13)
								{
									__instance.liveMixin.TakeDamage(0f, default(Vector3), DamageType.Normal, null);
								}
								bool flag14 = Tiredness.tiredness >= 100f;
								if (flag14)
								{
									__instance.liveMixin.TakeDamage(Main.config.FullSleepdamage, default(Vector3), DamageType.Normal, null);
								}
							}
						}
					}
					else
					{
						bool flag15 = Tiredness.lastUpdate > DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
						if (!flag15)
						{
							float lastUpdate2 = DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
							Tiredness.lastUpdate = lastUpdate2;
						}
					}
				}
			}
		}

		// Token: 0x0200000F RID: 15
		[HarmonyPatch(typeof(Survival))]
		[HarmonyPatch("Eat")]
		public class PlayerEatPatch
		{
			// Token: 0x06000031 RID: 49 RVA: 0x00003AB4 File Offset: 0x00001CB4
			public static void Postfix(Survival __instance, GameObject useObj)
			{
				bool flag = useObj != null;
				if (flag)
				{
					TechType techType = CraftData.GetTechType(useObj);
					bool flag2 = TechType.Coffee == techType;
					if (flag2)
					{
						Tiredness.tiredness -= Main.config.CoffeeTiredness;
					}
					TechType techType2 = CraftData.GetTechType(useObj);
					bool flag3 = TechType.FilteredWater == techType2;
					if (flag3)
					{
						Tiredness.tiredness -= Main.config.CoffeeTiredness;
					}
					TechType techType3 = CraftData.GetTechType(useObj);
					bool flag4 = TechType.DisinfectedWater == techType3;
					if (flag4)
					{
						Tiredness.tiredness -= Main.config.CoffeeTiredness;
					}
					TechType techType4 = CraftData.GetTechType(useObj);
					bool flag5 = TechType.BigFilteredWater == techType4;
					if (flag5)
					{
						Tiredness.tiredness -= Main.config.CoffeeTiredness;
					}
					TechType techType5 = CraftData.GetTechType(useObj);
					bool flag6 = TechType.WaterFiltrationSuitWater == techType5;
					if (flag6)
					{
						Tiredness.tiredness -= Main.config.CoffeeTiredness;
					}
				}
			}
		}

		// Token: 0x02000010 RID: 16
		public class MyTirednessManager : MonoBehaviour
		{
			// Token: 0x06000033 RID: 51 RVA: 0x00003BB2 File Offset: 0x00001DB2
			public void Start()
			{
				base.InvokeRepeating("ApplyTirednessOverTimeLoop", 3f, 3f);
			}

			// Token: 0x06000034 RID: 52 RVA: 0x00003BCC File Offset: 0x00001DCC
			private void ApplyTirednessOverTimeLoop()
			{
				bool flag = !base.enabled || !this._applyTiredness;
				if (flag)
				{
				}
			}

			// Token: 0x06000035 RID: 53 RVA: 0x00003BF4 File Offset: 0x00001DF4
			public void TirednessOverTime(bool switchOn)
			{
				this._applyTiredness = switchOn;
			}

			// Token: 0x04000037 RID: 55
			private bool _applyTiredness = false;
		}

		// Token: 0x02000011 RID: 17
		[HarmonyPatch(typeof(Bench), "SwitchSittingEffects")]
		internal static class Bench_SwitchSittingEffects
		{
			// Token: 0x06000037 RID: 55 RVA: 0x00003C10 File Offset: 0x00001E10
			[HarmonyPostfix]
			internal static void Postfix(Player player, bool switchOn)
			{
				Tiredness.MyTirednessManager component = Player.main.gameObject.GetComponent<Tiredness.MyTirednessManager>();
				bool flag = component == null;
				if (flag)
				{
					Tiredness.tiredness -= Main.config.BenchTiredness;
				}
			}
		}

		// Token: 0x02000012 RID: 18
		[Serializable]
		internal class TirednessSaveData
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000038 RID: 56 RVA: 0x00003C4E File Offset: 0x00001E4E
			// (set) Token: 0x06000039 RID: 57 RVA: 0x00003C56 File Offset: 0x00001E56
			public float _tiredness { get; set; }

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600003A RID: 58 RVA: 0x00003C5F File Offset: 0x00001E5F
			// (set) Token: 0x0600003B RID: 59 RVA: 0x00003C67 File Offset: 0x00001E67
			public float _lastUpdate { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600003C RID: 60 RVA: 0x00003C70 File Offset: 0x00001E70
			// (set) Token: 0x0600003D RID: 61 RVA: 0x00003C78 File Offset: 0x00001E78
			public bool _wasSleepIntroSent { get; set; }
		}
	}
}
