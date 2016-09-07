namespace QSPNETWrapper
{
    using System;
    using System.Collections.Generic;

    public abstract class QSPGame
    {
        public abstract int ActionsCount { get; }
        public abstract DateTime CompiledDate { get; }
        public abstract int FullRefreshCount { get; }
        public abstract int MaxVariablesCount { get; }
        public abstract int ObjectsCount { get; }
        public abstract string QSPFilePath { get; }
        public abstract IEnumerable<QSPBaseVariable> VariablesList { get; }
        public abstract Version Version { get; }
    }
}
