namespace QSPNETWrapper
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using static QSPGameWorld;

    /// <summary>
    /// Wrapper class to expose the QSP Game Engine to .net
    /// </summary>
    public class QSPWrapper
    {
        public QSPWrapper()
        {
            QSPInit();
        }

        ~QSPWrapper()
        {
            QSPDeInit();
        }

        /// <summary>
        /// Get the name of the variable at index
        /// </summary>
        /// <param name="index">Int between 0 and QSPGetMaxVarsCount()</param>
        /// <returns>The name of the variable</returns>
        public static string GetVariableNameByIndex( int index )
        {
            var ptrVariableName = IntPtr.Zero;
            QSPGetVarNameByIndex(index, ref ptrVariableName);
            return Marshal.PtrToStringUni(ptrVariableName);
        }

        /// <summary>
        /// Get the number of variables for name
        /// This indicate weither the variable is a single variable or an array of variables
        /// </summary>
        /// <param name="name">The name of the variables</param>
        /// <returns>The number of sub variables for this variable</returns>
        public static int GetVariableValuesCount( string name )
        {
            int count;
            return QSPGetVarValuesCount(name, out count) ? count : 0;
        }

        /// <summary>
        /// Get the number of indexes for a variables
        /// This indicate weither the variable is a single variable or an array of variables
        /// </summary>
        /// <param name="name">The name of the variables</param>
        /// <returns>The number of sub indexes for this variable</returns>
        public static int GetVariableIndexesCount( string name )
        {
            int count;
            return QSPGetVarIndexesCount(name, out count) ? count : 0;
        }

        /// <summary>
        /// Get the value for a variables name.
        /// </summary>
        /// <param name="name">The name of the variables</param>
        /// <param name="index">The index to access if it's an array</param>
        /// <param name="intValue">The int value for this variables at index</param>
        /// <param name="strValue">The string value for his variable at index</param>
        /// <returns></returns>
        public static bool GetVariableValues( string name, int index, out int intValue, out string strValue )
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
                intValue = intVal;
            }
            return result;
        }

        /// <summary>
        /// Get the index name and the position for this index name this variable have stored.
        /// Index and values can be at different position
        /// For example, take the Variable $foo['subFoo']. foo is the name of the variables and subFoo is one of the indexname.
        /// Even if subfoo is at position 0 in GetVariableIndex, it can point to the value at position 2 in GetVariableValues
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="index">Position of the index name to retrieve</param>
        /// <param name="varIndex">Position of this value in GetVariableValues</param>
        /// <param name="indexName">The name of this index</param>
        /// <returns></returns>
        public static bool GetIndexNameForVariable( string name, int index, out int varIndex, out string indexName )
        {
            var ptrValue = IntPtr.Zero;
            var result = QSPGetVarIndex(name, index, out varIndex, ref ptrValue);
            indexName = Marshal.PtrToStringUni(ptrValue);
            return result;
        }

        public static string GetCurrentLocation()
        {
            var ptrValue = QSPGetCurLoc();
            return Marshal.PtrToStringUni(ptrValue);
        }

        public static string GetVarsDesc()
        {
            var ptrValue = QSPGetVarsDesc();
            return Marshal.PtrToStringUni(ptrValue);
        }

        public static string GetMainDesc()
        {
            var ptrValue = QSPGetMainDesc();
            return Marshal.PtrToStringUni(ptrValue);
        }

        public static DateTime GetCompiledDate()
        {
            // Return in format : Jun  6 2010, 23:16:21
            var compiledDatePtr = QSPGetCompiledDateTime();
            var compiledDate = Marshal.PtrToStringUni(compiledDatePtr);

            var date = DateTime.Parse(compiledDate, new CultureInfo("en-US", false));
            return date;
        }

        public static Version GetVersion()
        {
            var versionpt = QSPGetVersion();
            var version = Marshal.PtrToStringUni(versionpt);
            return Version.Parse(version);
        }

        public static string GetQstFullPath()
        {
            var ptrValue = QSPGetQstFullPath();
            return Marshal.PtrToStringUni(ptrValue);
        }

        public static void GetCurrentStateData(out string location, out int actIndex, out int line)
        {
            var loc = IntPtr.Zero;
            QSPGetCurStateData(ref loc, out actIndex, out line);
            location = Marshal.PtrToStringUni(loc);
        }

        public static bool GetLocationName( int index, out string locationName )
        {
            var ptrValue = IntPtr.Zero;
            var result = QSPGetLocationName(index, ref ptrValue);
            locationName = Marshal.PtrToStringUni(ptrValue);
            return result;
        }

        public static void GetActionData(int index, out string imgPath, out string desc)
        {
            var ìmgPathPtr = IntPtr.Zero;
            var descPtr = IntPtr.Zero;
            QSPGetActionData(index, ref ìmgPathPtr, ref descPtr);
            imgPath = Marshal.PtrToStringUni(ìmgPathPtr);
            desc = Marshal.PtrToStringUni(descPtr);
        }

        public static void GetObjectData( int index, out string imgPath, out string desc )
        {
            var ìmgPathPtr = IntPtr.Zero;
            var descPtr = IntPtr.Zero;
            QSPGetObjectData(index, ref ìmgPathPtr, ref descPtr);
            imgPath = Marshal.PtrToStringUni(ìmgPathPtr);
            desc = Marshal.PtrToStringUni(descPtr);
        }



        #region Init / Debug / Version / Compiled date / MaxVars // Error
        //-------------------------------------------------------------------------------------------------------------------------

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPInit();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPDeInit();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void QSPGetLastErrorData( [MarshalAsAttribute(UnmanagedType.I4)] out QSPErrorCode error, ref IntPtr errorLoc, out int errorActIndex, out int errorLine );

        public static string GetErrorDesc( QSPErrorCode error )
        {
            var errorMsgptr = QSPGetErrorDesc(error);
            return Marshal.PtrToStringUni(errorMsgptr);
        }

        [DllImport("qsplib.dll",EntryPoint = "QSPEnableDebugMode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void EnableDebugMode( bool isDebug );

        /// <summary>
        /// Get the version of the wrapped QSP library
        /// </summary>
        /// <returns></returns>
        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetVersion();

        /// <summary>
        /// Get the compilated date for the wrapper QSP Library
        /// </summary>
        /// <returns></returns>
        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetCompiledDateTime();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetErrorDesc( [MarshalAsAttribute(UnmanagedType.I4)]  QSPErrorCode error );

        #endregion

        #region Open Game / Load Save / Save Game
        //-------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Open a QSP game at gamepath
        /// </summary>
        /// <param name="gamePath"></param>
        /// <returns></returns>
        [DllImport("qsplib.dll", EntryPoint= "QSPLoadGameWorld", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool QSPOpenGameFile( [MarshalAsAttribute(UnmanagedType.LPWStr)] String gamePath );

        /// <summary>
        /// Load a saved game
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        [DllImport("qsplib.dll", EntryPoint= "QSPOpenSavedGame", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool QSPLoadSavedGame( [MarshalAsAttribute(UnmanagedType.LPWStr)] String savePath, bool isRefresh );

        /// <summary>
        /// Write the game state to disk
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        [DllImport("qsplib.dll", EntryPoint= "QSPSaveGame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool QSPWriteSaveGame( [MarshalAsAttribute(UnmanagedType.LPWStr)] string savePath, bool isRefresh );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetQstFullPath();
        #endregion

        #region Variables
        //-------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Get the maximum number of variables the wrapper QSP Library is compiled for
        /// </summary>
        /// <returns></returns>
        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int QSPGetMaxVarsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPGetVarNameByIndex( int ind, ref IntPtr name );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool QSPGetVarValuesCount( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, out int count );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool QSPGetVarIndexesCount( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, out int count );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool QSPGetVarValues( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, int ind, out int numVal, ref IntPtr strVal );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool QSPGetVarIndex( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, int ind, out int numVal, ref IntPtr strVal );

        #endregion

        #region Location
        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetCurLoc();


        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPGetCurStateData( ref IntPtr loc, out int actIndex, out int line );

        [DllImport("qsplib.dll", EntryPoint = "QSPPGetLocationsCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLocationsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool QSPGetLocationName( int index, ref IntPtr locName );

        #endregion

        #region Main Description

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetMainDesc();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool QSPIsMainDescChanged();

        #endregion

        #region Vars Description

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr QSPGetVarsDesc();

        [DllImport("qsplib.dll", EntryPoint = "QSPIsVarsDescChanged", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool IsVarsDescChanged();

        #endregion

        #region Objects

        [DllImport("qsplib.dll", EntryPoint = "QSPGetObjectsCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetObjectsCount();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void QSPGetObjectData( int ind, ref IntPtr imgPath, ref IntPtr desc );

        #endregion

        #region Actions

        [DllImport("qsplib.dll", EntryPoint = "QSPGetActionsCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetActionsCount();


        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void QSPGetActionData( int ind, ref IntPtr imgPath, ref IntPtr desc );

        #endregion

        #region Gaming Function

        [DllImport("qsplib.dll",EntryPoint = "QSPGetFullRefreshCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetFullRefreshCount();


        [DllImport("qsplib.dll", EntryPoint = "QSPRestartGame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool RestartGame(bool isRefreshed);


        [DllImport("qsplib.dll", EntryPoint = "QSPSetInputStrText", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SetInputStringText( [MarshalAsAttribute(UnmanagedType.LPWStr)] string inputString);

        #endregion


        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QSPExecString( [MarshalAsAttribute(UnmanagedType.LPWStr)] string str, bool isRefresh );

        [DllImport("qsplib.dll",EntryPoint = "QSPSetCallBack", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetCallBack( [MarshalAsAttribute(UnmanagedType.I4)]  QSPCallback type, IntPtr callback );

        [DllImport("qsplib.dll", EntryPoint = "QSPExecCounter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ExecCounter( bool isRefresh );

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

        public enum QSPCallback
        {
            QSP_CALL_DEBUG, /* void func(const QSP_CHAR *str) */
            QSP_CALL_ISPLAYINGFILE, /* QSP_BOOL func(const QSP_CHAR *file) */
            QSP_CALL_PLAYFILE, /* void func(const QSP_CHAR *file, int volume) */
            QSP_CALL_CLOSEFILE, /* void func(const QSP_CHAR *file) */
            QSP_CALL_SHOWIMAGE, /* void func(const QSP_CHAR *file) */
            QSP_CALL_SHOWWINDOW, /* void func(int type, QSP_BOOL isShow) */
            QSP_CALL_DELETEMENU, /* void func() */
            QSP_CALL_ADDMENUITEM, /* void func(const QSP_CHAR *name, const QSP_CHAR *imgPath) */
            QSP_CALL_SHOWMENU, /* int func() */
            QSP_CALL_SHOWMSGSTR, /* void func(const QSP_CHAR *str) */
            QSP_CALL_REFRESHINT, /* void func(QSP_BOOL isRedraw) */
            QSP_CALL_SETTIMER, /* void func(int msecs) */
            QSP_CALL_SETINPUTSTRTEXT, /* void func(const QSP_CHAR *text) */
            QSP_CALL_SYSTEM, /* void func(const QSP_CHAR *str) */
            QSP_CALL_OPENGAMESTATUS, /* void func(const QSP_CHAR *file) */
            QSP_CALL_SAVEGAMESTATUS, /* void func(const QSP_CHAR *file) */
            QSP_CALL_SLEEP, /* void func(int msecs) */
            QSP_CALL_GETMSCOUNT, /* int func() */
            QSP_CALL_INPUTBOX, /* void func(const QSP_CHAR *text, QSP_CHAR *buffer, int maxLen) */
            QSP_CALL_DUMMY
        };

        public enum QSPWindow
        {
            QSP_WIN_ACTS,
            QSP_WIN_OBJS,
            QSP_WIN_VARS,
            QSP_WIN_INPUT
        };
    }
}