using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Subnautica_Enhanced_Sleep
{
    class SleepPatcher
    {
        public static FMODAsset FMA_nosleep_tooThirsty = new FMODAsset();


        public static float wentToSleep = 0;
        public static float wentToSleepD = 0;
        public static float lastUpdate = 0;
        public static bool isSleeping = false;
        public static bool isSkippingTime = false;
        public static bool buttonReleasedSinceSkipping = false;

        public static readonly bool useCustomSleepTime = true;
        public static readonly float customSleepTime = 500f;
        public static readonly float customSleepRTime = 20f;
        

        public static void invokeAssets()
        {

        }

        [HarmonyPatch(typeof(Bed))]
        [HarmonyPatch("EnterInUseMode")]
        private class EnterPatch
        {
            public static bool Prefix(Bed __instance)
            {
                if (useCustomSleepTime)
                {
                    /*
                    if (typeof(Bed).GetField("inUseMode", BindingFlags.NonPublic | BindingFlags.Instance)
                            .GetValue(__instance) != typeof(Bed).GetNestedType("InUseMode", BindingFlags.NonPublic)
                            .GetField("None").GetValue(__instance))
                        return false;
                    Player.main.GetComponent<Survival>().freezeStats = true;
                    if (GameOptions.GetVrAnimationMode())
                    {
                        object controller = typeof(Bed).GetField("currentStandUpCinematicController",
                                BindingFlags.NonPublic | BindingFlags.Instance)
                            .GetValue(__instance);
                            SafeAnimator.SetBool(__instance.animator, ((PlayerCinematicController)controller).animParam, true);
                            SafeAnimator.SetBool(__instance.animator, ((PlayerCinematicController)controller).animParam, false);
                            SafeAnimator.SetBool(Player.main.playerAnimator, __instance.cinematicController.playerViewAnimationName, true);
                            SafeAnimator.SetBool(Player.main.playerAnimator, ((PlayerCinematicController)controller).playerViewAnimationName, false);
                    }
                    Player.main.cinematicModeActive = true;
                    MainCameraControl.main.viewModel.localRotation = Quaternion.identity;
                    FieldInfo inUseField = typeof(Bed).GetField("inUseMode", BindingFlags.NonPublic | BindingFlags.Instance);
                    object inUseSleeping = typeof(Bed).GetType().GetNestedType("InUseMode", BindingFlags.NonPublic).GetField("Sleeping").GetValue(__instance);
                    inUseField.SetValue(__instance, inUseSleeping);

                    DayNightCycle.main.SkipTime(customSleepTime, customSleepRTime);
                    uGUI_PlayerSleep.main.StartSleepScreen();
                    wentToSleep = DayNightCycle.main.GetDayNightCycleTime();
                    wentToSleepD = (float)DayNightCycle.main.GetDay();
                    isSleeping = true;
                    return false;
                    */
                    typeof(Bed).GetField("kSleepGameTimeDuration", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, customSleepTime);
                    typeof(Bed).GetField("kSleepRealTimeDuration", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, customSleepRTime);
                }
                wentToSleep = DayNightCycle.main.GetDayNightCycleTime();
                wentToSleepD = (float) DayNightCycle.main.GetDay();
                isSleeping = true;
                return true;
                
            }
        }

        [HarmonyPatch(typeof(Bed))]
        [HarmonyPatch("OnHandClick")]
        private class HandPatch
        {
            public static bool Prefix(Bed __instance, GUIHand hand)
            {
                if ((hand.player.GetComponent<Survival>().food <= 25 || Player.main.GetComponent<Survival>().water <= 25) && GameModeUtils.IsOptionActive(GameModeOption.Survival))
                {
                    if (hand.player.GetComponent<Survival>().food <= 25 &&
                        hand.player.GetComponent<Survival>().water <= 25)
                    {
                        ErrorMessage.AddWarning("You can't sleep now because you are too hungry and thirsty! Drink and eat something before you go to bed.");
                    } else if (hand.player.GetComponent<Survival>().food <= 25 ||
                               !(hand.player.GetComponent<Survival>().water <= 25))
                    {
                        ErrorMessage.AddWarning("You can't sleep now because you are too hungry! Go eat something before you go to bed.");
                    } else if (!(hand.player.GetComponent<Survival>().food <= 25) ||
                               hand.player.GetComponent<Survival>().water <= 25)
                    {
                        ErrorMessage.AddWarning("You can't sleep now because you are too thirsty! Drink something before you go to bed.");
                    }
                    else
                    {
                        ErrorMessage.AddWarning("You can't sleep now! Try to drink and eat something and try again!");
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Bed))]
        [HarmonyPatch("ExitInUseMode")]
        private class ExitPatch
        {
            public static void Postfix(Bed __instance, Player player)
            {
                isSleeping = false;
                /*
                if (!GameModeUtils.IsOptionActive(GameModeOption.Survival)) return;
                player.liveMixin.health = player.liveMixin.maxHealth;
                player.liveMixin.health = player.liveMixin.maxHealth;
                Survival sv = player.GetComponent<Survival>();
                float stoodUp = DayNightCycle.main.GetDayNightCycleTime();
                float foodBefore = sv.food;
                float waterBefore = sv.water;
                float timeSlept = 0;
                double looseFactor = 0.5;
                if (wentToSleep > 0.5)
                {
                    timeSlept = (((DayNightCycle.main.GetDayNightCycleTime() + 1) - wentToSleep));
                }
                else
                {
                    timeSlept = (((DayNightCycle.main.GetDayNightCycleTime() + 0) - wentToSleep));
                }

                double hourTimeSlept = timeSlept / (1d / 24);
                double minuteTimeSlept = hourTimeSlept * 60;

                
                double foodLost = minuteTimeSlept / (25.20 / looseFactor); // factor * looseFactor : looseFactor would be 2 to half the time.
                double waterLost = minuteTimeSlept / (18.00 / looseFactor);
                float foodAfter = foodBefore - (float)foodLost;
                float waterAfter = waterBefore - (float)waterLost;
                if (foodAfter > 5 ) {sv.food = foodAfter;} else {sv.food = 5;}
                if (waterAfter > 5) { sv.water = waterAfter; } else { sv.water = 5; }
                Main.Log("!!Left Bed:\nWent to Bed Time: " + wentToSleep + "\nWoke up: " + stoodUp + "\nDuration: " + timeSlept + "\nDuration in IGHours: " + hourTimeSlept + "\nDuration in IGMinutes: " + minuteTimeSlept + "\nFood Before: " + sv.food + "\nFood Lost: " + foodLost + "\nFood After: " + sv.food + "\nWater Before: " + waterBefore + "\nWater Lost: " + waterLost + "\nWater After: " + sv.water);
                
                */
            }
        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        public class PlayerSleepPatch
        {
            public static void Postfix(Player __instance)
            {
                if (Tiredness.isIngame && !uGUI.main.loading.IsLoading && !uGUI.main.intro.showing)
                {
                    if (isSleeping)
                    {
                        float oldTime = lastUpdate;
                        float newTime = (float)DayNightCycle.main.GetDayNightCycleTime() +
                                        (float)Math.Floor(DayNightCycle.main.GetDay());
                        float timePassed = newTime - oldTime;
                        float timePassedHours = (float)(timePassed / (1d / Main.config.timePassedHours));
                        float timePassedMinutes = (float)(timePassedHours * Main.config.timePassedMinutes);
                        double looseFactor = Main.config.sleeplooseFactor;
                        double recoverFactor = Main.config.sleeprecoverFactor;
                        if (timePassedMinutes >= 1)
                        {
                            if (GameModeUtils.IsOptionActive(GameModeOption.NoSurvival))
                            {
                                float healthAdded = (float)(timePassedMinutes / (4.8 / recoverFactor));
                                float healthOut = (float)(__instance.liveMixin.health + healthAdded);

                                if (healthOut < 0)
                                {
                                    healthOut = 0;
                                }

                                if (healthOut >= __instance.liveMixin.maxHealth)
                                {
                                    healthOut = __instance.liveMixin.maxHealth;
                                }

                                __instance.liveMixin.health = healthOut;
                            }
                            else
                            {
                                //__instance.liveMixin.health = __instance.liveMixin.maxHealth;
                                //__instance.liveMixin.health = __instance.liveMixin.maxHealth;
                                Survival sv = __instance.GetComponent<Survival>();
                                float foodLost = (float)(timePassedMinutes / (25.20 / looseFactor));
                                float foodOut = (float)(sv.food - foodLost);
                                float waterLost = (float)(timePassedMinutes / (18.00 / looseFactor));
                                float waterOut = (float)(sv.water - waterLost);
                                float healthAdded = (float)(timePassedMinutes / (4.8 / recoverFactor));
                                float healthOut = (float)(__instance.liveMixin.health + healthAdded);

                                if (waterOut <= 0)
                                {
                                    waterOut = 0;
                                }

                                if (foodOut <= 0)
                                {
                                    foodOut = 0;
                                }

                                if (healthOut < 0)
                                {
                                    healthOut = 0;
                                }

                                if (healthOut >= __instance.liveMixin.maxHealth)
                                {
                                    healthOut = __instance.liveMixin.maxHealth;
                                }

                                sv.water = waterOut;
                                sv.food = foodOut;
                                __instance.liveMixin.health = healthOut;
                            }
                            lastUpdate = (float)DayNightCycle.main.GetDayNightCycleTime() +
                                         (float)Math.Floor(DayNightCycle.main.GetDay());
                        }
                        
                    }
                    else
                    {
                        lastUpdate = (float)DayNightCycle.main.GetDayNightCycleTime() +
                                     (float)Math.Floor(DayNightCycle.main.GetDay());
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class StopSleepPatch
        {
            public static void Postfix()
            {
                if (Tiredness.isIngame && !uGUI.main.loading.IsLoading && !uGUI.main.intro.showing)
                {
                    bool keyDown = Input.GetKeyDown(Main.config.Stopsleepkey);
                    bool consoleOpened = (bool)typeof(DevConsole).GetField("state",
                        BindingFlags.NonPublic | BindingFlags.Instance).GetValue(typeof(DevConsole).GetField("instance",
                        BindingFlags.NonPublic | BindingFlags.Static).GetValue(null));
                    if (!uGUI.main.userInput.focused && !consoleOpened)
                    {
                        if (keyDown && isSleeping)
                        {
                            DayNightCycle.main.StopSkipTimeMode();
                            buttonReleasedSinceSkipping = false;
                        }
                        else if (keyDown)
                        {
                            if (buttonReleasedSinceSkipping)
                            {
                                if (isSkippingTime)
                                {
                                    Player.main.cinematicModeActive = false;
                                    isSkippingTime = false;
                                    DayNightCycle.main.StopSkipTimeMode();
                                }
                                else
                                {
                                    if (!Player.main.cinematicModeActive)
                                    {
                                        Player.main.cinematicModeActive = true;
                                        isSkippingTime = true;
                                        DayNightCycle.main.SkipTime(1000f, 20f);
                                    }
                                }
                            }

                            buttonReleasedSinceSkipping = false;
                        }
                        else
                        {
                            buttonReleasedSinceSkipping = true;
                        }
                    }
                    else
                    {
                        buttonReleasedSinceSkipping = true;
                    }

                    if (isSkippingTime && !DayNightCycle.main.IsInSkipTimeMode())
                    {
                        Player.main.cinematicModeActive = false;
                        isSkippingTime = false;
                    }
                }
                else
                {
                    isSkippingTime = false;
                }
            }
        }
    }
}