using QSPLib_CppWinrt;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace QSPEditor.Views
{
    public sealed partial class LocationsDetailControl : UserControl
    {
        public Location MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as Location; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(Location), typeof(LocationsDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public LocationsDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LocationsDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);

            if (control.MasterMenuItem == null) return;

            //control.locationEditor.Blocks.Clear();
            //var blocks = control.locationEditor.Blocks;

            int indent = 0;
            //FormatLines(0, control.MasterMenuItem.OnVisitLines, blocks);

        }

        //Ya not the best. This need to be it's own control
        public static int FormatLines(int indent, IList<LineOfCode> lines, BlockCollection blocks, int lineNumberToHigligh = -1)
        {
            foreach (var lineOfCode in lines)
            {
                if (lineOfCode.Text.Equals("end", StringComparison.InvariantCultureIgnoreCase) && indent > 0) indent -= 25;

                Paragraph paragraph = new Paragraph();

                Run run = new Run
                {
                    Text = $"{lineOfCode.LineNum}-{lineOfCode.CachedStats.Count}-{lineOfCode.Text}"
                };

                paragraph.TextIndent = indent;

                foreach (var statement in lineOfCode.CachedStats)
                {
                    if (statement.Stat == Statement.qspStatComment)
                    {
                        paragraph.FontStyle = Windows.UI.Text.FontStyle.Italic;
                        break;
                    }
                    //switch lineOfCode.CachedStat:
                    //{
                    //    case:1:
                    //}
                }

                if (!string.IsNullOrEmpty(lineOfCode.Label))
                {
                    paragraph.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
                }
                if (lineOfCode.LineNum == lineNumberToHigligh)
                {
                    var mergedDict = Application.Current.Resources.MergedDictionaries.Where(md => md.Source.AbsoluteUri == "ms-resource:///Files/Styles/_Colors.xaml").FirstOrDefault();
                    run.Foreground = (Brush)mergedDict["QSPSystemErrorTextColor"];
                }

                // Add the Run to the Paragraph, the Paragraph to the RichTextBlock.
                paragraph.Inlines.Add(run);
                blocks.Add(paragraph);

                // indent for next line
                if (lineOfCode.IsMultiline)
                    indent += 25;
            }

            return indent;
        }

    }
}
