using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;

namespace XamarinAttributeProgrammer.ViewModels
{
    /// <summary>
    /// Author: Tam Nguyen
    /// It's best to update the color by calling the SetBacklightPreviewTo() or pointer version and pass in the RGB
    /// 
    /// </summary>
    public class ColorContainer
    {
        private string _hex;
        public int R = 0;
        public int G = 0;
        public int B = 0;

        public string Hex { 
            get => string.Format("#{0:X2}{1:X2}{2:X2}", R, G, B);
            set => _hex = value;
        }
        public ColorContainer()
        {

        }
        public ColorContainer(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Color GetColor()
        {
            return Color.FromRgb(R, G, B);
        }
    }
    public class ColorSelectionViewModel : BaseViewModel
    {
        public const bool BACKLIGHT = false;
        public const bool POINTER = true;

        public enum RGB {
            RED,
            GREEN,
            BLUE
        }

        private bool _focusColorOn = BACKLIGHT;
        BLEConnections _connection;
        GaugeAttributes _attributes;
        private ColorContainer _colorBack = new ColorContainer(); // change this color via RED/GREEN/BLUEval property 
        private ColorContainer _colorNeedle = new ColorContainer();
        public bool FocusColorOn  //                                             WE ONLY NEED TO CHANGE THIS TO UPDATE EVERYTHING ELSE
        {
            get { return _focusColorOn; }
            set
            {
                _focusColorOn = value;

                // Let all the binded variables know the values is updated
                // and that it needs to update too
                OnPropertyChanged();
                OnPropertyChanged(nameof(NeedleBtnClr));
                OnPropertyChanged(nameof(BacklightBtnClr));
                OnPropertyChanged(nameof(RedVal));
                OnPropertyChanged(nameof(GreenVal));
                OnPropertyChanged(nameof(BlueVal));
            }
        }
        public string FocusSelectionLbl { get => Resourcer.getResStrVal("colorFocusLabel"); }
        public string RedSliderLbl { get => Resourcer.getResStrVal("redSliderLbl"); }
        public string GreenSliderLbl { get => Resourcer.getResStrVal("greenSliderLbl"); }
        public string BlueSliderLbl { get => Resourcer.getResStrVal("blueSliderLbl"); }
        public string PresetSectLbl { get => Resourcer.getResStrVal("presetLbl"); }
        public string BacklightLbl { get => Resourcer.getResStrVal("backlight"); }
        public string NeedleLbl { get => Resourcer.getResStrVal("needle"); }
        public string collapseSectionLbl { get => Resourcer.getResStrVal("collapseSection"); }
        public string minimizeLbl { get => Resourcer.getResStrVal("mini"); }
        public string expandLbl { get => Resourcer.getResStrVal("expand"); }
        public int RedVal
        {
            get => (int)(_focusColorOn == BACKLIGHT ? _colorBack.R : _colorNeedle.R);
            set
            {
                SetColorValue(value, RGB.RED);
            }
        }
        public int GreenVal
        {
            get => (int)(_focusColorOn == BACKLIGHT ? _colorBack.G : _colorNeedle.G);
            set
            {
                SetColorValue(value, RGB.GREEN);
            }
        }
        public int BlueVal
        {
            get => (int)(_focusColorOn == BACKLIGHT ? _colorBack.B : _colorNeedle.B);
            set
            {
                SetColorValue(value, RGB.BLUE);
            }
        }

        public string SelectedColorInHex
        {
            get
            {
                NotifyGaugeOfChangeAsync();
                Task.Delay(50).Wait(); // wait 50ms so that we dont flood the gauge
                if (FocusColorOn == BACKLIGHT)
                    return BackColorInHex;
                else
                    return NeedleColorInHex;
            }
            set
            {
                if (FocusColorOn == BACKLIGHT)
                    _colorBack.Hex = value;
                else
                    _colorNeedle.Hex = value;
            }
        }

        public string NeedleColorInHex {
            get {
                // Since this property is the only proptery for the needle and it's binded,
                // we will call the SetGaugeNeedleTo to update the gauge's LED since this scope will get executed everytime
                // the color is changed
                SetGaugeNeedleTo(_colorNeedle.R, _colorNeedle.G, _colorNeedle.B);// No need to await, since we want it do it in the back
                return _colorNeedle.Hex;
            }
        }
        public string BackColorInHex {
            get {
                // Since this property is the only proptery for the back and it's binded,
                // we will call the SetGaugeNeedleTo to update the gauge's LED since this scope will get executed everytime
                // the color is changed
                SetGaugeBacklightTo(_colorBack.R, _colorBack.G, _colorBack.B); // No need to await, since we want it do it in the back
                return _colorBack.Hex; 
            }
        }


        public Color NeedleBtnClr // the text
        {
            get => _focusColorOn == POINTER ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray"];
        }
        public Color BacklightBtnClr
        {
            get => _focusColorOn == BACKLIGHT ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray"];
        }
        public ColorSelectionViewModel()
        {
            _connection = App._BLEConnection;
            _attributes = App.AttributeManager;
            RetrieveColorFromGauge();
        }

        public void RetrieveColorFromGauge()
        {
            /*
            // Currently on backlight
            SetRGBColorToFocus(_attributes.getBacklightColorRed(),
                          _attributes.getBacklightColorGreen(),
                          _attributes.getBacklightColorBlue());

            // Setup pointer needle color
            FocusColorOn = POINTER;
            SetRGBColorToFocus(_attributes.getPointerColorRed(),
                          _attributes.getPointerColorGreen(),
                          _attributes.getPointerColorBlue());

            FocusColorOn = BACKLIGHT;
            */
        }
        
