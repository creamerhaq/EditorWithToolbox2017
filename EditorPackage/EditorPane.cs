using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EditorWithToolbox2017.Editors;
using EditorWithToolbox2017.Properties;
using EditorWithToolbox2017.ToolboxItems;

namespace EditorWithToolbox2017.EditorPackage
{

	public sealed class EditorPane : WindowPane, IVsPersistDocData, IPersistFileFormat, IVsToolboxUser, IVsToolboxActiveUserHook
	{
		// Full path to the file.
		private FileInfo fileInfo;
		EditorControl editorControl;
		IVsStatusbar statusBar;
		OleDataObject _selectedToolboxData;

		protected override void Initialize()
		{
			editorControl = new Editors.EditorControl(this);
			this.editorControl.AllowDrop = true;

			#region Toolbox Items

			CreateToolboxData("Data 001", "0x01", "sakir File Toolbox", true);
			CreateToolboxData("Data 002", "0x02", "sakir File Toolbox");
			CreateToolboxData("Data 003", "0x03", "sakir File Toolbox");
			CreateToolboxData("Data 004", "0x04", "sakir File Toolbox");
			#endregion



			Content = editorControl;
			//base.Initialize();
		}

		#region IVsPersistDocData
		public int GetGuidEditorType(out Guid pClassID)
		{
			return ((IPersistFileFormat)this).GetClassID(out pClassID);
		}

		public int IsDocDataDirty(out int pfDirty)
		{
			return ((IPersistFileFormat)this).IsDirty(out pfDirty);
		}

		public int SetUntitledDocPath(string pszDocDataPath)
		{
			return ((IPersistFileFormat)this).InitNew(VSConstants.S_OK);
		}

