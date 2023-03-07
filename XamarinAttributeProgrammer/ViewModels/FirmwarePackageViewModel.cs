using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class FirmwarePackageViewModel : Helpers.BindableObject
    {
        #region Singleton

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static FirmwarePackageViewModel Instance => _instance ?? (_instance = new FirmwarePackageViewModel());

        public static FirmwarePackageViewModel _instance;

        #endregion

        public FileInfo SelectedFirmwareFile
        {
            get => GetValue<FileInfo>();
            set
            {
                if (SetValue(value))
                {
                    DfuInstallationConfigurationPageViewModel.Instance.DfuInstallation.FileUrl = value?.FullName;
                }
            }
        }

        public async Task<int> PickFileAsync()
        {
            try
            {
                var options = new PickOptions
                {
                    PickerTitle = "Please select a zip file",
                };
                var result = await FilePicker.PickAsync(options);
                if (result.ContentType == "application/zip")
                {
                    var newFilePath = await CopyFile(result);
                    if (!string.IsNullOrEmpty(newFilePath))
                        SelectedFirmwareFile = new FileInfo(newFilePath);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 2;
                // The user canceled or something went wrong
            }
        }

        private async Task<string> CopyFile(FileResult fileResult)
        {
            if (await Permissions.CheckStatusAsync<Permissions.StorageWrite>() != PermissionStatus.Granted)
                if (await Permissions.RequestAsync<Permissions.StorageWrite>() != PermissionStatus.Granted)
                    return null;

            var outputFilePath = Path.Combine(FileSystem.AppDataDirectory, fileResult.FileName);
            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            using (var fileStream = new FileStream(outputFilePath, FileMode.Create))
            using (var stream = await fileResult.OpenReadAsync())
            {
                for (var i = 0; i < stream.Length; i++)
                {
                    fileStream.WriteByte((byte) stream.ReadByte());
                }

                fileStream.Close();
                stream.Close();
            }

            return outputFilePath;
        }
    }
}