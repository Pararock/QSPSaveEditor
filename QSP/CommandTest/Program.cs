
namespace CommandTest
{
    using QSPNETWrapper;
    using System;

    class Program
    {
        static void Main( string[] args )
        {

            QSPGameWorld QSP = new QSPGameWorld();

            Console.WriteLine($"QSP Lib version : {QSP.Version}");
            Console.WriteLine($"Compiled on {QSP.CompiledDate}");

            if ( QSP.LoadGameWorld(@"c:\temp\world.qsp") )
            {
                if ( QSP.OpenSavedGame(@"C:\temp\save.sav", true) )
                {
                    foreach(QSPBaseVariable var in QSP.VariablesList)
                    {
                        Console.WriteLine(var.ToString());
                    }
                }
            }
            else
            {
                Console.WriteLine("cant load");
            }

            Console.ReadKey();
        }
    }
}
