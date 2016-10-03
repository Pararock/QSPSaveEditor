using QSPLib_CppWinrt;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace QSPEditor.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LocationEditorControl : Page
    {
        public Location Location
        {
            get { return GetValue(LocationProperty) as Location; }
            set { SetValue(LocationProperty, value); }
        }

        private string AllText;

        public string CodeContent
        {
            get { return AllText; }
            set { AllText = value; }
        }

        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Selected", typeof(Location), typeof(LocationEditorControl), new PropertyMetadata(null, OnLocationEditorPropertyChanged));

        public LocationEditorControl()
        {
            this.InitializeComponent();
        }

        private static void OnLocationEditorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Location newLocation)
            {
                if (d is LocationEditorControl editorControl)
                {
                    var content = new StringBuilder();
                    foreach (var line in newLocation.OnVisitLines)
                    {
                        content.AppendLine(line.Text);
                    }

                    editorControl.Editor.Text = content.ToString();
                }
            }
        }
    }
}
