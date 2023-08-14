using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
	// Token: 0x02000004 RID: 4
	internal class SleepPatcher
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002517 File Offset: 0x00000717
		public static void invokeAssets()
		{
		}

		// Token: 0x04000013 RID: 19
		public static FMODAsset FMA_nosleep_tooThirsty = new FMODAsset();

		// Token: 0x04000014 RID: 20
		public static float wentToSleep = 0f;

		// Token: 0x04000015 RID: 21
		public static float wentToSleepD = 0f;

		// Token: 0x04000016 RID: 22
		public static float lastUpdate = 0f;

		// Token: 0x04000017 RID: 23
		public static bool isSleeping = false;

		// Token: 0x04000018 RID: 24
		public static bool isSkippingTime = false;

		// Token: 0x04000019 RID: 25
		public static bool buttonReleasedSinceSkipping = false;

		// Token: 0x0400001A RID: 26
		public static readonly bool useCustomSleepTime = true;

		// Token: 0x0400001B RID: 27
		public static readonly float customSleepTime = 500f;

		// Token: 0x0400001C RID: 28
		public static readonly float customSleepRTime = 20f;

		// Token: 0x02000009 RID: 9
		[HarmonyPatch(typeof(Bed))]
		[HarmonyPatch("EnterInUseMode")]
		private class EnterPatch
		{
			// Token: 0x06000025 RID: 37 RVA: 0x00003138 File Offset: 0x00001338
			public static bool Prefix(Bed __instance)
			{
				bool useCustomSleepTime = SleepPatcher.useCustomSleepTime;
				if (useCustomSleepTime)
				{
					typeof(Bed).GetField("kSleepGameTimeDuration", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, SleepPatcher.customSleepTime);
					typeof(Bed).GetField("kSleepRealTimeDuration", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, SleepPatcher.customSleepRTime);
				}
				SleepPatcher.wentToSleep = DayNightCycle.main.GetDayNightCycleTime();
				SleepPatcher.wentToSleepD = (float)DayNightCycle.main.GetDay();
				SleepPatcher.isSleeping = true;
				return true;
			}
		}

		// Token: 0x0200000A RID: 10
		[HarmonyPatch(typeof(Bed))]
		[HarmonyPatch("OnHandClick")]
		private class HandPatch
		{
			// Token: 0x06000027 RID: 39 RVA: 0x000031CC File Offset: 0x000013CC
			public static bool Prefix(Bed __instance, GUIHand hand)
			{
				bool flag = (hand.player.GetComponent<Survival>().food <= 25f || Player.main.GetComponent<Survival>().water <= 25f) && GameModeUtils.IsOptionActive(GameModeOption.None);
				bool result;
				if (flag)
				{
					bool flag2 = hand.player.GetComponent<Survival>().food <= 25f && hand.player.GetComponent<Survival>().water <= 25f;
					if (flag2)
					{
						ErrorMessage.AddWarning("You can't sleep now because you are too hungry and thirsty! Drink and eat something before you go to bed.");
					}
					else
					{
						bool flag3 = hand.player.GetComponent<Survival>().food <= 25f || hand.player.GetComponent<Survival>().water > 25f;
						if (flag3)
						{
							ErrorMessage.AddWarning("You can't sleep now because you are too hungry! Go eat something before you go to bed.");
						}
						else
						{
							bool flag4 = hand.player.GetComponent<Survival>().food > 25f || hand.player.GetComponent<Survival>().water <= 25f;
							if (flag4)
							{
								ErrorMessage.AddWarning("You can't sleep now because you are too thirsty! Drink something before you go to bed.");
							}
							else
							{
								ErrorMessage.AddWarning("You can't sleep now! Try to drink and eat something and try again!");
							}
						}
					}
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x0200000B RID: 11
		[HarmonyPatch(typeof(Bed))]
		[HarmonyPatch("ExitInUseMode")]
		private class ExitPatch
		{
			// Token: 0x06000029 RID: 41 RVA: 0x00003306 File Offset: 0x00001506
			public static void Postfix(Bed __instance, Player player)
			{
				SleepPatcher.isSleeping = false;
			}
		}

		// Token: 0x0200000C RID: 12
		[HarmonyPatch(typeof(Player))]
		[HarmonyPatch("Update")]
		public class PlayerSleepPatch
		{
			// Token: 0x0600002B RID: 43 RVA: 0x00003310 File Offset: 0x00001510
			public static void Postfix(Player __instance)
			{
				bool flag = Tiredness.isIngame && !uGUI.main.loading.isLoading && !uGUI.main.intro.showing;
				if (flag)
				{
					bool isSleeping = SleepPatcher.isSleeping;
					if (isSleeping)
					{
						float lastUpdate = SleepPatcher.lastUpdate;
						float num = DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
						float num2 = num - lastUpdate;
						float num3 = (float)((double)num2 / (1.0 / (double)Main.config.timePassedHours));
						float num4 = num3 * Main.config.timePassedMinutes;
						double num5 = (double)Main.config.sleeplooseFactor;
						double num6 = (double)Main.config.sleeprecoverFactor;
						bool flag2 = num4 >= 1f;
						if (flag2)
						{
							bool flag3 = GameModeUtils.IsOptionActive(GameModeOption.NoSurvival);
							if (flag3)
							{
								float num7 = (float)((double)num4 / (4.8 / num6));
								float num8 = __instance.liveMixin.health + num7;
								bool flag4 = num8 < 0f;
								if (flag4)
								{
									num8 = 0f;
								}
								bool flag5 = num8 >= __instance.liveMixin.maxHealth;
								if (flag5)
								{
									num8 = __instance.liveMixin.maxHealth;
								}
								__instance.liveMixin.health = num8;
							}
							else
							{
								Survival component = __instance.GetComponent<Survival>();
								float num9 = (float)((double)num4 / (25.2 / num5));
								float num10 = component.food - num9;
								float num11 = (float)((double)num4 / (18.0 / num5));
								float num12 = component.water - num11;
								float num13 = (float)((double)num4 / (4.8 / num6));
								float num14 = __instance.liveMixin.health + num13;
								bool flag6 = num12 <= 0f;
								if (flag6)
								{
									num12 = 0f;
								}
								bool flag7 = num10 <= 0f;
								if (flag7)
								{
									num10 = 0f;
								}
								bool flag8 = num14 < 0f;
								if (flag8)
								{
									num14 = 0f;
								}
								bool flag9 = num14 >= __instance.liveMixin.maxHealth;
								if (flag9)
								{
									num14 = __instance.liveMixin.maxHealth;
								}
								component.water = num12;
								component.food = num10;
								__instance.liveMixin.health = num14;
							}
							SleepPatcher.lastUpdate = DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
						}
					}
					else
					{
						SleepPatcher.lastUpdate = DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
					}
				}
			}
		}

		// Token: 0x0200000D RID: 13
		[HarmonyPatch(typeof(Player))]
		[HarmonyPatch("Update")]
		internal class StopSleepPatch
		{
			// Token: 0x0600002D RID: 45 RVA: 0x000035B4 File Offset: 0x000017B4
			public static void Postfix()
			{
				bool flag = Tiredness.isIngame && !uGUI.main.loading.isLoading && !uGUI.main.intro.showing;
				if (flag)
				{
					bool keyDown = Input.GetKeyDown(Main.config.Stopsleepkey);
					bool flag2 = (bool)typeof(DevConsole).GetField("state", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(typeof(DevConsole).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null));
					bool flag3 = !uGUI.main.userInput.focused && !flag2;
					if (flag3)
					{
						bool flag4 = keyDown && SleepPatcher.isSleeping;
						if (flag4)
						{
							DayNightCycle.main.StopSkipTimeMode();
							SleepPatcher.buttonReleasedSinceSkipping = false;
						}
						else
						{
							bool flag5 = keyDown;
							if (flag5)
							{
								bool buttonReleasedSinceSkipping = SleepPatcher.buttonReleasedSinceSkipping;
								if (buttonReleasedSinceSkipping)
								{
									bool isSkippingTime = SleepPatcher.isSkippingTime;
									if (isSkippingTime)
									{
										Player.main.cinematicModeActive = false;
										SleepPatcher.isSkippingTime = false;
										DayNightCycle.main.StopSkipTimeMode();
									}
									else
									{
										bool flag6 = !Player.main.cinematicModeActive;
										if (flag6)
										{
											Player.main.cinematicModeActive = true;
											SleepPatcher.isSkippingTime = true;
											DayNightCycle.main.SkipTime(1000f, 20f);
										}
									}
								}
								SleepPatcher.buttonReleasedSinceSkipping = false;
							}
							else
							{
								SleepPatcher.buttonReleasedSinceSkipping = true;
							}
						}
					}
					else
					{
						SleepPatcher.buttonReleasedSinceSkipping = true;
					}
					bool flag7 = SleepPatcher.isSkippingTime && !DayNightCycle.main.IsInSkipTimeMode();
					if (flag7)
					{
						Player.main.cinematicModeActive = false;
						SleepPatcher.isSkippingTime = false;
					}
				}
				else
				{
					SleepPatcher.isSkippingTime = false;
				}
			}
		}
	}
}
