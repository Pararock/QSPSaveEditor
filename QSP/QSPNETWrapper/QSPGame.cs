namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;

    public abstract class QSPGame
    {
        public abstract string QSPFilePath { get; }
        public abstract Version Version { get; }
        public abstract DateTime CompiledDate { get; }
        public abstract IEnumerable<QSPVariable> VariablesList { get ;}
        public abstract int MaxVariablesCount { get; }
        public abstract int FullRefreshCount { get; }
        public abstract int ActionsCount { get; }
        public abstract int ObjectsCount { get; }
    }
}
