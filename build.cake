#load "build/helpers.cake"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// VERSIONING
///////////////////////////////////////////////////////////////////////////////

var packageVersion = string.Empty;
#load "build/version.cake"

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./src/DustMother.sln");
var solution = ParseSolution(solutionPath);
var projects = GetProjects(solutionPath, configuration);
var artifacts = "./dist/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
	packageVersion = BuildVersion(fallbackVersion);
	if (FileExists("./build/.dotnet/dotnet.exe")) {
		Information("Using local install of `dotnet` SDK!");
		Context.Tools.RegisterFile("./build/.dotnet/dotnet.exe");
	}
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	// Clean solution directories.
	foreach(var path in projects.AllProjectPaths)
	{
		Information("Cleaning {0}", path);
		CleanDirectories(path + "/**/bin/" + configuration);
		CleanDirectories(path + "/**/obj/" + configuration);
	}
	foreach (var proj in projects.AllProjects) {
		Information(proj.Type);
	}
	Information("Cleaning common files...");
	CleanDirectory(artifacts);
});

Task("Restore")
	.Does(() =>
{
	// Restore all NuGet packages.
	Information("Restoring solution...");
	foreach (var project in projects.AllProjectPaths) {
		DotNetCoreRestore(project.FullPath);
	}
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
	var settings = new DotNetCoreBuildSettings {
		Configuration = configuration,
		NoIncremental = true,
		ArgumentCustomization = args => args.Append($"/p:Version={packageVersion}")
	};
	DotNetCoreBuild(solutionPath, settings);
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
	CreateDirectory(testResultsPath);
	if (projects.TestProjects.Any()) {

		var settings = new DotNetCoreTestSettings {
			Configuration = configuration
		};

		foreach(var project in projects.TestProjects) {
			DotNetCoreTest(project.Path.FullPath, settings);
		}
	}
});

Task("Publish-Runtime")
	.IsDependentOn("Build")
	.Does(() =>
{
	var projectDir = $"{artifacts}publish";
	CreateDirectory(projectDir);
	foreach (var project in projects.SourceProjects)
	{
		var projPath = project.Path.FullPath;
		DotNetCorePublish(projPath, new DotNetCorePublishSettings {
			OutputDirectory = projectDir + "/dotnet-any",
			Configuration = configuration,
			PublishSingleFile = false,
			PublishTrimmed = false
		});
		var runtimes = new[] { "win-x64"};
		foreach (var runtime in runtimes) {
			var runtimeDir = $"{projectDir}/{runtime}";
			CreateDirectory(runtimeDir);
			Information("Publishing for {0} runtime", runtime);
			var settings = new DotNetCorePublishSettings {
				Runtime = runtime,
				Configuration = configuration,
				OutputDirectory = runtimeDir,
				PublishSingleFile = true,
				PublishTrimmed = true,
				IncludeNativeLibrariesForSelfExtract = true,
				ArgumentCustomization = args => args.Append($"/p:Version={packageVersion}")
			};
			DotNetCorePublish(projPath, settings);
			CreateDirectory($"{artifacts}archive");
			Zip(runtimeDir, $"{artifacts}archive/dm-{runtime}.zip");
		}
	}
});

Task("Default")
	.IsDependentOn("Build");

Task("Publish")
	.IsDependentOn("Publish-Runtime");

RunTarget(target);