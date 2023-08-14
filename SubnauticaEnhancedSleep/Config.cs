using System;
using Nautilus.Json;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
	// Token: 0x02000002 RID: 2
	[Menu("Enhanced Sleep")]
	public class Config : ConfigFile
	{
		// Token: 0x04000001 RID: 1
		[Slider("100% Tiredness Damage", 1f, 100f, DefaultValue = 2f, Format = "{0:R0}")]
		public float FullSleepdamage = 2f;

		// Token: 0x04000002 RID: 2
		[Slider("Coffee Remove Tiredness Amount", 1f, 100f, DefaultValue = 2f, Format = "{0:R0}")]
		public float CoffeeTiredness = 5f;

		// Token: 0x04000003 RID: 3
		[Slider("Bench Remove Tiredness Amount", 1f, 100f, DefaultValue = 1f, Format = "{0:R0}")]
		public float BenchTiredness = 3f;

		// Token: 0x04000004 RID: 4
		[Slider("Time Before Gaining Tiredness", 1f, 200f, DefaultValue = 12f, Format = "{0:R0}")]
		public float timetosleep = 12f;

		// Token: 0x04000005 RID: 5
		[Slider("Time Passed Minutes", 1f, 200f, DefaultValue = 60f, Format = "{0:R0}")]
		public float timePassedMinutes = 60f;

		// Token: 0x04000006 RID: 6
		[Slider("Time Passed Hours", 1f, 200f, DefaultValue = 24f, Format = "{0:R0}")]
		public float timePassedHours = 24f;

		// Token: 0x04000007 RID: 7
		[Slider("SleeprecoverFactor", 1f, 200f, DefaultValue = 1f, Format = "{0:R0}")]
		public float sleeprecoverFactor = 5f;

		// Token: 0x04000008 RID: 8
		[Slider("SleeploseFactor", 0.1f, 200f, DefaultValue = 0.5f, Format = "{0:R0}")]
		public float sleeplooseFactor = 0.5f;

		// Token: 0x04000009 RID: 9
		[Toggle("Tiredness GUI")]
		public bool Isshowing = true;

		// Token: 0x0400000A RID: 10
		public float minusfromwidth = 110f;

		// Token: 0x0400000B RID: 11
		public float minusfromheight = 1070f;

		// Token: 0x0400000C RID: 12
		[Keybind("Attach to Target Toggle Key")]
		public KeyCode Stopsleepkey = KeyCode.X;
	}
}
