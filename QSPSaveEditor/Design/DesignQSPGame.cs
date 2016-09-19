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

            lst.Add(new QSPVariable("VARIANT", "this is a variant variable", 222 ));
            lst.Add(new QSPVariable("VARIANT2", "this is a variant variable\r\nwith superlong text\r\n that just keep going and going and going\r\n and going and going and goint", 333));

            for (int i = 0; i < 5; i++ )
            {
                lst.Add(new QSPVariable("IntVariable" + i,string.Empty, i));
            }

            for ( int i = 0; i < 5; i++ )
            {
                lst.Add(new QSPVariable("StrVariable" + i, "StrVarlue" + i, 0));
            }

            for ( int i = 0; i < 2; i++ )
            {
                lst.Add(new QSPNamedArrayVariable("parentVariable","StrVariable" + i, "StrVarlue" + i, 0));
            }

            for ( int i = 0; i < 2; i++ )
            {
                lst.Add(new QSPNamedArrayVariable("parentVariable", "StrVariable" + i, "StrVarlue" + i, i + 1));
            }

            lst[0].StringValue = "6545645";

            lst[6].StringValue = "ModifiedValue";

            lst[1].NewValues(new QSPVariable("IntVariable3", "NewStringValue" ,9999));

            lst[11].StringValue = "ModifiedValue2";

            _lstVariables = lst;
        }

        public override IList<QSPVariable> VariablesList
        {
            get
            {
                if(_lstVariables == null)
                {
                    PopulateVariableList();
                }
                return _lstVariables;
            }
        }

        public override int MaxVariablesCount => 55485285;

        public override int FullRefreshCount => 5485;

        public override string CurrentLocation => "CurrentLocation";

        public override string MainDescription => string.Empty;
        public override string VarsDescription => string.Empty;

        public override BindingList<QSPObject> ObjectList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override BindingList<QSPAction> ActionList
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
