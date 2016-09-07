
namespace CommandTest
{
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;

    class Program
    {
        static void Main( string[] args )
        {/*

            QSPGameWorld QSP = new QSPGameWorld();

            Console.WriteLine($"QSP Lib version : {QSP.Version}");
            Console.WriteLine($"Compiled on {QSP.CompiledDate}");

            if ( QSP.LoadGameWorld(@"c:\temp\world.qsp") )
            {
                if ( QSP.OpenSavedGame(@"C:\temp\save.sav", true) )
                {

                    Console.WriteLine(QSP.GetMainDesc());

            QSPValue qspVar2;
            QSP.GetVariableValues("MONEY", 0, out qspVar2);
            Console.WriteLine($"{(qspVar2 as QSPNumValue).Value.ToString()}");

            
            var money = QSP.VariablesList.First(var => var.Name == "MONEY");
            Console.WriteLine(money.ToString());
            var money2 = (money as QSPSingleVariable).Value as QSPNumValue;
            money2.Value = 234234;

            var dirtyVar = QSP.VariablesList.Where(var => var.IsDirty).Select(var => var);
            foreach(var variable in dirtyVar)
            {
                Console.WriteLine($"Var {variable.Name} is dirty");
            }


            if ( QSPGameWorld.ExecString("$Glory['GLORY_STAR_ORDER'] = 'sdfsdf'", true) )
            {
                Console.WriteLine("YES");
            }
            else
            {
                Console.WriteLine("Fuck!");
            }

            QSPValue qspVar;
            QSP.GetVariableValues("MONEY", 0, out qspVar);
            Console.WriteLine($"{(qspVar as QSPNumValue).Value.ToString()}");



            QSP.WriteSaveGame(@"C:\temp\save.sav", true);
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
        Console.ReadKey();*/
        }
    }
}
