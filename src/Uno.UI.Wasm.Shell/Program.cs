using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.Foundation;
using Uno.Logging;
using Windows.UI.Popups;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;
using Uno.Roslyn;
using Uno.UI.SourceGenerators.XamlGenerator;

namespace Uno.UI.Wasm.Shell
{
	class Program
	{
		private static App.App _app;

		static void Main(string[] args)
		{
			ParseXaml();

			ConfigureFilters(LogExtensionPoint.AmbientLoggerFactory);

			Application.Start(_ => new App.App());
		}

		static void ConfigureFilters(ILoggerFactory factory)
		{
			factory
				.WithFilter(new FilterLoggerSettings
					{
						{"Uno", LogLevel.Warning},
						{"Windows", LogLevel.Warning},
						{"SampleControl.Presentation", LogLevel.Debug},

						// Generic Xaml events
						// { "Windows.UI.Xaml", LogLevel.Debug },

						// { "Uno.UI.Controls.AsyncValuePresenter", LogLevel.Debug },
						// { "Uno.UI.Controls.IfDataContext", LogLevel.Debug },

						// Layouter specific messages
						// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
						//{ "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
						//{ "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },

						// Binding related messages
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },

						//  Binder memory references tracking
						// { "ReferenceHolder", LogLevel.Debug },
					}
				)
				.AddConsole(LogLevel.Debug);
		}

		private static void ParseXaml()
		{
			var compilation = Compile();

			var fileName = "file.xaml";
			File.WriteAllText(fileName, Xaml);

			var file = new XamlFileDefinition(fileName);
			var metadataHelper =
				new RoslynMetadataHelper("Debug", compilation, null, null);

			var generator = new XamlFileGenerator(
				file: file,
				targetPath: "compiled",
				defaultNamespace: "MyApp",
				medataHelper: metadataHelper,
				fileUniqueId: file.UniqueID,
				lastReferenceUpdateTime: DateTime.MaxValue,
				analyzerSuppressions: new string[0],
				globalStaticResourcesMap: new XamlGlobalStaticResourcesMap(),
				outputSourceComments: true,
				resourceKeys: new string[0],
				isUiAutomationMappingEnabled: false,
				uiAutomationMappings: null,
				isWasm: true);

			generator.GenerateFile();
		}

		private static Compilation Compile()
		{
			var codeSyntax = SyntaxFactory.ParseCompilationUnit(Csharp);


			var language = new CSharpLanguage();

			var compilation = language
				.CreateLibraryCompilation(assemblyName: "myAssembly", enableOptimisations: false)
				.AddSyntaxTrees(codeSyntax.SyntaxTree);

			return compilation;
		}

		private static string Csharp = @"
namespace MyApp
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
		}
	}
}";

		private static string Xaml = @"<Page x:Class=""MyApp.MainPage""
	xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
	xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
	xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
	xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
	mc:Ignorable=""d""><TextBlock>ItWorks!</TextBlock></Page>";

		public class CSharpLanguage : ILanguageService
		{
			private readonly IReadOnlyCollection<MetadataReference> _references = new[]
			{
				CorlibReference,
				SystemCoreReference,
				ConsoleReference,
				DecimalReference,
			};
		

		public Compilation CreateLibraryCompilation(string assemblyName, bool enableOptimisations)
		{
			var options = new CSharpCompilationOptions(
						OutputKind.DynamicallyLinkedLibrary,
						optimizationLevel: OptimizationLevel.Release,
						allowUnsafe: true)
					// Disabling concurrent builds allows for the emit to finish.
					.WithConcurrentBuild(false)
				;

			Console.WriteLine($"References: {string.Join(", ", _references.Select(r => r.Display))}");

			return CSharpCompilation.Create(assemblyName, options: options, references: _references);
		}
	}

	private static readonly MetadataReference CorlibReference =
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

		private static readonly MetadataReference ConsoleReference =
			MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location);

		private static readonly MetadataReference DecimalReference =
			MetadataReference.CreateFromFile(typeof(System.Decimal).Assembly.Location);

		private static readonly MetadataReference SystemCoreReference =
			MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);

		private static readonly MetadataReference CodeAnalysisReference =
			MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);
	}
}
