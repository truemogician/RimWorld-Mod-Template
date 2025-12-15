using UnityEngine;
using Verse;

namespace RimWorldModTemplate;

public class Mod : Verse.Mod {
	public Mod(ModContentPack content) : base(content) {
		Settings.Default = GetSettings<Settings>();
    }

	public override string SettingsCategory() => ThisAssembly.Info.Title;

	public override void DoSettingsWindowContents(Rect inRect) {
		var listing = new Listing_Standard();
		listing.Begin(inRect);

		listing.Label($"Market Value Multiplier: {Settings.Default.Multiplier:F2}");
		Settings.Default.Multiplier = listing.Slider(Settings.Default.Multiplier, 0.1f, 5.0f);
		Settings.Default.Apply();

        listing.End();
	}
}