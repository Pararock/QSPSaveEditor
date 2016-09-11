namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public abstract class QSPGame: INotifyPropertyChanged
    {
        public abstract int ActionsCount { get; }
        public abstract int FullRefreshCount { get; }
        public abstract int MaxVariablesCount { get; }
        public abstract int ObjectsCount { get; }
        public abstract string QSPFilePath { get; }
        public abstract IEnumerable<QSPVariable> VariablesList { get; }
        public abstract DateTime CompiledDate { get; }
        public abstract Version Version { get; }
        public abstract bool IsMainDescriptionChanged { get; }
        public abstract bool IsVarsDescChanged { get; }

        public abstract bool ExecCommand(string command);

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public abstract string MainDescription { get; }
        public abstract string VarsDescription { get; }

        public abstract string CurrentLocation { get; }
    }
}
