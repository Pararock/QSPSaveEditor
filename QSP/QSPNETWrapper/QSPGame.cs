using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QSPNETWrapper
{
    public class QSPGame
    {
        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QSPInit();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QSPDeInit();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr QSPGetVersion();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr QSPGetCompiledDateTime();

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int QSPGetMaxVarsCount();

        [DllImport("qsplib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QSPLoadGameWorld( [MarshalAsAttribute(UnmanagedType.LPWStr)] String gamePath );

        [DllImport("qsplib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QSPOpenSavedGame( [MarshalAsAttribute(UnmanagedType.LPWStr)] String savePath , [MarshalAsAttribute(UnmanagedType.Bool)] bool isRefresh);

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr QSPGetErrorDesc( [MarshalAsAttribute(UnmanagedType.I4)]  QSPErrorCode error);

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QSPGetLastErrorData( [MarshalAsAttribute(UnmanagedType.I4)] out QSPErrorCode error, ref IntPtr errorLoc, out int errorActIndex, out int errorLine );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QSPGetVarNameByIndex( int ind, ref IntPtr name );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QSPGetVarValuesCount([MarshalAsAttribute(UnmanagedType.LPWStr)] string name, out int count);

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QSPGetVarIndexesCount( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, out int count );

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QSPGetVarValues([MarshalAsAttribute(UnmanagedType.LPWStr)] string name, int ind, out int numVal, ref IntPtr strVal);

        [DllImport("qsplib.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QSPGetVarIndex( [MarshalAsAttribute(UnmanagedType.LPWStr)] string name, int ind, out int numVal, ref IntPtr strVal );
        
        private bool isGameWorldLoaded = false;

        public QSPGame()
        {
            QSPInit();
        }

        public bool LoadGameWorld( string QSPPath )
        {
            isGameWorldLoaded = QSPLoadGameWorld(QSPPath);
            return isGameWorldLoaded;
        }

        public bool OpenSavedGame( string savePath, bool isRefreshed )
        {
            if ( isGameWorldLoaded )
            {
                return QSPOpenSavedGame(savePath, isRefreshed);
            }
            else
            {
                return false;
            }
        }

        public string GetLastError()
        {
            QSPErrorCode error;
            int errorActIndex;
            int errorLine;
            IntPtr ptrError = IntPtr.Zero;
            QSPGetLastErrorData(out error, ref ptrError, out errorActIndex, out errorLine);
            if (ptrError == IntPtr.Zero)
            {
                return GetErrorDesc(error);
            }
            else
            {
                return Marshal.PtrToStringUni(ptrError);
            }            
        }

        public string GetVariableValues( string name , int index )
        {
            int numval;
            IntPtr ptrValue = IntPtr.Zero;
            QSPGetVarValues(name, index, out numval, ref ptrValue);
            return Marshal.PtrToStringUni(ptrValue);
        }

        public string GetVariableIndex( string name, int index )
        {
            int numval;
            IntPtr ptrValue = IntPtr.Zero;
            QSPGetVarIndex(name, index, out numval, ref ptrValue);
            return Marshal.PtrToStringUni(ptrValue);
        }

        public int GetVariableValuesCount(string name)
        {
            int count;
            QSPGetVarValuesCount(name, out count);
            return count;
        }

        public int GetVariableIndexesCount( string name )
        {
            int count;
            QSPGetVarIndexesCount(name, out count);
            return count;
        }

        public string GetVariableNameByIndex(int index)
        {
            IntPtr ptrVariableName = IntPtr.Zero;
            QSPGetVarNameByIndex(index, ref ptrVariableName);
            return Marshal.PtrToStringUni(ptrVariableName);
        }
    
        public int VariablesCount
        {
            get
            {
                return QSPGetMaxVarsCount();
            }
        }

        public string GetErrorDesc(QSPErrorCode error)
        {
            IntPtr errorMsgptr = QSPGetErrorDesc(error);
            return Marshal.PtrToStringUni(errorMsgptr);
        }

        public Version Version
        {
            get
            {
                IntPtr versionpt = QSPGetVersion();
                string version = Marshal.PtrToStringUni(versionpt);
                return Version.Parse(version);
            }
        }
                
        public DateTime CompiledDate
        {
            get
            {
                // Return in format : Jun  6 2010, 23:16:21
                IntPtr compiledDatePtr = QSPGetCompiledDateTime();
                string compiledDate = Marshal.PtrToStringUni(compiledDatePtr);

                DateTime date = DateTime.Parse(compiledDate, new CultureInfo("en-US", false));
                return date;
            }
        }

        ~QSPGame()
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
    }
}
