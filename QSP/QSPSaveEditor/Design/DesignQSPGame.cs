namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;

    public class DesignQSPGame: QSPGame
    {
        private List<QSPVariable> _lstVariables;

        public DesignQSPGame()
        {
        }

        public override string QSPFilePath => @"c:\fakepath\";

        public override Version Version => new Version(@"5.7.0");

        public override DateTime CompiledDate => DateTime.Today;

        public void PopulateVariableList()
        {
            var lst = new List<QSPVariable>();
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
                lst.Add(new QSPArrayVariable("parentVariable","StrVariable" + i, "StrVarlue" + i));
            }

            lst[0].Value = "6545645";

            lst[6].Value = "ModifiedValue";

            lst[11].Value = "ModifiedValue2";

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
    }
}
