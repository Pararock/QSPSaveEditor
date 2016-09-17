
namespace CommandTest
{
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;

    class Program
    {

        static void Main( string[] args)
        {

            QSPGameWorld QSP = new QSPGameWorld();

            Console.WriteLine($"QSP Lib version : {QSP.Version}");
            Console.WriteLine($"Compiled on {QSP.CompiledDate}");

            if ( QSP.LoadGameWorld(@"c:\temp\test2.qsp") )
            {
                if ( QSP.OpenSavedGame(@"C:\temp\test2.sav", true) )
                {
                    /*foreach(QSPVariable var in QSP.VariablesList)
                    {
                        Console.WriteLine(var.ToString());
                    }*/

                    /*if ( QSP.RestartWorld(true) )
                    {
                        string location;
                        int index;
                        int line;
                        QSP.GetCurrentStateData(out location, out index, out line);
                        Console.WriteLine($"{location}{index}{line}");
                    }*/

                   Console.WriteLine($"{QSP.LocationsCount}");

                    for ( int i = 0; i < QSP.LocationsCount; i++ )
                    {
                        Console.WriteLine($"{QSP.GetLocationName(i)}");
                    }

                    foreach(var variable in QSP.ActionList)
                    {
                        Console.WriteLine($"{variable.Index} - {variable.ImagePath} - {variable.Description}");
                    }

                    foreach ( var variable in QSP.ObjectList )
                    {
                        Console.WriteLine($"{variable.Index} - {variable.ImagePath} - {variable.Description}");
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
