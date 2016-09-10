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

        private IEnumerable<QSPVariable> _variableList;
        private bool isGameWorldActive;
        private bool isGameWorldLoaded;

        public QSPWrapper qspWrapper;

        public QSPGameWorld()
        {
            qspWrapper = new QSPWrapper();
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override int ActionsCount => QSPGetActionsCount();

        public override DateTime CompiledDate
        {
            get
            {
                // Return in format : Jun  6 2010, 23:16:21
                var compiledDatePtr = QSPWrapper.QSPGetCompiledDateTime();
                var compiledDate = Marshal.PtrToStringUni(compiledDatePtr);

                var date = DateTime.Parse(compiledDate, new CultureInfo("en-US", false));
                return date;
            }
        }

        public override Version Version
        {
            get
            {
                var versionpt = QSPWrapper.QSPGetVersion();
                var version = Marshal.PtrToStringUni(versionpt);
                return Version.Parse(version);
            }
        }

        public override int FullRefreshCount => QSPGetFullRefreshCount();

        public override bool IsMainDescriptionChanged => QSPIsMainDescChanged();

        public override bool IsVarsDescChanged => QSPIsVarsDescChanged();

        public override int MaxVariablesCount => QSPWrapper.QSPGetMaxVarsCount();

        public override int ObjectsCount => QSPGetObjectsCount();

        public override string QSPFilePath
        {
            get
            {
                if ( isGameWorldLoaded )
                {
                    var ptrFilePath = QSPGetQstFullPath();
                    return Marshal.PtrToStringUni(ptrFilePath);
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public override IEnumerable<QSPVariable> VariablesList => _variableList;

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


        public string GetCurrentLocation()
        {
            if ( isGameWorldLoaded )
            {
                var ptrCurLoc = QSPGetCurLoc();
                return Marshal.PtrToStringUni(ptrCurLoc);
            }
            else
            {
                return string.Empty;
            }
        }

        public override string GetMainDesc()
        {
            if ( isGameWorldActive )
            {
                var ptrVarsDesc = QSPGetMainDesc();
                return Marshal.PtrToStringUni(ptrVarsDesc);
            }
            else
            {
                return string.Empty;
            }
        }

        public override string GetVarsDesc()
        {
            if ( isGameWorldActive )
            {
                var ptrVarsDesc = QSPGetVarsDesc();
                return Marshal.PtrToStringUni(ptrVarsDesc);
            }
            else
            {
                return string.Empty;
            }
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
                ExecString(variable.ExecString, true);
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
            if ( isGameWorldLoaded && isGameWorldActive )
            {
                return QSPWrapper.QSPWriteSaveGame(savePath, isRefreshed);
            }
            else
            {
                return false;
            }
        }

        protected void OnPropertyChanged( string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SendPropertyChange()
        {
            OnPropertyChanged(nameof(ActionsCount));
            OnPropertyChanged(nameof(FullRefreshCount));
            OnPropertyChanged(nameof(ObjectsCount));
        }

        private static QSPVariable CreateVariable( string name, int intValue, string strValue )
        {
            QSPVariable newVariable;
            if ( strValue == null )
            {
                newVariable = new QSPVariable(name, intValue);
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
            return QSPExecString(cmd, isRefreshed);
        }


        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPExecString( [MarshalAsAttribute(UnmanagedType.LPWStr)] string str, bool isRefresh );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int QSPGetActionsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetCurLoc();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int QSPGetFullRefreshCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetMainDesc();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int QSPGetObjectsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetQstFullPath();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetVarsDesc();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool QSPIsMainDescChanged();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool QSPIsVarsDescChanged();

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

                            if(String.IsNullOrEmpty(indexName))
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
    }
}
