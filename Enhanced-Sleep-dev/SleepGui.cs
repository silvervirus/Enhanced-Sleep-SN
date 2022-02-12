using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
    class SleepGui : MonoBehaviour
    {
        public static SleepGui _Instance = null;
        private static string currentTiredness = "";
        private static float currentTirednessV = 0;

        private static string wentBedTime = "";
        private static string wentBedDay = "";
        private static string cTime = "";
        private static string cDay = "";
        private static string CurrTime = "";
        private static string CurrDay = "";
        public static bool isShow = false;

        public void Awake()
        {
            SleepGui._Instance = this;
        }

        public void Update()
        {
            if (Player.main != null)
            {
                int __h = (int) Math.Floor(Mathf.Repeat(SleepPatcher.wentToSleep / (1f / 24), 24f));
                int __m = (int) Math.Floor(Mathf.Repeat((SleepPatcher.wentToSleep / (1f / 24)) * 60f, 60f));
                int __d = (int) Math.Floor(SleepPatcher.wentToSleepD);
                wentBedTime = __h.ToString("00") + ":" + __m.ToString("00");
                wentBedDay = "Day " + __d;
                float __cTime = (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
                int __cH = (int)Math.Floor(Mathf.Repeat(__cTime * 24f, 24f));
                int __cM = (int)Math.Floor(Mathf.Repeat(__cTime * 24f * 60f, 60f));
                int __cD = (int)Math.Floor(__cTime);
                cTime = __cH.ToString("00") + ":" + __cM.ToString("00");
                cDay = "Day " + __cD;
            }
        }

        public void OnGUI()
        {
            if (!SleepGui.isShow)
            {
                //return;
            }

            if (Tiredness.isIngame && !uGUI.main.loading.IsLoading && !uGUI.main.intro.showing)
            {
                if (SleepPatcher.isSleeping)
                {
                    float __centerX = Screen.width / 2;
                    float __centerY = Screen.height / 2;

                    float __distToX = 80;
                    float __distToY = 20;

                    GUIStyle __styleD = new GUIStyle();
                    __styleD.normal.textColor = Color.white;
                    __styleD.fontSize = 64;
                    __styleD.alignment = TextAnchor.LowerCenter;
                    GUIStyle __styleT = new GUIStyle();
                    __styleT.normal.textColor = Color.white;
                    __styleT.fontSize = 32;
                    __styleT.alignment = TextAnchor.UpperCenter;
                    GUIStyle __styleInfo = new GUIStyle();
                    __styleInfo.normal.textColor = Color.white;
                    __styleInfo.fontSize = 16;
                    __styleInfo.alignment = TextAnchor.LowerRight;
                    GUIStyle __styleFood = new GUIStyle();
                    __styleFood.normal.textColor = new Color(255, 125, 0, 255);
                    __styleFood.fontSize = 16;
                    __styleFood.alignment = TextAnchor.MiddleCenter;
                    GUIStyle __styleWater = new GUIStyle();
                    __styleWater.normal.textColor = new Color(0, 125, 255, 255);
                    __styleWater.fontSize = 16;
                    __styleWater.alignment = TextAnchor.MiddleCenter;
                    GUIStyle __styleHealth = new GUIStyle();
                    __styleHealth.normal.textColor = new Color(255, 125, 125, 255);
                    __styleHealth.fontSize = 16;
                    __styleHealth.alignment = TextAnchor.MiddleCenter;


                    float __WBX = __centerX - __distToX - 200;
                    float __WBY = __centerY - 40 - 20;
                    GUI.Label(new Rect(__WBX, __WBY, 200, 40), wentBedDay, __styleD);
                    GUI.Label(new Rect(__WBX, __WBY + 40 + 10, 200, 40), wentBedTime, __styleT);

                    float __SUX = __centerX + __distToX;
                    float __SUY = __centerY - 40 - 20;
                    GUI.Label(new Rect(__SUX, __SUY, 200, 40), cDay, __styleD);
                    GUI.Label(new Rect(__SUX, __SUY + 40 + 10, 200, 40), cTime, __styleT);
                    Survival sv = Player.main.GetComponent<Survival>();
                    GUI.Label(new Rect(0, __SUY + 80 + 10, Screen.width, 40), Player.main.liveMixin.health.ToString(), __styleHealth);
                    GUI.Label(new Rect(0, __SUY + 120 + 10, Screen.width, 40), sv.food.ToString(), __styleFood);
                    GUI.Label(new Rect(0, __SUY + 160 + 10, Screen.width, 40), sv.water.ToString(), __styleWater);

                    float infostartX = Screen.width - 100;
                    float infostartY = Screen.height - 30;
                    
                    GUI.Label(new Rect(infostartX, infostartY, 90, 20), "Press" +" "+ Main.config.Stopsleepkey +" "+"to wake up.", __styleInfo);
                }

            }
        }

        public static void Load()
        {
            GameObject gameObject = new GameObject("SleepGUI").AddComponent<SleepGui>().gameObject;
        }
    }
}
