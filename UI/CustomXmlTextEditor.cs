using System;
using System.Drawing;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace Raytracer.UI
{
	public class CustomXmlTextEditor : TextEditorControl
	{
		// Methods
		public CustomXmlTextEditor()
		{
			base.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("XML");
			base.Document.FoldingManager.FoldingStrategy = new Raytracer.ICSharpCode.XmlEditor.XmlFoldingStrategy();
            base.Document.FormattingStrategy = new Raytracer.ICSharpCode.SharpDevelop.DefaultEditor.XmlFormattingStrategy();
			base.TextEditorProperties = InitializeProperties();
			base.Document.FoldingManager.UpdateFoldings(string.Empty, null);
			base.textAreaPanel.Refresh();
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ImeModeBase = System.Windows.Forms.ImeMode.Disable;

            base.Document.DocumentChanged += this.Document_DocumentChanged;
		}

        private void Document_DocumentChanged(object sender, DocumentEventArgs e)
        {
            base.Document.FoldingManager.UpdateFoldings(string.Empty, null);
            base.textAreaPanel.Refresh();
        }

		private static ITextEditorProperties InitializeProperties()
		{
			var properties = new DefaultTextEditorProperties();
			properties.Font = new Font("Courier new", 9, FontStyle.Regular);
			properties.IndentStyle = IndentStyle.Smart;
			properties.ShowSpaces = false;
			properties.LineTerminator = Environment.NewLine;
			properties.ShowTabs = false;
			properties.ShowInvalidLines = false;
			properties.ShowEOLMarker = false;
			properties.TabIndent = 2;
			properties.CutCopyWholeLine = true;
			properties.LineViewerStyle = LineViewerStyle.None;
			properties.MouseWheelScrollDown = true;
			properties.MouseWheelTextZoom = true;
			properties.AllowCaretBeyondEOL = false;
			properties.AutoInsertCurlyBracket = true;
			properties.BracketMatchingStyle = BracketMatchingStyle.After;
			properties.ConvertTabsToSpaces = false;
			properties.ShowMatchingBracket = true;
			properties.EnableFolding = true;
			properties.ShowVerticalRuler = false;
			properties.IsIconBarVisible = true;
			properties.Encoding = System.Text.Encoding.Unicode;
			//properties.UseAntiAliasedFont = false;
            
			return properties;
		}
	}
}
