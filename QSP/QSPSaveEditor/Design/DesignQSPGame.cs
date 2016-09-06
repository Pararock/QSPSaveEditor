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
            _lstVariables = new List<QSPVariable>();
            var strValue = new QSPStringValue(0, "TestValue1");
            var singleVariable = new QSPSingleVariable(0, "Variable1", strValue);
            _lstVariables.Add(singleVariable);

            var numValue2 = new QSPNumValue(1,22222);
            var singleVariable2 = new QSPSingleVariable(1, "Variable2", numValue2);
            _lstVariables.Add(singleVariable2);

            var strValue3 = new QSPStringValue(0, "TestValue3");
            var singleVariable3 = new QSPSingleVariable(0, "Variable3-1", strValue3);
            var numValue3 = new QSPNumValue(1, 333333);
            var singleVariable4 = new QSPSingleVariable(1, "Variable3-2", numValue3);

            var lst = new List<QSPSingleVariable>();
            lst.Add(singleVariable3);
            lst.Add(singleVariable4);
            var varArray = new QSPVarArray(2, "Variable3", lst);
            _lstVariables.Add(varArray);

        }

        public override List<QSPVariable> VariablesList
        {
            get
            {
                return _lstVariables;
            }
        }

        public List<QSPVarArray> VariablesList2
        {
            get
            {
                var lstvariables2 = new List<QSPVarArray>();
                var strValue3 = new QSPStringValue(0, "TestValue3");
                var singleVariable3 = new QSPSingleVariable(0, "Variable3-1", strValue3);
                var numValue3 = new QSPNumValue(1, 333333);
                var singleVariable4 = new QSPSingleVariable(1, "Variable3-2", numValue3);
                var lst = new List<QSPSingleVariable>();
                lst.Add(singleVariable3);
                lst.Add(singleVariable4);
                var varArray = new QSPVarArray(2, "Variable3", lst);
                lstvariables2.Add(varArray);

                return lstvariables2;
            }
        }

        public override int MaxVariablesCount => 55485285;

        public override int FullRefreshCount => 5485;

        public override int ActionsCount => 23423;

        public override int ObjectsCount => 1237;
    }
}
