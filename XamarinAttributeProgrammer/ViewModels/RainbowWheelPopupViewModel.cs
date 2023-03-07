using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class RainbowWheelPopupViewModel : BaseViewModel
    {
        private Color _selectedColor;
        private BLEConnections _connections;
        private bool _focusColorOn = ColorSelectionViewModel.BACKLIGHT;
        
        public string OkBtnLbl { get => Resourcer.Ok; }
        public string CancelBtnLbl { get => Resourcer.Cancel; }

        public Color SelectedColor 
        { 
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                Console.WriteLine(_selectedColor.ToString());
                Task delaytask = UpdateGaugeLEDAsync((int)(value.R * 255),
                                (int)(value.G * 255),
                                (int)(value.B * 255));
                OnPropertyChanged();
            }
        }
        public RainbowWheelPopupViewModel(bool focusOn)
        {
            _connections = App._BLEConnection;
            _focusColorOn = focusOn;
        }

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        private async Task<bool> UpdateGaugeLEDAsync(int r, int g, int b)
        {
            // set the value null if the focus is not for it.
            ColorContainer back = _focusColorOn == ColorSelectionViewModel.BACKLIGHT ?
                                        new ColorContainer(r, g, b) : 
                                        null;
            ColorContainer pointer = _focusColorOn == ColorSelectionViewModel.POINTER ?
                                        new ColorContainer(r, g, b) : 
                                        null;

            byte[] cmd = GaugeCommands.ColorPreviewReq(back, pointer);
            await semaphoreSlim.WaitAsync(); // Wait here until locks opens

            try
            {
                if (_connections.CanWrite())
                    return await _connections.TryWriteAsync(cmd);
            }
            catch (Exception ex) when (!(ex is CharacteristicReadException)) // We are ignoring this exception because it's just telling us that the TX channel is flooded, since we're using UDP protocol
            {
                Console.WriteLine("Error in UpdateGaugeLED(): " + ex.Message);
            }
            semaphoreSlim.Release();
            return false;
        }
    }
}