        public void SetColorValue(int value, RGB col)
        {
            value = Math.Min(Math.Max(value, 0), 255); // Keep the value between 0 and 255
            if (_focusColorOn == BACKLIGHT)
            {
                switch(col)
                {
                    case RGB.RED:
                        _colorBack.R = value;
                        break;
                    case RGB.GREEN:
                        _colorBack.G = value;
                        break;
                    case RGB.BLUE:
                        _colorBack.B = value;
                        break;
                }
            }
            else
            {
                switch (col)
                {
                    case RGB.RED:
                        _colorNeedle.R = value;
                        break;
                    case RGB.GREEN:
                        _colorNeedle.G = value;
                        break;
                    case RGB.BLUE:
                        _colorNeedle.B = value;
                        break;
                }
            }

            OnPropertyChanged(nameof(RedVal));
            OnPropertyChanged(nameof(BlueVal));
            OnPropertyChanged(nameof(GreenVal));

            if (FocusColorOn == BACKLIGHT)
                OnPropertyChanged(nameof(BackColorInHex));
            else
                OnPropertyChanged(nameof(NeedleColorInHex));

            OnPropertyChanged(nameof(SelectedColorInHex));
            //NotifyGaugeOfChangeAsync();
            //Task.Delay(50).Wait(); // wait 50ms so that we dont flood the gauge
        }

        public bool ChangeColorFocusOn(bool val)
        {
            FocusColorOn = val;
            return FocusColorOn;
        }

        public void SetColorTo(Color c)
        {
            RedVal = (int)(c.R * 255);
            GreenVal = (int)(c.G * 255);
            BlueVal = (int)(c.B * 255);
        }
        public void SetRGBColorToFocus(int r, int g, int b)
        {
            if (RedVal == r &&
                GreenVal == g &&
                BlueVal == b)
                return;

            RedVal = r;
            GreenVal = g;
            BlueVal = b;
            OnPropertyChanged(nameof(NeedleColorInHex));
            OnPropertyChanged(nameof(BackColorInHex));
        }

        public async Task SetBacklightPreviewTo(int r, int g, int b)
        {
            if (r == _colorNeedle.R &&
                g == _colorNeedle.G &&
                b == _colorNeedle.B)
                return;

            _colorBack = new ColorContainer(r, g, b);

            if (_focusColorOn == BACKLIGHT)
            {

                OnPropertyChanged(nameof(RedVal));
                OnPropertyChanged(nameof(GreenVal));
                OnPropertyChanged(nameof(BlueVal));
                OnPropertyChanged(nameof(SelectedColorInHex));
            }
            else
            {
                await Task.Delay(50);
                await NotifyGaugeOfChangeAsync();
            }

            OnPropertyChanged(nameof(BackColorInHex));
        }
        public async Task SetPointerPreviewTo(int r, int g, int b)
        {
            if (r == _colorNeedle.R &&
                g == _colorNeedle.G &&
                b == _colorNeedle.B)
                return;

            _colorNeedle = new ColorContainer(r, g, b);

            if (_focusColorOn == POINTER)
            {
                OnPropertyChanged(nameof(RedVal));
                OnPropertyChanged(nameof(GreenVal));
                OnPropertyChanged(nameof(BlueVal));
                OnPropertyChanged(nameof(SelectedColorInHex));
            }
            else
            {
                await Task.Delay(50);
                await NotifyGaugeOfChangeAsync();
            }

            OnPropertyChanged(nameof(NeedleColorInHex));
        }

        public void SetGaugeBacklightTo(int r, int g, int b)
        {
            /*
            _attributes.setBacklightColorRed((short)r);
            _attributes.setBacklightColorGreen((short)g);
            _attributes.setBacklightColorBlue((short)b);

            // collect our mssage to send to the gauge
            //byte[] cmd = GaugeCommands.StringToBytes(_attributes.attributeMessageColorPreviewPackage());
            //return await _connection.TXChannel.WriteAsync(cmd);
            */
        }
        public void SetGaugeNeedleTo(int r, int g, int b)
        {
            _attributes.setPointerColorRed((short)r);
            _attributes.setPointerColorGreen((short)g);
            _attributes.setPointerColorBlue((short)b);

            // collect our mssage to send to the gauge
            //byte[] cmd = GaugeCommands.StringToBytes(_attributes.attributeMessageColorPreviewPackage());
            //return await _connection.TXChannel.WriteAsync(cmd);
        }

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        public async Task<bool> NotifyGaugeOfChangeAsync()
        { 
            await semaphoreSlim.WaitAsync(); // Wait here until locks opens

            byte[] cmd = GaugeCommands.ColorPreviewReq(_colorBack, _colorNeedle);
            bool r = false;

            try
            {
                if (_connection.CanWrite())
                {
                    r = await _connection.TryWriteAsync(cmd);
                    await Task.Delay(50);
                }
            }
            catch (CharacteristicReadException rex) { } // We are ignoring this exception because it's just telling us that the TX channel is flooded, since we're using UDP protocol
            catch (Exception e)
            {
                Console.WriteLine("Error in NotifyGaugeOfChange(): " + e.Message);
            }

            semaphoreSlim.Release();
            
            return r;
        }
    }
}