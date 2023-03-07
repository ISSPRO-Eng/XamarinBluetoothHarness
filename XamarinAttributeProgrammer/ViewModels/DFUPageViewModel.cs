using System;
using XamarinAttributeProgrammer.Helpers;
using XamarinAttributeProgrammer.Views;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class DFUPageViewModel : BindableObject
    {
        #region Singleton

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static DFUPageViewModel Instance => _instance ?? (_instance = new DFUPageViewModel());

        public static DFUPageViewModel _instance;

        #endregion

        public bool HasStarted
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }


        public void Start()
        {
            try
            {
                DfuInstallationConfigurationPageViewModel.Instance.DfuInstallation.Start();
            }
            catch (Exception ex)
            {
            }
        }

    }
}