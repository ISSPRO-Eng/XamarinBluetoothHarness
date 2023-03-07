using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Views;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class DiagnosticViewModel : BaseViewModel
    {
        public string ClearTxt { get => Resourcer.getResStrVal("clear"); }
        public string CopyTxt { get => Resourcer.getResStrVal("copy"); }
        public string Log
        {
            get => DiagnosticPage._log;
        }
        public DiagnosticViewModel()
        {
                
        }

        public void UpdateLog()
        {
            OnPropertyChanged(Log);
        }
    }
}
