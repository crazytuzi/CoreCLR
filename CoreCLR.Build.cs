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

		const int MajorVersion = 10;

		const int MinorVersion = 0;

		const int PatchVersion = 4;

		PublicDefinitions.AddRange(new string[]
		{
			$"DOTNET_MAJOR_VERSION={MajorVersion}",
			$"DOTNET_MINOR_VERSION={MinorVersion}",
			$"DOTNET_PATCH_VERSION={PatchVersion}"
		});

		var bUseRelease = true;

		var bIsDebug = !bUseRelease &&
		               (Target.Configuration == UnrealTargetConfiguration.Debug ||
		                Target.Configuration == UnrealTargetConfiguration.DebugGame);

		var CoreCLRConfiguration = bIsDebug ? "Debug" : "Release";

		PublicDefinitions.Add($"CORECLR_CONFIGURATION=TEXT(\"{CoreCLRConfiguration}\")");

		PublicIncludePaths.Add(Path.Combine(ModuleDirectory, "src"));

		var LibraryPath = Path.Combine(ModuleDirectory, "lib", CoreCLRConfiguration);

		var SharedPath = $"shared/Microsoft.NETCore.App/{MajorVersion}.{MinorVersion}.{PatchVersion}";

		var BinaryOutputDirectory = Target.bBuildEditor
			? Path.Combine(PluginDirectory, "Binaries", Target.Platform.ToString())
			: "$(BinaryOutputDir)";

		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath, Target.Platform.ToString());

			RuntimeDependencies.Add($"{BinaryOutputDirectory}/hostfxr.dll",
				Path.Combine(PlatformLibraryPath, "hostfxr.dll"));

			RuntimeDependencies.Add($"{BinaryOutputDirectory}/CoreCLR.runtimeconfig.json",
				Path.Combine(PlatformLibraryPath, "CoreCLR.runtimeconfig.json"));

			RuntimeDependencies.Add($"{BinaryOutputDirectory}/mscordaccore_amd64_amd64_42.42.42.42424.dll",
				Path.Combine(PlatformLibraryPath, "mscordaccore_amd64_amd64_42.42.42.42424.dll"));

			var SharedDirectory = $"{BinaryOutputDirectory}/{SharedPath}";

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/{Path.GetFileName(File)}", File);
			}

			var DynamicLinkLibraries = new[]
			{
				"coreclr.dll",
				"hostpolicy.dll",
				"clrgc.dll",
				"clrgcexp.dll",
				"clrjit.dll",
				"clretwrc.dll",
				"mscordaccore.dll",
				"mscordbi.dll",
				"mscorrc.dll",
				"Microsoft.DiaSymReader.Native.amd64.dll",
				"System.IO.Compression.Native.dll",
				"msquic.dll"
			};

			foreach (var DynamicLinkLibrary in DynamicLinkLibraries)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/{DynamicLinkLibrary}",
					Path.Combine(PlatformLibraryPath, DynamicLinkLibrary));
			}

			if (bIsDebug)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/clrinterpreter.dll",
					Path.Combine(PlatformLibraryPath, "clrinterpreter.dll"));
			}
		}
		else if (Target.Platform == UnrealTargetPlatform.Linux || Target.Platform == UnrealTargetPlatform.LinuxArm64)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath,
				Target.Platform == UnrealTargetPlatform.Linux ? "Linux_x86_64" : "Linux_aarch64");

			RuntimeDependencies.Add($"{BinaryOutputDirectory}/libhostfxr.so",
				Path.Combine(PlatformLibraryPath, "libhostfxr.so"));

			RuntimeDependencies.Add($"{BinaryOutputDirectory}/CoreCLR.runtimeconfig.json",
				Path.Combine(PlatformLibraryPath, "CoreCLR.runtimeconfig.json"));

			var SharedDirectory = $"{BinaryOutputDirectory}/{SharedPath}";

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/{Path.GetFileName(File)}", File);
			}

			var SharedObjects = new[]
			{
				"libcoreclr.so",
				"libhostpolicy.so",
				"libclrgc.so",
				"libclrgcexp.so",
				"libclrjit.so",
				"libcoreclrtraceptprovider.so",
				"libmscordaccore.so",
				"libmscordbi.so",
				"libSystem.Globalization.Native.so",
				"libSystem.IO.Compression.Native.so",
				"libSystem.Native.so",
				"libSystem.Net.Security.Native.so",
				"libSystem.Security.Cryptography.Native.OpenSsl.so",
				"libicuuc.so.76.1",
				"libicui18n.so.76.1",
				"libicudata.so.76.1"
			};

			foreach (var SharedObject in SharedObjects)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/{SharedObject}",
					Path.Combine(PlatformLibraryPath, SharedObject));
			}

			if (bIsDebug)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/libclrinterpreter.so",
					Path.Combine(PlatformLibraryPath, "libclrinterpreter.so"));
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

			RuntimeDependencies.Add($"{BinaryOutputDirectory}/libhostfxr.dylib",
				Path.Combine(PlatformLibraryPath, "libhostfxr.dylib"));

			RuntimeDependencies.Add($"{BinaryOutputDirectory}/CoreCLR.runtimeconfig.json",
				Path.Combine(PlatformLibraryPath, "CoreCLR.runtimeconfig.json"));

			var SharedDirectory = $"{BinaryOutputDirectory}/{SharedPath}";

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/{Path.GetFileName(File)}", File);
			}

			var DynamicLibraries = new[]
			{
				"libcoreclr.dylib",
				"libhostpolicy.dylib",
				"libclrgc.dylib",
				"libclrgcexp.dylib",
				"libclrjit.dylib",
				"libmscordaccore.dylib",
				"libmscordbi.dylib",
				"libSystem.Globalization.Native.dylib",
				"libSystem.IO.Compression.Native.dylib",
				"libSystem.Native.dylib",
				"libSystem.Net.Security.Native.dylib",
				"libSystem.Security.Cryptography.Native.Apple.dylib"
			};

			foreach (var DynamicLibrary in DynamicLibraries)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/{DynamicLibrary}",
					Path.Combine(PlatformLibraryPath, DynamicLibrary));
			}

			if (bIsDebug)
			{
				RuntimeDependencies.Add($"{SharedDirectory}/libclrinterpreter.dylib",
					Path.Combine(PlatformLibraryPath, "libclrinterpreter.dylib"));
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