		public int LoadDocData(string pszMkDocument)
		{
			return ((IPersistFileFormat)this).Load(pszMkDocument, 0, 0);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
		public int SaveDocData(VSSAVEFLAGS dwSave, out string pbstrMkDocumentNew, out int pfSaveCanceled)
		{
			pbstrMkDocumentNew = null;
			pfSaveCanceled = 0;

			switch (dwSave)
			{
				case VSSAVEFLAGS.VSSAVE_Save:
					editorControl.IsDirty = false;
					break;
				case VSSAVEFLAGS.VSSAVE_SaveAs:
					break;
				case VSSAVEFLAGS.VSSAVE_SilentSave:
					break;
				case VSSAVEFLAGS.VSSAVE_SaveCopyAs:
					break;
				default:
					break;
			}

			return VSConstants.S_OK;
		}

		public int Close()
		{
			return VSConstants.S_OK;
		}

		public int OnRegisterDocData(uint docCookie, IVsHierarchy pHierNew, uint itemidNew)
		{
			return VSConstants.S_OK;
		}

		public int RenameDocData(uint grfAttribs, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
		{
			return VSConstants.S_OK;
		}

		public int IsDocDataReloadable(out int pfReloadable)
		{
			// Allow file to be reloaded
			pfReloadable = 1;
			return VSConstants.S_OK;
		}

		public int ReloadDocData(uint grfFlags)
		{
			return ((IPersistFileFormat)this).Load(null, grfFlags, 0);
		}
		#endregion

		#region IPersistFileFormat
		public int GetClassID(out Guid pClassID)
		{
			pClassID = new Guid(EditorPackage.PackageGuidString);
			return VSConstants.S_OK;
		}



		public int IsDirty(out int pfIsDirty)
		{

			pfIsDirty = (editorControl.IsDirty) ? 1 : 0;
			return VSConstants.S_OK;
		}

		public int InitNew(uint nFormatIndex)
		{
			if (nFormatIndex != 0)
			{
				throw new ArgumentException("Unknown format");
			}
			// until someone change the file, we can consider it not dirty as
			// the user would be annoyed if we prompt him to save an empty file
			//isDirty = false;
			return VSConstants.S_OK;
		}

		public int Load(string pszFilename, uint grfMode, int fReadOnly)
		{
			fileInfo = new FileInfo(pszFilename);

			return VSConstants.S_OK;
		}

		public int Save(string pszFilename, int fRemember, uint nFormatIndex)
		{
			return VSConstants.S_OK;
		}

		public int SaveCompleted(string pszFilename)
		{
			return VSConstants.S_OK;
		}

		public int GetCurFile(out string ppszFilename, out uint pnFormatIndex)
		{
			// We only support 1 format so return its index
			pnFormatIndex = 0;
			ppszFilename = fileInfo.FullName;
			return VSConstants.S_OK;
		}

		public int GetFormatList(out string ppszFormatList)
		{
			ppszFormatList = "Editor Format List -";
			return VSConstants.S_OK;
		}

		#endregion

		#region IVsToolboxUser
		public int IsSupported(IDataObject pDO)
		{
			// Create a OleDataObject from the input interface.
			OleDataObject oleData = new OleDataObject(pDO);

			// Check if the data object is of type MyToolboxData.
			if (oleData.GetDataPresent(typeof(ToolboxData)))
				return VSConstants.S_OK;

			// In all the other cases return S_FALSE
			return VSConstants.S_FALSE;
		}

		public int ItemPicked(IDataObject pDO)
		{
			// Create a OleDataObject from the input interface.
			OleDataObject oleData = new OleDataObject(pDO);

			// Check if the picked item is the one we added to the toolbox.
			if (oleData.GetDataPresent(typeof(ToolboxData)))
			{
				System.Diagnostics.Trace.WriteLine("MyToolboxItemData selected from the toolbox");
				ToolboxData myData = (ToolboxData)oleData.GetData(typeof(ToolboxData));
				editorControl.tbDropData.Text = myData.Content;
			}
			return VSConstants.S_OK;
		}
		#endregion

		#region IVsToolboxActiveUserHook (for Drag Drop)
		public int InterceptDataObject(IDataObject pIn, out IDataObject ppOut)
		{
			ToolboxData myData = new ToolboxData("NotDefined");
			ppOut = null;

			if (_selectedToolboxData == null)
			{
				return VSConstants.S_FALSE;
			}
			else
			{
				myData = (ToolboxData)_selectedToolboxData.GetData(typeof(ToolboxData));

			}


			OleDataObject newtoolboxData = new OleDataObject();
			newtoolboxData.SetText($"Dropped {myData?.Content}", System.Windows.Forms.TextDataFormat.Text);
			ppOut = newtoolboxData;
			return VSConstants.S_OK;
		}

		public int ToolboxSelectionChanged(IDataObject pSelected)
		{
			if (pSelected == null) return VSConstants.S_FALSE;
			_selectedToolboxData = (OleDataObject)pSelected;
			ToolboxData data = (ToolboxData)_selectedToolboxData.GetData(typeof(ToolboxData));
			editorControl.tbDropData.Text = data.Content;
			return VSConstants.S_OK;
		}
		#endregion

		#region Methods

		private void CreateToolboxData(string pToolboxItemName, string pData, string pTabInToolbox, bool pRemoveTabItems = false)
		{

			// Create the data object that will store the data for the menu item.
			OleDataObject toolboxData = new OleDataObject();
			toolboxData.SetData(typeof(ToolboxData), new ToolboxData(pData));
			// Get the toolbox service
			IVsToolbox toolbox = (IVsToolbox)GetService(typeof(SVsToolbox));
			//IVsToolbox2 vsToolbox2 = (IVsToolbox2)toolbox;
			//IVsToolbox3 vsToolbox3 = (IVsToolbox3)vsToolbox2;


			if (pRemoveTabItems)
			{
				bool succeed = ErrorHandler.Succeeded(toolbox.RemoveTab(pTabInToolbox));

			}

			// Create the array of TBXITEMINFO structures to describe the items
			// we are adding to the toolbox.
			TBXITEMINFO[] itemInfo = new TBXITEMINFO[1];
			itemInfo[0].bstrText = pToolboxItemName;
			itemInfo[0].clrTransparent = ColorToUInt(Color.Black);
			itemInfo[0].hBmp = Resources.cartForToolboxItem.GetHbitmap();
			itemInfo[0].dwFlags = (uint)__TBXITEMINFOFLAGS.TBXIF_DONTPERSIST;
			//itemInfo[0].iImageIndex = 0;
			//itemInfo[0].iImageWidth = 16;

			ErrorHandler.ThrowOnFailure(toolbox.AddItem(toolboxData, itemInfo, pTabInToolbox));

		}

		private uint ColorToUInt(Color color)
		{
			return (uint)((color.A << 24) | (color.R << 16) |
										(color.G << 8) | (color.B << 0));
		}

		public IVsStatusbar StatusBar
		{
			get
			{
				if (statusBar == null)
				{
					statusBar = GetService(typeof(SVsStatusbar)) as IVsStatusbar;
				}

				return statusBar;
			}
		}


		#endregion
	}
}
