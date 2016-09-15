namespace QSPSaveEditor.View
{
    using MahApps.Metro.Controls;
    using QSPNETWrapper.Model;
    using System.IO;

    /// <summary>
    /// Interaction logic for EditVariableView.xaml
    /// </summary>
    public partial class EditVariableView : MetroWindow
    {
        public EditVariableView()
        {
            InitializeComponent();

            var dataContext = this.DataContext as QSPVariable;

            using ( var stream = new MemoryStream() )
            {
                using ( var writer = new StreamWriter(stream) )
                {
                    writer.Write(dataContext.StringValue);
                    writer.Flush();
                    stream.Position = 0;
                    textEditor.Load(stream);
                }
            }
        }
    }
}
