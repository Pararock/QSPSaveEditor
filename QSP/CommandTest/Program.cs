using QSPNETWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest
{
    class Program
    {
        static void Main( string[] args )
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            QSPGame QSP = new QSPGame();

            Console.WriteLine($"QSP Lib version : {QSP.Version}");
            Console.WriteLine($"Compiled on {QSP.CompiledDate}");

            if (QSP.LoadGameWorld(@"qspfile.qsp") )
            {
                if( QSP.OpenSavedGame(@"savefile.sav", true) )
                {
                    Console.WriteLine($"Load sucessful \n Variables: {QSP.VariablesCount}");
                    for ( int i = 0; i < QSP.VariablesCount; i++ )
                    {
                        string name = QSP.GetVariableNameByIndex(i);
                        if ( !string.IsNullOrEmpty(name) )
                        {
                            int valueCount = QSP.GetVariableValuesCount(name);
                            int indexCount = QSP.GetVariableIndexesCount(name);
                            Console.WriteLine($"Variable {i} named {name} have a value count of {valueCount} and index count: {indexCount}");
                            if(indexCount == 0)
                            {
                                Console.WriteLine($"\t{QSP.GetVariableValues(name, 0)}");
                            }
                            else
                            {
                                for ( int j = 0; j < valueCount; j++ )
                                {
                                    Console.WriteLine($"\tValue ({j}): {QSP.GetVariableIndex(name, j)} => {QSP.GetVariableValues(name, j)}");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine(QSP.GetLastError());
                }
            }
            else
            {
                Console.WriteLine(QSP.GetLastError());
            }
            Console.ReadKey();
        }
    }
}
