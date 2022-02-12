using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
	[Menu("Enhanced Sleep")]
	public  class Config : ConfigFile
	{

		[Slider("100% Tiredness Damage", 1f, 100f, DefaultValue = 2F, Format = "{0:R0}")]
		public float FullSleepdamage = 2f;
		[Slider("Coffee Remove Tiredness Amount", 1f, 100f, DefaultValue = 2F, Format = "{0:R0}")]
		public float CoffeeTiredness = 5f;
		[Slider("Bench Remove Tiredness Amount", 1f, 100f, DefaultValue = 1F, Format = "{0:R0}")]
		public float BenchTiredness = 3f;
		[Slider("Time Before Gaining Tiredness", 1f, 200f, DefaultValue = 12F, Format = "{0:R0}")]
		public float timetosleep = 12f;
		[Slider("Time Passed Minutes", 1f, 200f, DefaultValue = 60F, Format = "{0:R0}")]
		public float timePassedMinutes = 60f;
		[Slider("Time Passed Hours", 1f, 200f, DefaultValue = 24F, Format = "{0:R0}")]
		public float timePassedHours = 24f;
		[Slider("SleeprecoverFactor", 1f, 200f, DefaultValue = 1F, Format = "{0:R0}")]
		public float sleeprecoverFactor = 5f;
		[Slider("SleeploseFactor", 0.1f, 200f, DefaultValue = 0.5F, Format = "{0:R0}")]
		public float sleeplooseFactor = 0.5f;
		[Toggle("Tiredness GUI")]
		public bool Isshowing = true;
		public float minusfromwidth = 110;
		public float minusfromheight = 1070;
		[Keybind("Attach to Target Toggle Key")]
		public KeyCode Stopsleepkey = KeyCode.X;
	}
}

