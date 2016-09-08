namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class QSPGameWorld : QSPGame
    {

        private IEnumerable<QSPVariable> _variableList;
        private bool isGameWorldActive;

        private bool isGameWorldLoaded;

        public QSPGameWorld()
        {
            QSPInit();
        }

        ~QSPGameWorld()
        {
            QSPDeInit();
        }

        public enum QSPErrorCode
        {
            QSP_ERR_DIVBYZERO = 100,
            QSP_ERR_TYPEMISMATCH,
            QSP_ERR_STACKOVERFLOW,
            QSP_ERR_TOOMANYITEMS,
            QSP_ERR_FILENOTFOUND,
            QSP_ERR_CANTLOADFILE,
            QSP_ERR_GAMENOTLOADED,
            QSP_ERR_COLONNOTFOUND,
            QSP_ERR_CANTINCFILE,
            QSP_ERR_CANTADDACTION,
            QSP_ERR_EQNOTFOUND,
            QSP_ERR_LOCNOTFOUND,
            QSP_ERR_ENDNOTFOUND,
            QSP_ERR_LABELNOTFOUND,
            QSP_ERR_NOTCORRECTNAME,
            QSP_ERR_QUOTNOTFOUND,
            QSP_ERR_BRACKNOTFOUND,
            QSP_ERR_BRACKSNOTFOUND,
            QSP_ERR_SYNTAX,
            QSP_ERR_UNKNOWNACTION,
            QSP_ERR_ARGSCOUNT,
            QSP_ERR_CANTADDOBJECT,
            QSP_ERR_CANTADDMENUITEM,
            QSP_ERR_TOOMANYVARS,
            QSP_ERR_INCORRECTREGEXP,
            QSP_ERR_CODENOTFOUND,
            QSP_ERR_TONOTFOUND
        };

        public override int ActionsCount => QSPGetActionsCount();

        public override DateTime CompiledDate
        {
            get
            {
                // Return in format : Jun  6 2010, 23:16:21
                var compiledDatePtr = QSPGetCompiledDateTime();
                var compiledDate = Marshal.PtrToStringUni(compiledDatePtr);

                var date = DateTime.Parse(compiledDate, new CultureInfo("en-US", false));
                return date;
            }
        }

        public override int FullRefreshCount => QSPGetFullRefreshCount();

        public override int MaxVariablesCount => QSPGetMaxVarsCount();

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

        public override Version Version
        {
            get
            {
                var versionpt = QSPGetVersion();
                var version = Marshal.PtrToStringUni(versionpt);
                return Version.Parse(version);
            }
        }

        public string GetMainDesc()
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

        public string GetVarsDesc()
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
            isGameWorldLoaded = QSPLoadGameWorld(QSPPath);
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
                if ( QSPOpenSavedGame(savePath, isRefreshed) )
                {
                    isGameWorldActive = true;
                    PopulateVariableList();
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
                return QSPSaveGame(savePath, isRefreshed);
            }
            else
            {
                return false;
            }
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
                newVariable = new QSPArrayVariable(parentName, name, intValue);
            }
            else
            {
                newVariable = new QSPArrayVariable(parentName, name, strValue);
            }
            return newVariable;
        }

        private static bool ExecString( string cmd, bool isRefreshed )
        {
            return QSPExecString(cmd, isRefreshed);
        }

        private static string GetErrorDesc( QSPErrorCode error )
        {
            var errorMsgptr = QSPGetErrorDesc(error);
            return Marshal.PtrToStringUni(errorMsgptr);
        }

        private static string GetLastError()
        {
            QSPErrorCode error;
            int errorActIndex;
            int errorLine;
            var ptrError = IntPtr.Zero;
            QSPGetLastErrorData(out error, ref ptrError, out errorActIndex, out errorLine);
            return ptrError == IntPtr.Zero ? GetErrorDesc(error) : Marshal.PtrToStringUni(ptrError);
        }

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPDeInit();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPExecString( [MarshalAsAttribute(UnmanagedType.LPWStr)] string str, bool isRefresh );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int QSPGetActionsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetCompiledDateTime();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetErrorDesc( [MarshalAsAttribute(UnmanagedType.I4)]  QSPErrorCode error );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int QSPGetFullRefreshCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPGetLastErrorData( [MarshalAsAttribute(UnmanagedType.I4)] out QSPErrorCode error, ref IntPtr errorLoc, out int errorActIndex, out int errorLine );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetMainDesc();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int QSPGetMaxVarsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int QSPGetObjectsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetQstFullPath();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPGetVarIndex( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, int ind, out int numVal, ref IntPtr strVal );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPGetVarIndexesCount( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, out int count );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPGetVarNameByIndex( int ind, ref IntPtr name );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetVarsDesc();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPGetVarValues( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, int ind, out int numVal, ref IntPtr strVal );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPGetVarValuesCount( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, out int count );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetVersion();
        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPInit();

        [DllImport("qsplib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPLoadGameWorld( [MarshalAsAttribute(UnmanagedType.LPWStr)] String gamePath );

        [DllImport("qsplib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPOpenSavedGame( [MarshalAsAttribute(UnmanagedType.LPWStr)] String savePath, [MarshalAsAttribute(UnmanagedType.Bool)] bool isRefresh );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QSPSaveGame( [MarshalAsAttribute(UnmanagedType.LPWStr)] string savePath, [MarshalAsAttribute(UnmanagedType.Bool)]  bool isRefresh );

        private bool GetVariableIndex( string name, int index, out int varIndex, out string indexName )
        {
            if ( isGameWorldActive )
            {
                var ptrValue = IntPtr.Zero;
                var result = QSPGetVarIndex(name, index, out varIndex, ref ptrValue);
                indexName = Marshal.PtrToStringUni(ptrValue);
                return result;
            }
            else
            {
                varIndex = 0;
                indexName = string.Empty;
                return false;
            }
        }

        private int GetVariableIndexesCount( string name )
        {
            if ( isGameWorldActive )
            {
                int count;
                QSPGetVarIndexesCount(name, out count);
                return count;
            }
            else
            {
                return 0;
            }
        }

        private string GetVariableNameByIndex( int index )
        {
            if ( isGameWorldActive )
            {
                var ptrVariableName = IntPtr.Zero;
                QSPGetVarNameByIndex(index, ref ptrVariableName);
                return Marshal.PtrToStringUni(ptrVariableName);
            }
            else
            {
                return string.Empty;
            }
        }

        private bool GetVariableValues( string name, int index, out int intValue, out string strValue )
        {
            if ( isGameWorldActive )
            {
                var ptrValue = IntPtr.Zero;
                int intVal;
                var result = QSPGetVarValues(name, index, out intVal, ref ptrValue);

                if ( ptrValue == IntPtr.Zero )
                {
                    intValue = intVal;
                    strValue = null;
                }
                else
                {
                    strValue = Marshal.PtrToStringUni(ptrValue);
                    intValue = 0;
                }
                return result;
            }
            else
            {
                intValue = 0;
                strValue = null;
                return false;
            }
        }

        private int GetVariableValuesCount( string name )
        {
            if ( isGameWorldActive )
            {
                int count;
                QSPGetVarValuesCount(name, out count);
                return count;
            }
            else
            {
                return 0;
            }
        }

        private void PopulateVariableList()
        {
            var variablesList = new List<QSPVariable>();

            for ( int i = 0; i < MaxVariablesCount; i++ )
            {
                var name = GetVariableNameByIndex(i);
                if ( !string.IsNullOrEmpty(name) )
                {
                    var valueCount = GetVariableValuesCount(name);
                    var indexCount = GetVariableIndexesCount(name);

                    if ( indexCount == 0 )
                    {

                        int intValue;
                        string strValue;
                        GetVariableValues(name, 0, out intValue, out strValue);
                        variablesList.Add(CreateVariable(name, intValue, strValue));
                    }
                    else
                    {
                        for ( int j = 0; j < indexCount; j++ )
                        {
                            int valueIndex;
                            string indexName;
                            GetVariableIndex(name, j, out valueIndex, out indexName);

                            int intValue;
                            string strValue;

                            GetVariableValues(name, valueIndex, out intValue, out strValue);

                            variablesList.Add(CreateVariable(name, indexName, intValue, strValue));
                        }
                    }
                }
            }

            _variableList = variablesList;
        }
    }
}
