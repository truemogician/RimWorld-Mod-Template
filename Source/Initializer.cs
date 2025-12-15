using HarmonyLib;
using Verse;
using System.Reflection;

namespace RimWorldModTemplate;

[StaticConstructorOnStartup]
public static class Initializer {
	static Initializer() {
		var harmony = new Harmony(ThisAssembly.Project.PackageId);
		harmony.PatchAll(Assembly.GetExecutingAssembly());
		Log.Message($"[{ThisAssembly.Info.Title}] Initialized");
    }
}