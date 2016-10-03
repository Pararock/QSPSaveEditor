using Microsoft.Toolkit.Uwp.UI.Controls;
using QSPEditor.Configuration;
using QSPEditor.ViewModels;
using QSPLib_CppWinrt;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace QSPEditor.Views
{
    public sealed partial class VariablesPage : Page
    {
        private readonly VariablesViewModel _variablesViewModel;
        private VariablesViewModel ViewModel
        {
            get { return _variablesViewModel; }
        }

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on VariablesPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public VariablesPage()
        {
            _variablesViewModel = ServiceLocator.Current.GetService<VariablesViewModel>();
            InitializeComponent();
        }

        private void dg_Sorting(object sender, DataGridColumnEventArgs e)
        {
            //Use the Tag property to pass the bound column name for the sorting implementation 
            if (e.Column.Tag.ToString() == "BaseName")
            {
                //Implement sort on the column "Range" using LINQ
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                        orderby item.BaseName ascending
                                                        select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                        orderby item.BaseName descending
                                                        select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            if (e.Column.Tag.ToString() == "Name")
            {
                //Implement sort on the column "Range" using LINQ
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                        orderby item.Name ascending
                                                        select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                        orderby item.Name descending
                                                        select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            if (e.Column.Tag.ToString() == "Number")
            {
                //Implement sort on the column "Range" using LINQ
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                        orderby item.Number ascending
                                                        select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                        orderby item.Number descending
                                                        select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
                if (e.Column.Tag.ToString() == "Position")
                {
                    //Implement sort on the column "Range" using LINQ
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                            orderby item.Position ascending
                                                            select item);
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        dg.ItemsSource = new List<Variable>(from item in _variablesViewModel.FilteredVariables
                                                            orderby item.Position descending
                                                            select item);
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                }
            }


            // Remove sorting indicators from other columns
            foreach (var dgColumn in dg.Columns)
            {
                if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    dgColumn.SortDirection = null;
                }
            }
        }
        //private void hideEmptyValue(object sender, RoutedEventArgs e)
        //{
        //    _variablesViewModel.
        //}

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
