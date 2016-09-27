namespace QSPSaveEditor.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using Message;
    using Model;
    using QSPNETWrapper.Model;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Data;
    using View;
    using System.IO;
    using System.Windows;
    using ICSharpCode.AvalonEdit;
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit.Document;

    public class VariablesViewModel : ViewModelBase
    {
        public bool isSaveLoaded;
        private RelayCommand clearFiltercommand;
        private RelayCommand resetBaselineCommand;

        private string filterString = String.Empty;
        private bool filterModifiedOnly;
        private IQSPGameDataService gameDataService;
        private IQSPVariablesListDataService variableDataService;

        private IList<QSPVariable> variablesList;
        private ICollectionView variablesView;

        private RelayCommand<bool> openVariableEditorCommand;
        private RelayCommand closeEditorCommand;

        public IList<QSPVariable> VariableList => variablesList;

        private TextDocument textDocument;

        private bool variableEditorOpen;
        private bool isEditingNewString;
        private QSPVariable currentVariable;

        private int characterLimitFilter = 25;

        public VariablesViewModel( IQSPGameDataService gameDataService, IQSPVariablesListDataService variableDataService )
        {
            this.gameDataService = gameDataService;
            this.variableDataService = variableDataService;

            this.MessengerInstance.Register<SaveMessage>(this, ReceiveSaveMessage);

            if ( IsInDesignMode )
            {
                UpdateListAsync();
            }
        }

        public TextDocument VariableDocument
        {
            get
            {
                return textDocument;
            }
            set
            {
                Set(nameof(VariableDocument), ref textDocument, value);
            }
        }

        public bool VariableEditorOpen
        {
            get
            {
                return variableEditorOpen;
            }
            set
            {
                Set(nameof(VariableEditorOpen), ref variableEditorOpen, value);
            }
        }

        private void ReceiveSaveMessage( SaveMessage message )
        {
            switch (message.MessageType)
            {
                case SaveMessageType.SaveLoaded:
                    IsSaveLoaded = true;
                    UpdateListAsync();
                    break;
                case SaveMessageType.SaveClosed:
                    IsSaveLoaded = false;
                    VariablesView = null;
                    break;
            }

        }

        public RelayCommand CloseEditorCommand => closeEditorCommand ?? (closeEditorCommand = new RelayCommand( () =>
        {
            VariableEditorOpen = false;            

            if ( isEditingNewString )
            {
                currentVariable.NewStringValue = VariableDocument.Text;
            }
            else
            {
                currentVariable.StringValue = VariableDocument.Text;
            }
        }));

        public RelayCommand<bool> OpenVariableEditorCommand => openVariableEditorCommand ?? (openVariableEditorCommand = new RelayCommand<bool>((isNewString) =>
        {
            currentVariable = VariablesView.CurrentItem as QSPVariable;
            VariableEditorOpen = !VariableEditorOpen;

            isEditingNewString = isNewString;

            if ( isEditingNewString )
            {
                VariableDocument = new TextDocument(currentVariable.NewStringValue.ToArray());
            }
            else
            {
                VariableDocument = new TextDocument(currentVariable.StringValue.ToArray());
            }
        }));

        public RelayCommand ClearFilterCommand => clearFiltercommand ?? (clearFiltercommand = new RelayCommand(() => VariablesFilter = string.Empty));

        public RelayCommand ResetBaseLineCommand => resetBaselineCommand ?? (resetBaselineCommand = new RelayCommand( () =>
            {
                variableDataService.ResetVariablesBaseline();
                ResetBaseLineCommand.RaiseCanExecuteChanged();
            },
            () =>
            {
                return IsSaveLoaded ? variablesList.Any(x => x.IsModified) : false;
            }));

        public string VariablesFilter
        {
            get
            {
                return filterString;
            }
            set
            {
                if ( value == filterString ) return;
                Set(nameof(VariablesFilter), ref filterString, value);
                VariablesView.Refresh();

            }
        }
        public bool ModifiedFilter
        {
            get
            {
                return filterModifiedOnly;
            }
            set
            {
                if ( value == filterModifiedOnly ) return;
                Set(nameof(ModifiedFilter), ref filterModifiedOnly, value);
                VariablesView.Refresh();
            }
        }


        public ICollectionView VariablesView
        {
            get
            {
                return variablesView;
            }
            set
            {
                Set(nameof(VariablesView), ref variablesView, value);
            }
        }

        public bool IsSaveLoaded
        {
            get
            {
                return isSaveLoaded;
            }
            set
            {
                Set(nameof(IsSaveLoaded), ref isSaveLoaded, value);
            }
        }

        private async void UpdateListAsync( )
        {
            variablesList = await variableDataService.GetQSPVariableList(gameDataService);
            VariablesView = CollectionViewSource.GetDefaultView(variablesList);
            VariablesView.Filter = VariableseFilter;
            ResetBaseLineCommand.RaiseCanExecuteChanged();
        }

        private bool VariableseFilter( object item )
        {
            var variable = item as QSPVariable;

            if(ModifiedFilter && !variable.IsModified)
            {
                return false;
            }

            if ( !string.IsNullOrEmpty(VariablesFilter) )
            {
                return variable.ExecString.IndexOf(VariablesFilter, 0, StringComparison.OrdinalIgnoreCase) != -1;
            }
            
            /*if(characterLimitFilter > 0 && variable.CharacterCount > characterLimitFilter)
            {
                return false;
            }*/

            return true;
        }
    }
}
