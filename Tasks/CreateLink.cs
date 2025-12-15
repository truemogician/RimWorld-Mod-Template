using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class CreateLink : Task {
	[Required]
	public string Source { get; set; }

	[Required]
	public string Destination { get; set; }

	public override bool Execute() {
		if (!Directory.Exists(Source)) {
			Log.LogError($"[Symlink] Source directory not found: {Source}");
			return false;
		}
		if (Directory.Exists(Destination)) {
			// Check if it's already a Symlink/Junction
			var attr = File.GetAttributes(Destination);
			if ((attr & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint) {
				Log.LogMessage(MessageImportance.High, $"[Symlink] Link already exists at: {Destination}");
				return true;
			}
			// It's a real folder
			Log.LogWarning($"[Symlink] A real folder already exists at {Destination}. Please delete it manually if you want to switch to Symlinks.");
			return true;
		}
		Log.LogMessage(MessageImportance.High, $"[Symlink] Linking '{Source}' -> '{Destination}'");
		try {
			var process = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = "cmd.exe",
					// Junction doesn't require admin rights on local drives
					Arguments = $"/c mklink /J \"{Destination}\" \"{Source}\"",
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				}
			};
			process.Start();
			process.WaitForExit();
			if (process.ExitCode == 0) {
				Log.LogMessage(MessageImportance.High, "[Symlink] Successfully created mod link.");
				return true;
			}
			string error = process.StandardError.ReadToEnd();
			Log.LogWarning($"[Symlink] Failed to create link. Error code: {process.ExitCode}. Details: {error}");
			return true; // Don't break build, just warn
		}
		catch (Exception ex) {
			Log.LogWarning($"[Symlink] Exception while creating link: {ex.Message}");
			return true;
		}
	}
}