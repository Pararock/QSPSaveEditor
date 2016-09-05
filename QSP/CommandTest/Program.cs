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

            if ( QSP.LoadGameWorld(@"c:\temp\world.qsp") )
            {
                if ( QSP.OpenSavedGame(@"C:\temp\save.sav", true) )
                {
                    foreach(QSPVariable variable in QSP.VariablesList)
                    {
                        Console.WriteLine($"{variable.Name}");
                        if(variable is QSPSingleVariable)
                        {
                            Console.WriteLine($"\t Value : {(variable as QSPSingleVariable).Value}");
                        } else if(variable is QSPVarArray)
                        {
                            QSPVarArray varArray = variable as QSPVarArray;
                            foreach(QSPSingleVariable value in varArray.Values )
                            {
                                Console.WriteLine($"\t {value.Name} => {value.Value}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("WTF!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("cant load");
                }
            }
            else
            {
                Console.WriteLine("cant open qsp");
            }
            Console.ReadKey();
        }
    }
}
