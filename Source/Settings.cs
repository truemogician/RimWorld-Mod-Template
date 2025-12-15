using Verse;

namespace RimWorldModTemplate;

public class Settings : ModSettings {
	private float _multiplier = 1.0f;

	public static Settings Default { get; internal set; } = null!;

	public float Multiplier {
		get => _multiplier;
		internal set => _multiplier = value;
    }

	public override void ExposeData() {
		base.ExposeData();
		Scribe_Values.Look(ref _multiplier, "multiplier", 1.0f);
	}

	public void Apply() {
		foreach (var thing in DefDatabase<ThingDef>.AllDefs)
			thing.BaseMarketValue *= _multiplier;
    }
}