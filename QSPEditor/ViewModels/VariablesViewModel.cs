using Microsoft.Toolkit.Uwp.UI;
using QSPEditor.Helpers;
using QSPEditor.Services;
using QSPLib_CppWinrt;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QSPEditor.ViewModels
{
    public class VariablesViewModel : Observable
    {
        private Variable _selected;
        private readonly Engine _engine;

        private bool m_grouped = true;

        public Variable Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public bool Grouped
        {
            get { return m_grouped; }
            set { ToggleGroupByBaseName(); }
        }

        private IEnumerable<Variable> _filteredVariables;
        private RelayCommand _toggleEmptyFilterCommand;
        private RelayCommand _toggleBaseNameCommand;

        public AdvancedCollectionView VariablesCollection;

        public IList<Variable> Variables => _engine.Variables;
        public IEnumerable<Variable> FilteredVariables => _filteredVariables;

        public ICommand ToggleEmptyFilterCommand => _toggleEmptyFilterCommand ?? (_toggleEmptyFilterCommand = new RelayCommand(ToggleEmptyFilter));
        public ICommand ToggleGroupByBaseNameCommand => _toggleBaseNameCommand ?? (_toggleBaseNameCommand = new RelayCommand(ToggleGroupByBaseName));

        private void ToggleGroupByBaseName()
        {
            if (m_grouped)
            {
                //Set(ref _filteredVariables, new List<Variable>(from variable in Variables
                //                                               where variable.Text != "" && variable.Number != 0
                //                                               select variable), nameof(FilteredVariables));
            }
            else
            {
                //Set(ref _filteredVariables, new List<Variable>(from variable in FilteredVariables
                //                                               group variable by variable.BaseName into baseNameGroup
                //                                               select baseNameGroup), nameof(FilteredVariables));
            }
            var acv = new AdvancedCollectionView((System.Collections.IList)Variables);

        }

        private void ToggleEmptyFilter()
        {

            //Set(ref _filteredVariables, new List<Variable>(from variable in Variables
            //                                               where variable.Text != "" && variable.Number != 0
            //                                               select variable), nameof(FilteredVariables));
        }

        public VariablesViewModel(IEngineService engineService)
        {
            _engine = engineService.Engine;
            _engine.PropertyChanged += _engine_PropertyChanged;
        }

        private void _engine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("Variables"))
            {
                //Set(ref _filteredVariables, _engine.Variables, nameof(FilteredVariables));
            }
        }

        public async Task LoadDataAsync()
        {
            Set(ref _filteredVariables, _engine.Variables, nameof(FilteredVariables));
            //Source.Clear();

            //// TODO WTS: Replace this with your actual data
            //var data = await SampleDataService.GetGridDataAsync();

            //foreach (var item in data)
            //{
            //    Source.Add(item);
            //}
        }
    }
}
