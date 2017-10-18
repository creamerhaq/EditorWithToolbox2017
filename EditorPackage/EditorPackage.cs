using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.Windows.Controls;

namespace EditorWithToolbox2017.EditorPackage
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
	/// </para>
	/// </remarks>

	// // We register our AddNewItem Templates the Miscellaneous Files Project:
	//[ProvideEditorExtension(typeof(EditorFactory), ".kaymakci", 32,
	//          ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
	//          TemplateDir = @"..\..\Templates",
	//          NameResourceID = 106)]
	//// We register that our editor supports LOGVIEWID_Designer logical view
	//[ProvideEditorLogicalView(typeof(EditorFactory), "{7651a703-06e5-11d1-8ebd-00a0c90f26ea}")]
	[PackageRegistration(UseManagedResourcesOnly = true)]
	//[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[InstalledProductRegistration("#1110", "#1112", "1.0", IconResourceID = 1400)] // Info on this package for Help/About
	[ProvideEditorFactory(typeof(EditorFactory), 110)]
	[ProvideAutoLoad(UIContextGuids.SolutionExists)]
	//[ProvideOptionPage(typeof(TextBox), "ToDo", "General", 101, 106, true)]
	[Guid(EditorPackage.PackageGuidString)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	public sealed class EditorPackage : Package, IDisposable
	{
		#region Fields

		private EditorFactory editorFactory;

		#endregion Fields
		/// <summary>
		/// EditorPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "2691aae9-7fad-4ce4-80b0-4605358a969d";

		/// <summary>
		/// Initializes a new instance of the <see cref="EditorPackage"/> class.
		/// </summary>
		public EditorPackage()
		{
			// Inside this method you can place any initialization code that does not require
			// any Visual Studio service because at this point the package object is created but
			// not sited yet inside Visual Studio environment. The place to do all the other
			// initialization is the Initialize method.
		}

		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", typeof(EditorPackage).ToString()));

			//Create Editor Factory
			base.Initialize();
			editorFactory = new EditorFactory();
			base.RegisterEditorFactory(editorFactory);
		}

		#region IDisposable Support
		private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

		void Dispose(bool disposing)
		{
			try
			{
				Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Dispose() of: {0}", this.ToString()));
				if (disposing)
				{
					if (editorFactory != null)
					{
						editorFactory.Dispose();
						editorFactory = null;
					}
					GC.SuppressFinalize(this);
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
		// ~EditorPackage() {
		//   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
		//   Dispose(false);
		// }

		// Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
		public void Dispose()
		{
			// Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
			Dispose(true);
			// TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
			// GC.SuppressFinalize(this);
		}
		#endregion

		#endregion
	}
}
