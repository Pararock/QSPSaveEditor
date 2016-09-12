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

        private IList<QSPVariable> _variableList;
        private bool isGameWorldActive;
        private bool isGameWorldLoaded;

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

        public override IList<QSPVariable> VariablesList => _variableList;

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
            return isGameWorldLoaded;
        }

        public void ModifyVariables()
        {
            var dirtyVar = _variableList.Where(var => var.IsDirty).Select(var => var);
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
                    isGameWorldActive = true;
                    PopulateVariableList();
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
            QSPVariable newVariable;
            if ( strValue == null )
            {
                newVariable = new QSPVariable(name, intValue);
            }
            else if ( intValue != 0 )
            {
                newVariable = new QSPVariantVariable(name, intValue, strValue);
            }
            else
            {
                newVariable = new QSPVariable(name, strValue);
            }
            return newVariable;
        }

        private static QSPVariable CreateVariable( string parentName, string name, int intValue, string strValue )
        {
            QSPVariable newVariable;
            if ( strValue == null )
            {
                newVariable = new QSPNamedArrayVariable(parentName, name, intValue);
            }
            else
            {
                newVariable = new QSPNamedArrayVariable(parentName, name, strValue);
            }
            return newVariable;
        }


        private static QSPVariable CreateVariable( string parentName, int position, int intValue, string strValue )
        {
            QSPVariable newVariable;
            if ( strValue == null )
            {
                newVariable = new QSPPositionArrayVariable(parentName, position, intValue);
            }
            else
            {
                newVariable = new QSPPositionArrayVariable(parentName, position, strValue);
            }
            return newVariable;
        }

        private static bool ExecString( string cmd, bool isRefreshed )
        {
            return QSPWrapper.QSPExecString(cmd, isRefreshed);
        }

        private void PopulateVariableList()
        {
            var variablesList = new List<QSPVariable>();

            for ( int i = 0; i < MaxVariablesCount; i++ )
            {
                var name = QSPWrapper.GetVariableNameByIndex(i);
                if ( !string.IsNullOrEmpty(name) )
                {
                    var valueCount = QSPWrapper.GetVariableValuesCount(name);
                    var indexCount = QSPWrapper.GetVariableIndexesCount(name);

                    if ( indexCount == 0 )
                    {

                        int intValue;
                        string strValue;
                        QSPWrapper.GetVariableValues(name, 0, out intValue, out strValue);
                        variablesList.Add(CreateVariable(name, intValue, strValue));
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

                                variablesList.Add(CreateVariable(name, j, intValue, strValue));
                            }
                            else
                            {
                                int intValue;
                                string strValue;

                                QSPWrapper.GetVariableValues(name, valueIndex, out intValue, out strValue);

                                variablesList.Add(CreateVariable(name, indexName, intValue, strValue));
                            }
                        }
                    }
                }
            }

            _variableList = variablesList;
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
