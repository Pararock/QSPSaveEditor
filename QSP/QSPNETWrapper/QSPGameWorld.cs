namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class QSPGameWorld : QSPGame
    {

        public QSPWrapper qspWrapper;

        private Dictionary<string, QSPVariable> _variableList;
        private bool isGameWorldActive;
        private bool isGameWorldLoaded;

        private string currentSaveFile = string.Empty;

        public QSPGameWorld()
        {
            qspWrapper = new QSPWrapper();
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override int ActionsCount => QSPWrapper.GetActionsCount();

        public override DateTime CompiledDate => QSPWrapper.GetCompiledDate();

        public override string CurrentLocation => QSPWrapper.GetCurrentLocation();

        public override int FullRefreshCount => QSPWrapper.GetFullRefreshCount();

        public override bool IsMainDescriptionChanged => QSPWrapper.QSPIsMainDescChanged();

        public override bool IsVarsDescChanged => QSPWrapper.IsVarsDescChanged();

        public override string MainDescription => QSPWrapper.GetMainDesc();

        public override int MaxVariablesCount => QSPWrapper.QSPGetMaxVarsCount();

        public override int ObjectsCount => QSPWrapper.GetObjectsCount();

        public override string QSPFilePath => QSPWrapper.GetQstFullPath();

        public override IList<QSPVariable> VariablesList => _variableList.Values.ToList();

        public override string VarsDescription => QSPWrapper.GetVarsDesc();

        public override Version Version => QSPWrapper.GetVersion();

        public static Exception GetLastError()
        {
            QSPWrapper.QSPErrorCode error;
            int errorActIndex;
            int errorLine;
            var ptrError = IntPtr.Zero;
            QSPWrapper.QSPGetLastErrorData(out error, ref ptrError, out errorActIndex, out errorLine);
            Exception exception;
            if ( ptrError == IntPtr.Zero )
            {
                exception = new Exception(QSPWrapper.GetErrorDesc(error));
            }
            else
            {
                var errorStr = Marshal.PtrToStringUni(ptrError);
                exception = new Exception($"Error #{error} {errorStr} actIndex: {errorActIndex} line:{errorLine}");
            }

            return exception;
        }

        public override bool ExecCommand( string command )
        {
            return QSPWrapper.QSPExecString(command, false);
        }

        public bool LoadGameWorld( string QSPPath )
        {
            isGameWorldLoaded = QSPWrapper.QSPOpenGameFile(QSPPath);
            isGameWorldActive = false;
            return isGameWorldLoaded;
        }

        public void ModifyVariables()
        {
            var dirtyVar = _variableList.Where(var => var.Value.IsDirty).Select(var => var.Value);
            foreach ( var variable in dirtyVar )
            {
                if(ExecString(variable.ExecString, true))
                {
                    variable.IsDirty = false;
                }
            }
        }

        public bool OpenSavedGame( string savePath, bool isRefreshed )
        {
            if ( isGameWorldLoaded )
            {
                if ( QSPWrapper.QSPLoadSavedGame(savePath, isRefreshed) )
                {
                    if(isGameWorldActive && savePath == currentSaveFile )
                    {
                        UpdateVariableList();
                    }
                    else
                    {
                        currentSaveFile = savePath;
                        isGameWorldActive = true;
                        PopulateVariableList();
                    }

                    SendPropertyChange();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool WriteSaveGame( string savePath, bool isRefreshed )
        {
            return QSPWrapper.QSPWriteSaveGame(savePath, isRefreshed);
        }

        protected void OnPropertyChanged( string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static QSPVariable CreateVariable( string name, int intValue, string strValue )
        {
            return new QSPVariable(name, strValue, intValue); ;
        }

        private static QSPVariable CreateVariable( string parentName, string name, int intValue, string strValue )
        {
            return new QSPNamedArrayVariable(parentName, name, strValue, intValue);
        }


        private static QSPVariable CreateVariable( string parentName, int position, int intValue, string strValue )
        {
            return new QSPPositionArrayVariable(parentName, position, strValue, intValue); ;
        }

        private static bool ExecString( string cmd, bool isRefreshed )
        {
            return QSPWrapper.QSPExecString(cmd, isRefreshed);
        }

        public void UpdateVariableList()
        {
            //var newVariablesList = new Dictionary<string, QSPVariable>();

            for ( int i = 0; i < MaxVariablesCount; i++ )
            {
                var name = QSPWrapper.GetVariableNameByIndex(i);
                if ( !string.IsNullOrEmpty(name) )
                {
                    if( !_variableList.ContainsKey(name) )
                    {
                        var newVariable = CreateNewVariable(name);
                        newVariable.IsNew = true;
                        _variableList.Add(name, newVariable);
                    }
                    else
                    {
                        var oldVariable = _variableList[name];
                        var newViariable = CreateNewVariable(name);
                        if( oldVariable.GetType() == newViariable.GetType())
                        {
                            oldVariable.NewValues(newViariable);
                        }
                    }
                }
            }
            OnPropertyChanged(nameof(VariablesList));
            //_variableList = newVariablesList;
        }

        private void PopulateVariableList()
        {
            var variablesList = new Dictionary<string, QSPVariable>();

            for ( int i = 0; i < MaxVariablesCount; i++ )
            {
                var name = QSPWrapper.GetVariableNameByIndex(i);
                if ( !string.IsNullOrEmpty(name) )
                {
                    variablesList.Add(name, CreateNewVariable(name));
                }
            }

            _variableList = variablesList;
        }

        private static QSPVariable CreateNewVariable( string name )
        {
            QSPVariable newVariable = null;
            var valueCount = QSPWrapper.GetVariableValuesCount(name);
            var indexCount = QSPWrapper.GetVariableIndexesCount(name);

            if ( indexCount == 0 )
            {

                int intValue;
                string strValue;
                QSPWrapper.GetVariableValues(name, 0, out intValue, out strValue);
                newVariable = CreateVariable(name, intValue, strValue);
            }
            else
            {
                for ( int j = 0; j < indexCount; j++ )
                {
                    int valueIndex;
                    string indexName;
                    QSPWrapper.GetVIndexNameForVariable(name, j, out valueIndex, out indexName);

                    if ( String.IsNullOrEmpty(indexName) )
                    {
                        int intValue;
                        string strValue;

                        QSPWrapper.GetVariableValues(name, j, out intValue, out strValue);

                        newVariable = CreateVariable(name, j, intValue, strValue);
                    }
                    else
                    {
                        int intValue;
                        string strValue;

                        QSPWrapper.GetVariableValues(name, valueIndex, out intValue, out strValue);

                        newVariable = CreateVariable(name, indexName, intValue, strValue);
                    }
                }
            }
            return newVariable;
        }

        private void SendPropertyChange()
        {
            OnPropertyChanged(nameof(ActionsCount));
            OnPropertyChanged(nameof(FullRefreshCount));
            OnPropertyChanged(nameof(ObjectsCount));
            OnPropertyChanged(nameof(CurrentLocation));
            OnPropertyChanged(nameof(MainDescription));
            OnPropertyChanged(nameof(VarsDescription));
            OnPropertyChanged(nameof(QSPFilePath));
        }
    }
}
