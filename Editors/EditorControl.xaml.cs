using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EditorWithToolbox2017.EditorPackage;
using EditorWithToolbox2017.ToolboxItems;

namespace EditorWithToolbox2017.Editors
{
	/// <summary>
	/// Interaktionslogik für EditorControl.xaml
	/// </summary>
	public partial class EditorControl : UserControl
	{
		EditorPane editorPane;

		public EditorControl()
		{
			InitializeComponent();
		}

		public EditorControl(EditorPane pEditorPane) : this()
		{
			editorPane = pEditorPane;
		}

		public bool IsDirty { get; set; }

		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			IsDirty = (bool)cbIsDirty.IsChecked;
		}

		private void Grid_MouseMove(object sender, MouseEventArgs e)
		{
			//if (e.LeftButton == MouseButtonState.Pressed)
			//{
			//	if (e.Source != null)
			//	{
			//		DragDrop.DoDragDrop((Grid)e.Source, (Grid)e.Source, DragDropEffects.Move);
			//	}
			//}
		}

		private void Grid_PreviewDragEnter(object sender, DragEventArgs e)
		{
			if (editorPane != null)
				editorPane.StatusBar.SetText("DragEnter :" + e.Data.ToString());
			e.Handled = true;
		}

		private void Grid_DragOver(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.Copy | DragDropEffects.Move;

			if (editorPane != null)
				editorPane.StatusBar.SetText("Grid_DragOver :" + e.Data.ToString());
			e.Handled = true;
		}

		private void Grid_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.StringFormat))
			{
				string str = e.Data.GetData(DataFormats.StringFormat) as string;
				tbDropData.Text = str;
			}
			e.Handled = true;
		}
	}
}
