using System;
using System.Collections.Generic;
using System.IO;
using EpicGames.Core;
using UnrealBuildTool;

public class CoreCLR : ModuleRules
{
	public CoreCLR(ReadOnlyTargetRules Target) : base(Target)
	{
		Type = ModuleType.External;

		PublicDefinitions.AddRange(new string[]
		{
			"DOTNET_MAJOR_VERSION=10",
			"DOTNET_MINOR_VERSION=0",
			"DOTNET_PATCH_VERSION=4"
		});

		var bIsDebug = Target.Configuration == UnrealTargetConfiguration.Debug ||
		               Target.Configuration == UnrealTargetConfiguration.DebugGame;

		var CoreCLRConfiguration = bIsDebug ? "Debug" : "Release";

		PublicDefinitions.Add($"CORECLR_CONFIGURATION=TEXT(\"{CoreCLRConfiguration}\")");

		PublicIncludePaths.Add(Path.Combine(ModuleDirectory, "src"));

		var LibraryPath = Path.Combine(ModuleDirectory, "lib", CoreCLRConfiguration);

		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath, Target.Platform.ToString());

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/coreclr.dll" : "$(TargetOutputDir)/coreclr.dll",
				Path.Combine(PlatformLibraryPath, "coreclr.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/hostfxr.dll" : "$(TargetOutputDir)/hostfxr.dll",
				Path.Combine(PlatformLibraryPath, "hostfxr.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/hostpolicy.dll" : "$(TargetOutputDir)/hostpolicy.dll",
				Path.Combine(PlatformLibraryPath, "hostpolicy.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/clretwrc.dll" : "$(TargetOutputDir)/clretwrc.dll",
				Path.Combine(PlatformLibraryPath, "clretwrc.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/clrgc.dll" : "$(TargetOutputDir)/clrgc.dll",
				Path.Combine(PlatformLibraryPath, "clrgc.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/clrgcexp.dll" : "$(TargetOutputDir)/clrgcexp.dll",
				Path.Combine(PlatformLibraryPath, "clrgcexp.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/clrjit.dll" : "$(TargetOutputDir)/clrjit.dll",
				Path.Combine(PlatformLibraryPath, "clrjit.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled
					? "$(BinaryOutputDir)/Microsoft.DiaSymReader.Native.amd64.dll"
					: "$(TargetOutputDir)/Microsoft.DiaSymReader.Native.amd64.dll",
				Path.Combine(PlatformLibraryPath, "Microsoft.DiaSymReader.Native.amd64.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled
					? "$(BinaryOutputDir)/mscordaccore.dll"
					: "$(TargetOutputDir)/mscordaccore.dll",
				Path.Combine(PlatformLibraryPath, "mscordaccore.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled
					? "$(BinaryOutputDir)/mscordaccore_amd64_amd64_42.42.42.42424.dll"
					: "$(TargetOutputDir)/mscordaccore_amd64_amd64_42.42.42.42424.dll",
				Path.Combine(PlatformLibraryPath, "mscordaccore_amd64_amd64_42.42.42.42424.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/mscordbi.dll" : "$(TargetOutputDir)/mscordbi.dll",
				Path.Combine(PlatformLibraryPath, "mscordbi.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/mscorrc.dll" : "$(TargetOutputDir)/mscorrc.dll",
				Path.Combine(PlatformLibraryPath, "mscorrc.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled ? "$(BinaryOutputDir)/msquic.dll" : "$(TargetOutputDir)/msquic.dll",
				Path.Combine(PlatformLibraryPath, "msquic.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled
					? "$(BinaryOutputDir)/System.IO.Compression.Native.dll"
					: "$(TargetOutputDir)/System.IO.Compression.Native.dll",
				Path.Combine(PlatformLibraryPath, "System.IO.Compression.Native.dll"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled
					? "$(BinaryOutputDir)/CoreCLR.runtimeconfig.json"
					: "$(TargetOutputDir)/CoreCLR.runtimeconfig.json",
				Path.Combine(PlatformLibraryPath, "CoreCLR.runtimeconfig.json"));

			if (bIsDebug)
			{
				RuntimeDependencies.Add(
					Target.bIsEngineInstalled
						? "$(BinaryOutputDir)/clrinterpreter.dll"
						: "$(TargetOutputDir)/clrinterpreter.dll",
					Path.Combine(PlatformLibraryPath, "clrinterpreter.dll"));
			}

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}
		}
		else if (Target.Platform == UnrealTargetPlatform.Linux || Target.Platform == UnrealTargetPlatform.LinuxArm64)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath,
				Target.Platform == UnrealTargetPlatform.Linux ? "Linux_x86_64" : "Linux_aarch64");

			RuntimeDependencies.Add("$(BinaryOutputDir)/libcoreclr.so",
				Path.Combine(PlatformLibraryPath, "libcoreclr.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libhostfxr.so",
				Path.Combine(PlatformLibraryPath, "libhostfxr.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libhostpolicy.so",
				Path.Combine(PlatformLibraryPath, "libhostpolicy.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libclrgc.so",
				Path.Combine(PlatformLibraryPath, "libclrgc.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libclrgcexp.so",
				Path.Combine(PlatformLibraryPath, "libclrgcexp.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libclrjit.so",
				Path.Combine(PlatformLibraryPath, "libclrjit.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libcoreclrtraceptprovider.so",
				Path.Combine(PlatformLibraryPath, "libcoreclrtraceptprovider.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libmscordaccore.so",
				Path.Combine(PlatformLibraryPath, "libmscordaccore.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libmscordbi.so",
				Path.Combine(PlatformLibraryPath, "libmscordbi.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Globalization.Native.so",
				Path.Combine(PlatformLibraryPath, "libSystem.Globalization.Native.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.IO.Compression.Native.so",
				Path.Combine(PlatformLibraryPath, "libSystem.IO.Compression.Native.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Native.so",
				Path.Combine(PlatformLibraryPath, "libSystem.Native.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Net.Security.Native.so",
				Path.Combine(PlatformLibraryPath, "libSystem.Net.Security.Native.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Security.Cryptography.Native.OpenSsl.so",
				Path.Combine(PlatformLibraryPath, "libSystem.Security.Cryptography.Native.OpenSsl.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/CoreCLR.runtimeconfig.json",
				Path.Combine(PlatformLibraryPath, "CoreCLR.runtimeconfig.json"));

			if (bIsDebug)
			{
				RuntimeDependencies.Add("$(BinaryOutputDir)/libclrinterpreter.so",
					Path.Combine(PlatformLibraryPath, "libclrinterpreter.so"));
			}

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}
		}
		else if (Target.Platform == UnrealTargetPlatform.Mac)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath,
#if UE_5_2_OR_LATER
				Target.Architecture.bIsX64
#else
				Target.Architecture == "x86_64" || Target.Architecture == "x64"
#endif

					? "macOS_x86_64"
					: "macOS_arm64");

			RuntimeDependencies.Add("$(BinaryOutputDir)/libcoreclr.dylib",
				Path.Combine(PlatformLibraryPath, "libcoreclr.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libhostfxr.dylib",
				Path.Combine(PlatformLibraryPath, "libhostfxr.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libhostpolicy.dylib",
				Path.Combine(PlatformLibraryPath, "libhostpolicy.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libclrgc.dylib",
				Path.Combine(PlatformLibraryPath, "libclrgc.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libclrgcexp.dylib",
				Path.Combine(PlatformLibraryPath, "libclrgcexp.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libclrjit.dylib",
				Path.Combine(PlatformLibraryPath, "libclrjit.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libmscordaccore.dylib",
				Path.Combine(PlatformLibraryPath, "libmscordaccore.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libmscordbi.dylib",
				Path.Combine(PlatformLibraryPath, "libmscordbi.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Globalization.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Globalization.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.IO.Compression.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.IO.Compression.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Net.Security.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Net.Security.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Security.Cryptography.Native.Apple.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Security.Cryptography.Native.Apple.dylib"));

			RuntimeDependencies.Add(
				Target.bIsEngineInstalled
					? "$(BinaryOutputDir)/CoreCLR.runtimeconfig.json"
					: "$(TargetOutputDir)/CoreCLR.runtimeconfig.json",
				Path.Combine(PlatformLibraryPath, "CoreCLR.runtimeconfig.json"));

			if (bIsDebug)
			{
				RuntimeDependencies.Add(
					Target.bIsEngineInstalled
						? "$(BinaryOutputDir)/libclrinterpreter.dylib"
						: "$(TargetOutputDir)/libclrinterpreter.dylib",
					Path.Combine(PlatformLibraryPath, "libclrinterpreter.dylib"));
			}

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}
		}

		if (!Target.bBuildEditor)
		{
			CopyInterop();
		}
	}

	private void CopyInterop()
	{
		var PublishDirectory = GetPublishDirectory();

		var InteropFilePath = Path.Combine(
			Target.ProjectFile.Directory.FullName,
			"Content", PublishDirectory, "Interop.dll");

		if (File.Exists(InteropFilePath))
		{
			RuntimeDependencies.Add("$(BinaryOutputDir)/Interop.dll", InteropFilePath);
		}
	}

	private string GetPublishDirectory()
	{
		var SettingFilePath = Path.Combine(Target.ProjectFile.Directory.FullName,
			"Config", "DefaultUnrealCSharpSetting.ini");

		if (File.Exists(SettingFilePath))
		{
			var SettingConfigFile = new ConfigFile(new FileReference(SettingFilePath));

			if (SettingConfigFile.TryGetSection("/Script/UnrealCSharpCore.UnrealCSharpSetting", out var Section))
			{
				var HierarchySection = new ConfigHierarchySection(new List<ConfigFileSection> { Section });

				if (HierarchySection.TryGetValue("PublishDirectory", out var Value))
				{
					const string Prefix = "Path=\"";

					var Index = Value.IndexOf(Prefix, StringComparison.Ordinal);

					if (Index >= 0)
					{
						Index += Prefix.Length;

						var EndIndex = Value.IndexOf('"', Index);

						if (EndIndex > Index)
						{
							return Value.Substring(Index, EndIndex - Index);
						}
					}
				}
			}
		}

		return "Script";
	}

	private static IEnumerable<string> GetFiles(string InDirectory, string InPattern = "*.*")
	{
		var Files = new List<string>();

		foreach (var File in Directory.GetFiles(InDirectory, InPattern))
		{
			Files.Add(File);
		}

		foreach (var File in Directory.GetDirectories(InDirectory))
		{
			Files.AddRange(GetFiles(File, InPattern));
		}

		return Files;
	}
}