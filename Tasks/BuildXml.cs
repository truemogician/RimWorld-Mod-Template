using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class BuildXml : Task {
	[Required]
	public string Template { get; set; }

	[Required]
	public string Destination { get; set; }

	[Required]
	public ITaskItem[] Tokens { get; set; }

	public override bool Execute() {
		if (!File.Exists(Template)) {
			Log.LogError($"[BuildXml] Source template not found: {Template}");
			return false;
		}
		string content = File.ReadAllText(Template);
		foreach (var item in Tokens) {
			// The "Include" name
			string key = item.ItemSpec;
			string value = item.GetMetadata("Value");
			// Perform replacement: {{Key}} -> Value
			string placeholder = "{{" + key + "}}";
			if (content.Contains(placeholder))
				content = content.Replace(placeholder, value);
		}
		// Standard check to prevent unnecessary disk writes (and file locks)
		Directory.CreateDirectory(Path.GetDirectoryName(Destination)!);
		if (!File.Exists(Destination) || File.ReadAllText(Destination) != content) {
			File.WriteAllText(Destination, content);
			Log.LogMessage(MessageImportance.High, $"[BuildXml] Generated {Path.GetFileName(Destination)}");
		}
		return true;
	}
}