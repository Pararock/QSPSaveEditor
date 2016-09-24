namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public abstract class QSPGame: INotifyPropertyChanged
    {
        public abstract int FullRefreshCount { get; }
        public abstract int MaxVariablesCount { get; }
        public abstract string QSPFilePath { get; }
        public abstract IList<QSPVariable> VariablesList { get; }
        public abstract DateTime CompiledDate { get; }
        public abstract Version Version { get; }

        public abstract bool ExecCommand(string command);

        public abstract bool RestartWorld(bool refresh);

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public abstract string MainDescription { get; }
        public abstract string VarsDescription { get; }

        public abstract string CurrentLocation { get; }

        public abstract BindingList<QSPObject> ObjectList { get; }
        public abstract BindingList<QSPAction> ActionList { get; }
    }
}
