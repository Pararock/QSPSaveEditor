namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class DesignQSPGame: QSPGame
    {
        private List<QSPVariable> _lstVariables;

        public override event PropertyChangedEventHandler PropertyChanged;

        public DesignQSPGame()
        {
        }

        public override string QSPFilePath => @"c:\fakepath\";

        public override Version Version => new Version(@"5.7.0");

        public override DateTime CompiledDate => DateTime.Today;

        public override bool ExecCommand( string command )
        {
            throw new NotImplementedException();
        }

        public void PopulateVariableList()
        {
            var lst = new List<QSPVariable>();

            lst.Add(new QSPVariantVariable("VARIANT", 222, "this is a variant variable"));

            for(int i = 0; i < 5; i++ )
            {
                lst.Add(new QSPVariable("IntVariable" + i, i));
            }

            for ( int i = 0; i < 5; i++ )
            {
                lst.Add(new QSPVariable("StrVariable" + i, "StrVarlue" + i));
            }

            for ( int i = 0; i < 5; i++ )
            {
                lst.Add(new QSPNamedArrayVariable("parentVariable","StrVariable" + i, "StrVarlue" + i));
            }

            lst[0].Value = "6545645";

            lst[6].Value = "ModifiedValue";

            lst[11].Value = "ModifiedValue2";
            lst[1].Value = "asdf"; // illegal value

            _lstVariables = lst;
        }

        public override IEnumerable<QSPVariable> VariablesList
        {
            get
            {
                return _lstVariables;
            }
        }

        public override int MaxVariablesCount => 55485285;

        public override int FullRefreshCount => 5485;

        public override int ActionsCount => 23423;

        public override int ObjectsCount => 1237;

        public override bool IsMainDescriptionChanged => false;

        public override bool IsVarsDescChanged => false;

        public override string CurrentLocation => "CurrentLocation";

        public override string GetMainDesc() => string.Empty;
        public override string GetVarsDesc() => string.Empty;
    }
}
