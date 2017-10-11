using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.OLE.Interop;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace EditorWithToolbox2017.EditorPackage
{
    //[Guid(GuidStrings.GuidEditorFactory)]
    [ProvideView(LogicalView.Designer, null)]
    [ProvideEditorExtension(typeof(EditorFactory), EditorFactory.Extension, 60)]
    public class EditorFactory : IVsEditorFactory, IDisposable
    {
        public const string Extension = ".sakir";

        #region Fields
        // private instance of the EditorFactory's OleServiceProvider
        private ServiceProvider vsServiceProvider;
        #endregion

        #region Constructors
        /// <summary>
        /// Explicitly defined default constructor.
        /// Initialize new instance of the EditorFactory object.
        /// </summary>
        public EditorFactory()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering {0} constructor", typeof(EditorFactory).ToString()));
        }

        #endregion Constructors

       // [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public int CreateEditorInstance(uint grfCreateDoc, string pszMkDocument, string pszPhysicalView, IVsHierarchy pvHier, uint itemid, IntPtr punkDocDataExisting, out IntPtr ppunkDocView, out IntPtr ppunkDocData, out string pbstrEditorCaption, out Guid pguidCmdUI, out int pgrfCDW)
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering {0} CreateEditorInstance()", this.ToString()));

            // Initialize to null
            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            pguidCmdUI = new Guid(EditorPackage.PackageGuidString);
            pgrfCDW = 0;
            pbstrEditorCaption = null;

            // Validate inputs
            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0)
            {
                return VSConstants.E_INVALIDARG;
            }
            if (punkDocDataExisting != IntPtr.Zero)
            {
                return VSConstants.VS_E_INCOMPATIBLEDOCDATA;
            }

            // Create the Document (editor)
            EditorPane newEditor = new EditorPane();
            ppunkDocView = Marshal.GetIUnknownForObject(newEditor);
            ppunkDocData = Marshal.GetIUnknownForObject(newEditor);
            pbstrEditorCaption = "";

            return VSConstants.S_OK;
        }

        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            vsServiceProvider = new ServiceProvider(psp);
            return VSConstants.S_OK;
        }

        public int Close()
        {
            return VSConstants.S_OK;
        }

        public int MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
        {
            pbstrPhysicalView = null;   // initialize out parameter

            // we support only a single physical view
            if (VSConstants.LOGVIEWID_Primary == rguidLogicalView)
            {
                // primary view uses NULL as pbstrPhysicalView
                return VSConstants.S_OK;
            }
            else
            {
                // you must return E_NOTIMPL for any unrecognized rguidLogicalView values
                return VSConstants.E_NOTIMPL;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }

                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.

                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~EditorFactory() {
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
    }
}
