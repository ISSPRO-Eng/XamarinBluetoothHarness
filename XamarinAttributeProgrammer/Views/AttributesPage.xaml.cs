using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.HelperClass;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;
using XamarinAttributeProgrammer.ViewModels;
using Plugin.BLE.Abstractions.Exceptions;

/// <summary>
/// Author: Tam Nguyen
/// Date of comment: 10/19/2021
/// 
/// Attribute page (3rd tab) allow the users to customize their EV3 gauges settings
/// to fit their vehicle's requirement. On this page, the gauge's type and mode,
/// along with the attributes, are all adjustable with data validation to reduce
/// user prone errors.
/// </summary>

namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttributesPage : ContentPage
    {
        AttributesViewModel vm;
        GaugeAttributes _att;
        BLEConnections _connection;
        string previousConnectionGuid = null;
        bool _loadedAtt = false;
        bool _changeDetected = false;
        bool _speedoTach = false;
        bool _alreadyShowAdvanceWarning = false;


        // Following two enums are from the Attribute.h in the executable
        enum GaugeType
        {
            ATT_GAUGE_4kTACH_80mphSPEEDO,
            ATT_GAUGE_4kTACH_120mphSPEEDO,
            ATT_GAUGE_6kTACH_120mphSPEEDO,
            ATT_GAUGE_8kTACH_120mphSPEEDO,
            ATT_GAUGE_8kTACH_160mphSPEEDO,
            ATT_GAUGE_3kTACH_200kmSPEEDO,
            ATT_GAUGE_6kTACH_200kmSPEEDO,
            ATT_GAUGE_8kTACH_200kmSPEEDO,
            ATT_GAUGE_30psiBOOST_1600PYRO,
            ATT_GAUGE_40psiBOOST_1600PYRO,
            ATT_GAUGE_60psiBOOST_2000YRO,
            ATT_GAUGE_2barBOOST_900PYRO,
            ATT_GAUGE_DUAL_1600PYRO,
            ATT_GAUGE_DEF_FUEL,
            ATT_GAUGE_TEMP_FUEL,
            ATT_GAUGE_TEMP_PRESSURE_FUEL_VOLTS,
            ATT_GAUGE_DEMO_2_1,
            ATT_GAUGE_DEMO_4_1,
            //  Add new gauge here. Keep the above's order!
            ATT_GAUGE_NONE = 65535
        }
        enum GaugeMode
        {
            ATT_GAUGE_MODE_NORMAL = 0,
            ATT_GAUGE_MODE_TEST,
            ATT_GAUGE_MODE_SWEEP_DEMO,
            ATT_GAUGE_MODE_WARN_DEMO,
            ATT_GAUGE_MODE_MAX_SIZE
        }
        public AttributesPage()
        {
            InitializeComponent();
            BindingContext = vm = new AttributesViewModel();  // Binding this view to the viewmodel for all bindings on the xaml
            _att = App.AttributeManager;
            _connection = App._BLEConnection;


            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // On iOS, SelectedIndexChanged is triggered when the item change but is still in focused (haven't clicked done/ok yet), but on Android it only fire
                // after the user hit done/ok (get out of focus). So, we're removing the event trigger if we're on iOS and set the trigger to fire
                // when it lost focus (tap out or done).
                TypeCombo.SelectedIndexChanged -= GaugeType_SelectedAsync;
                TypeCombo.Unfocused += GaugeType_UnfocusedAsync;
            }
        }


        /// <summary>
        /// Function to remove items from the picker that is not made for the specific gauge.
        /// </summary>
        /// <param name="typeCombo"> the picker </param>
        /// <param name="gaugeType"> A list of type:pcb# (key:pair) </param>
        private void FilterTypeList(Picker typeCombo, IList<KeyValuePair<string, int>> gaugeType)
        {
            var itemsource = TypeCombo.ItemsSource;
            if (!_connection.IsConnected)
            {
                // Check to see if the app is connected to the device and it we have a copy of the
                // gauge's attributes. If we don't, see if the picker already display the whole list
                // before updating it to display the whole list (possibly again).
                if (itemsource == null || itemsource.Count < gaugeType.Count)
                    typeCombo.ItemsSource = gaugeType.Select(x => x.Key).ToList(); // can't use itemsource to update since it's a copy of the actual itemsource
                return;
            }

            int pcb = 255;
            // Show all gauge types since we do not utilize an ADC for pcb selections
            List<string> types = gaugeType.Where(x => x.Value == pcb).Select(x => x.Key).ToList();
            types.Add(gaugeType[gaugeType.Count - 1].Key); // we want to add the last item, which is NONE

            typeCombo.ItemsSource = types;
        }
        
        // Event handler when the the page is in focus (such as switch back to this page)
        protected override void OnAppearing()
        {
            DiagnosticPage.AddToLog("I: Entering Attribute Pages");

            // We want to check to see if this page had already load the attribute of the connected
            // gauge, and if not then load the attribute.
            if (!_loadedAtt)
            {
                FillEntry();
                _loadedAtt = true;
            }
        }
        
        void EnableTypeEvent()
        {
            // Disable the event when the gauge type is selected
            // so that it doesnt request the user when they want to retrieve the 
            // att from the gauge, then we enable (via this) it again after it's loaded

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                TypeCombo.Unfocused += GaugeType_UnfocusedAsync;
            }
            else
                TypeCombo.Unfocused += GaugeType_SelectedAsync;
        }

        // As the title said, this is to fill out all the boxes on the page
        private void FillEntry(bool clear = false, bool? skipPickerUpdate = false)
        {
            
            if (_att == null)
                return;
               

            // If clear is false, check to see if the attribute manager
            // has the attributes from the gauges. If it doesn't then enable the clear
            // clear flag, since there is nothing to load from
            if (clear == false)
                clear = !_att.hasAttributes;

            if (!skipPickerUpdate.HasValue || skipPickerUpdate.Value == false)
            {
                FilterTypeList(TypeCombo, vm.GaugeType);

                int type = Math.Min(_att.getGaugeType(), vm.GaugeType.Count -1); // we dont want it to go out of bound

                // Check to see if a Type was assigned, and if that value is in our combo box's list
                if (type >= 0 && (!clear || _connection.IsConnected))
                {
                    string lookFor = vm.GaugeType[type].Key;
                    for (int i = 0; i < TypeCombo.Items.Count; i++)
                    {
                        string item = TypeCombo.Items[i];
                        if (lookFor.Equals(item))
                        {
                            TypeCombo.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }


            // Shared
            voltHi.Text = clear ? "18.1" : _att.getGaugeBacklightTopVoltage().ToString();
            voltLo.Text = clear ? "7.8" : _att.getGaugeBacklightBotVoltage().ToString();
            dimmerInput.SelectedIndex = clear ? 0 : _att.getBacklightInput();
            warningLightFlash.SelectedIndex = clear ? 0 : _att.getBacklightFlash();
            maxBrightness.Text = clear ? "100" : _att.getMaxBrightness().ToString();
            daytimeBrightness.Text = clear ? "0" : _att.getDaytimeBrightness().ToString();

            // Quad Selected = 0
            quadSelection.SelectedIndex = 0;

            // Quad 0
            quad0WarnZone.SelectedIndex = clear ? 0 : _att.getQuad0GaugeWarningZone();
            quad0WarnLevel.Text = clear ? "0" : _att.getQuad0GaugeWarningLevel().ToString();
            quad0PtrWeight.SelectedIndex = clear ? 0 : _att.getQuad0GaugePointerWeight();
            quad0InputSource.SelectedIndex = clear ? 0 : _att.getQuad0SensorType();
            quad0Hys.Text = clear ? "2" : _att.getQuad0GaugeHysteresis().ToString();

            // Driver
            outputZone.SelectedIndex = clear ? 0 : _att.getGaugeOutputZone();
            outputThres.Text = clear ? "0" : _att.getGaugeOutputLevel().ToString();
            outputQuad.SelectedIndex = clear ? 0 : _att.getGaugeOutputQuad();

            // Advanced
            curveQuad.SelectedIndex = clear ? 0 : _att.getCoeffQuad();
            memorySlot.SelectedIndex = clear ? 0 : _att.getNvmPair();
            sensCoeff0.Text = clear ? "0" : _att.getGaugeCoefficient0().ToString("0.0000000000");
            sensCoeff1.Text = clear ? "0" : _att.getGaugeCoefficient1().ToString("0.0000000000");

            // SPEEDO/TACH
            enableAccumulation.SelectedIndex = clear ? 0 : _att.getTotalAccumulationEnabled();
            distanceUnits.SelectedIndex = clear ? 0 : _att.getUnits();
            speedoPPM.Text = clear ? "4000" : _att.getSpeedoPPM().ToString();
            tachPPR.Text = clear ? "25" : _att.getTachPPR().ToString();

            if (!clear || _connection.IsConnected)
            {
                previousConnectionGuid = _att.getGUID().ToString();
                _loadedAtt = true;
            }
            _changeDetected = vm.Haschanges = false;
            
        }

        private bool DoesFormContainsNull()
        {
            int gauge = _att.getGaugeType();
            bool answer = string.IsNullOrEmpty(voltHi.Text)         || string.IsNullOrEmpty(voltLo.Text) ||
                          string.IsNullOrEmpty(daytimeBrightness.Text) || string.IsNullOrEmpty(maxBrightness.Text) ||
                          string.IsNullOrEmpty(outputThres.Text) ||

                          string.IsNullOrEmpty(quad0Hys.Text) || string.IsNullOrEmpty(quad0WarnLevel.Text) ||
                          string.IsNullOrEmpty(sensCoeff0.Text)     || string.IsNullOrEmpty(sensCoeff1.Text);
            // if Speedo, do not have empty PPM value
            if (gauge == 24)
            {
                answer |= string.IsNullOrEmpty(speedoPPM.Text);
            }
            // if Tachometer, do not have empty PPR
            if(gauge == 25)
            {
                answer |= string.IsNullOrEmpty(tachPPR.Text);
            }
            return answer;
        }

        // Function made to collect all the inputs and update our current attribute profile
        private bool CollectEntry()
        {
            if (DoesFormContainsNull())
                return false;

            int type = 0; 
        // To confirm
            for (int i = 0; i < vm.GaugeType.Count; i++)
            {
                if (vm.GaugeType[i].Key.Equals(TypeCombo.SelectedItem))
                {
                    type = i;
                    _att.setGaugeType(type); // Get our selected item and look in vm.GaugeType for matching key, then use the value as our type
                    break;
                }
            }
            _att.setGaugeBacklightTopVoltage(float.Parse(voltHi.Text));
            _att.setGaugeBacklightBotVoltage(float.Parse(voltLo.Text));
            _att.setBacklightInput(Convert.ToByte(dimmerInput.SelectedIndex));
            _att.setBacklightFlash(Convert.ToByte(warningLightFlash.SelectedIndex));
            _att.setDaytimeBrightness(Convert.ToByte(maxBrightness.Text));
            _att.setMaxBrightness(Convert.ToByte(daytimeBrightness.Text));

            _att.setQuad0GaugeWarningLevel(Convert.ToByte(quad0WarnLevel.Text));
            _att.setQuad0GaugeWarningZone(Convert.ToByte(quad0WarnZone.SelectedIndex));
            _att.setQuad0GaugePointerWeight(Convert.ToByte(quad0PtrWeight.SelectedIndex));
            _att.setQuad0SensorType(Convert.ToByte(quad0InputSource.SelectedIndex));
            _att.setQuad0GaugeHysteresis(Convert.ToByte(quad0Hys.Text));
            _att.setTachSensor(Convert.ToByte(tachHallEffect.SelectedIndex));

            _att.setGaugeOutputLevel(Convert.ToByte(outputThres.Text));
            _att.setGaugeOutputZone(Convert.ToByte(outputZone.SelectedIndex));
            _att.setGaugeOutputQuad(Convert.ToByte(outputQuad.SelectedIndex));

            _att.setCoeffQuad(Convert.ToByte(curveQuad.SelectedIndex));
            _att.setNvmPair(Convert.ToByte(memorySlot.SelectedIndex));
            _att.setGaugeCoefficient0(Convert.ToSingle(sensCoeff0.Text));
            _att.setGaugeCoefficient1(Convert.ToSingle(sensCoeff1.Text));

            //Speedo/Tach
            _att.setTotalAccumulationEnabled(Convert.ToByte(enableAccumulation.SelectedIndex));
            _att.setUnits(Convert.ToByte(distanceUnits.SelectedIndex));
            _att.setSpeedoPPM(Convert.ToInt16(speedoPPM.Text));
            _att.setTachPPR(Convert.ToByte(tachPPR.Text));
            
            return true;
            
        }

        /// <summary>
        /// This is the Save/Cancel function. To flash the gauge with new settings,
        /// the applications will generate 6 packets and send each one at a time.
        /// To ensure the data are properly sent, we want to implement a threadlock
        /// (semaphore) to force the packets to take turn so that it acts more like
        /// a TCP than UDP.
        /// </summary>
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        private async void SaveCancelBtns_ClickedAsync(object sender, EventArgs e)
        {
            Button sndr = sender as Button;

            switch (sndr.ClassId)
            {
                case "R": // Hard factory reset
                    try
                    {
                        await ResetForm(factoryReset: true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        DiagnosticPage.AddToLog("ESC: " + ex.Message);
                    }
                    break;
                case "C": // Cancel
                    try
                    {
                        TypeCombo.SelectedIndexChanged -= GaugeType_SelectedAsync;
                        await ResetForm();
                        EnableTypeEvent();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        DiagnosticPage.AddToLog("ESC: " + ex.Message);
                    }
                    break;
                case "S": // SAVE
                    if (!CollectEntry())
                    {
                        await DisplayAlert(Resourcer.Error, 
                                            Resourcer.getResStrVal("errorEmptyField"), 
                                            Resourcer.Ok);
                        return;
                    }
                    
                    // Generate our packets for all of our attributes
                    byte[][] cmds = GaugeCommands.FlashCommand();

                    try
                    {
                        Thread.Sleep(100); // Avoids GATT error being thrown
                        DiagnosticPage.AddToLog($"I: Saving new attribute(s) to {_connection.DeviceName}");
                        BLEConnections connection = App._BLEConnection;
                        int i = 0;
                        foreach (var cmd in cmds)
                        {
                            Console.WriteLine($"{i}: {BitConverter.ToString(cmd)}");
                            await semaphoreSlim.WaitAsync(); // Wait here until locks opens
                            await connection.TryWriteAsync(cmd);
                            semaphoreSlim.Release();
                        }

                        await DisplayAlert(Resourcer.Success,
                                                Resourcer.getResStrVal("flashedSuccessMsg"),
                                                Resourcer.Ok);

                        DiagnosticPage.AddToLog("I: New Attributes profile:\n" + _att.GetAttributeLog());
                    }

                    catch (CharacteristicReadException rex) {
                        Console.WriteLine("Error: " + rex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        DiagnosticPage.AddToLog("ESC: " + ex.Message);
                    }
                    finally
                    {
                        if (semaphoreSlim.CurrentCount == 0)
                        {
                            // If there is no available threads, unlock all
                            semaphoreSlim.Release();
                        }
                    }

                    vm.Haschanges = _changeDetected = false;
                    break;
            }
        }

        /// <summary>
        /// Event handler for when the text inside the entryfields changed
        /// Since all the entry fields on the form are numerical input, make sure to include the ClassID property
        /// if you want to include a MAX value. This is the most efficient way i found to do a range of 0-x
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// </summary>
        ///
        private void EntryText_ChangedPercentage(object sender, EventArgs e)
        {

            Entry sndr = sender as Entry;
            float max = 0;
            float min = 0;

            // Check to see if we have given this entry a max (classId) range for numeric inputs
            if (string.IsNullOrEmpty(sndr.ClassId)) return;

            // Try to parse that value if it does exist
            if (!float.TryParse(sndr.ClassId, out max))
            {
                // If parsing failed, check to see if we're looking for float max
                if (sndr.ClassId.Equals("FM")) // float max
                {
                    max = float.MaxValue;
                    min = float.MinValue;
                }
                else
                {
                    // If the input is not parseable as float and not looking for max value, then something is wrong
                    DiagnosticPage.AddToLog($"EETC: Unknown class ID {sndr.ClassId} when parsing acceptable range in attribute page's Entry field");

                    DisplayAlert(Resourcer.Error,
                                Resourcer.getResStrVal("parseError"),
                                Resourcer.Ok);

                    sndr.Focus(); // set the focus back since this triggered when focus is lost
                    return;
                }
            }

        }
        private void EntryText_Changed(object sender, EventArgs e)
        {

            Entry sndr = sender as Entry;
            float max = 0;
            float min = 0;

            // Check to see if we have given this entry a max (classId) range for numeric inputs
            if (string.IsNullOrEmpty(sndr.ClassId)) return;

            // Try to parse that value if it does exist
            if (!float.TryParse(sndr.ClassId, out max))
            {
                // If parsing failed, check to see if we're looking for float max
                if (sndr.ClassId.Equals("FM")) // float max
                {
                    max = float.MaxValue;
                    min = float.MinValue;
                }
                else
                {
                    // If the input is not parseable as float and not looking for max value, then something is wrong
                    DiagnosticPage.AddToLog($"EETC: Unknown class ID {sndr.ClassId} when parsing acceptable range in attribute page's Entry field");

                    DisplayAlert(Resourcer.Error,
                                Resourcer.getResStrVal("parseError"),
                                Resourcer.Ok);

                    sndr.Focus(); // set the focus back since this triggered when focus is lost
                    return;
                }
            }

            float value;
            float.TryParse(sndr.Text, out value);

            value = Math.Max(min, value);
            value = Math.Min(max, value);
            sndr.Text = value.ToString();

            _changeDetected = vm.Haschanges = true;

        }

        private void EntryTextNoDecimals_Changed(object sender, EventArgs e)
        {
            // Let this function process our input to be a valid numeric input
            EntryText_Changed(sender, e);

            // from there, we'll get the processed text and round it to the nearest whole number
            Entry entry = sender as Entry;
            int value = (int)Math.Round(float.Parse(entry.Text));
            entry.Text = value.ToString();
        }

        private void ComboBox_Changed(object sender, EventArgs e)
        {
            // If the value in the combo boxes changed,
            // then set the flag to true
            vm.Haschanges = _changeDetected = true;
        }

        // For our iOS unfocus event. Only for iOs
        private void GaugeType_UnfocusedAsync(object sender, FocusEventArgs e)
        {
            try
            {
                GaugeType_SelectedAsync(sender, e);
            }
            catch (Exception ex)
            {
                DiagnosticPage.AddToLog("EGTU: " + ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        private void GaugeMode_Selected(object sender, EventArgs e)
        {
            Picker sndr = sender as Picker;

            if (!_loadedAtt) return; // don't bother with this yet when we first enter;

            //_att.setGaugeMode(Convert.ToByte(sndr.SelectedIndex));
            ComboBox_Changed(sender, e);
        }

        /// <summary>
        /// This function is triggered when the Gauge Type combo box has a value changed.
        /// It takes the new value, prompt the user, and (if accepted) loads the type's
        /// profile.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GaugeType_SelectedAsync(object sender, EventArgs e)
        {
            Picker sndr = sender as Picker;


            if (!_loadedAtt)
                return; // don't bother with this function yet when we first enter;

            // Get the selected value, if for some reason the obj is null, assign NA as value
            string itemSelected = sndr?.SelectedItem?.ToString() ?? "<NA>";

            // Check if selected item is null, or if gauge is none
            if (sndr == null || 
                sndr.SelectedItem == null ||
                itemSelected.Equals("GAUGE NONE"))
                return;

            // Check with the user if they want to load the profile of the Type they've selected
            //bool confirm = await DisplayAlert(Resourcer.Warning,
            //                                   string.Format(Resourcer.getResStrVal("loadProfile"), itemSelected),
             ///                                  Resourcer.Ok, Resourcer.Cancel);

           // if (!confirm) // User do not want to load the profile
            //{
             //   Toast.ShowMessage(this, "Action canceled.");
             //   return;
            //}

            // Since we're only letting the user select specific type, the order will be wrong,
            // so we want to get the correct order by seeing where the index of the selected device is.
            int val = vm.GaugeType.Select(x => x.Key).ToList().IndexOf(sndr.SelectedItem.ToString());

            // Check to see if the index is set first
            GaugeType gaugeType = (GaugeType)(val >= 0 && val <= 18 ?   val : // we want to ignore the last index "Gauge None"
                                                                       65535);
            // If the gauge does not have any attributes, just use factory defaults
            if (!_att.hasAttributes)
                _att.ResetFactoryDefaults();
            // Set common Attributes for all gauges
            _att.setGaugeBacklightTopVoltage(18.1f);
            _att.setGaugeBacklightBotVoltage(7.8f);
            _att.setBacklightInput(0);
            _att.setBacklightFlash(0);
            _att.setMaxBrightness(100);
            _att.setDaytimeBrightness(0);

            // Switch copied from AP V1.x
            switch (gaugeType)
            {
                case GaugeType.ATT_GAUGE_4kTACH_80mphSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_4kTACH_120mphSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_6kTACH_120mphSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_8kTACH_120mphSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_8kTACH_160mphSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_3kTACH_200kmSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_6kTACH_200kmSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_8kTACH_200kmSPEEDO:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_30psiBOOST_1600PYRO:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_40psiBOOST_1600PYRO:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_60psiBOOST_2000YRO:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_2barBOOST_900PYRO:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_DUAL_1600PYRO:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_DEF_FUEL:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_TEMP_FUEL:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_TEMP_PRESSURE_FUEL_VOLTS:
                    _speedoTach = vm.Speedotach = false;
                    break;
                case GaugeType.ATT_GAUGE_DEMO_2_1:
                    _speedoTach = vm.Speedotach = true;
                    break;
                case GaugeType.ATT_GAUGE_DEMO_4_1:
                    _speedoTach = vm.Speedotach = false;
                    break;
            }
            /*
                case GaugeType.ATT_GAUGE_PRI_AIR_PRESS_175: // Air Pres 0-175
                    _att.setGaugeFull(175);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(500);
                    _att.setGaugeMinReading(-50);
                    _att.setGaugePointerWeight(4000);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(11);
                    break;
                case GaugeType.ATT_GAUGE_REAR_AXLE_TEMP_100_280: // Rear Axle Temp 100-280
                case GaugeType.ATT_GAUGE_TRANS_OIL_TEMP_100_280: // Trans oil Temp 100-280
                case GaugeType.ATT_GAUGE_COOL_TEMP_100_280: // cool Temp 100-280
                    _att.setGaugeFull(280);
                    _att.setGaugeHome(100);
                    _att.setGaugeMaxReading(500);
                    _att.setGaugeMinReading(-50);
                    _att.setGaugePointerWeight(4000);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(4);
                    break;
                case GaugeType.ATT_GAUGE_EXHAUST_BACK_PRESS_60: // Exhaust Pressure 0-60 PSI
                    _att.setGaugeFull(60);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(120);
                    _att.setGaugeMinReading(-10);
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(9);
                    break;
                case GaugeType.ATT_GAUGE_EXHAUST_BACK_PRESS_100: // Exhaust Pressure 0-100 PSI
                case GaugeType.ATT_GAUGE_FUEL_PRESS_100_100: // Fuel Pressure 100 psi
                    _att.setGaugeFull(100);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150); //500
                    _att.setGaugeMinReading(-10); //-50
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(10);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_LEVEL_STEWART_WARNER: //Fuel level 242-30 ohms
                    _att.setGaugeFull(1);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(0); //TODO: test this out on next rev of boards
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(1);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_LEVEL_0_30: //Fuel level 0 - 30 ohms
                    _att.setGaugeFull(1);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(0); //TODO: test this out on next rev of boards
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(17);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_LEVEL_0_90: //Fuel level 0 - 90 ohms
                    _att.setGaugeFull(1);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(0); //TODO: test this out on next rev of boards
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(18);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_LEVEL_10_180: //Fuel level 10 - 180 ohms
                    _att.setGaugeFull(1);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(0); //TODO: test this out on next rev of boards
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(19);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_LEVEL_78_10: //Fuel level 78 - 10 ohms
                    _att.setGaugeFull(1);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(0); //TODO: test this out on next rev of boards
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(20);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_LEVEL_10_158: //Fuel level 78 - 10 ohms
                    _att.setGaugeFull(1);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(0); //TODO: test this out on next rev of boards
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(21);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_PRESS_30_30: // Fuel Pressure 30 psi (30 psi sensor)
                //No differences than Fuel Pressure 30_100
                //just follow the same case statement below
                case GaugeType.ATT_GAUGE_FUEL_PRESS_100_30: // Fuel Pressure 30 psi (100 psi sensor)
                    _att.setGaugeFull(30);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(-10);
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135);
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve((short)(gaugeTye == GaugeType.ATT_GAUGE_FUEL_PRESS_30_30 ? 6 : 7));
                    break;
                case GaugeType.ATT_GAUGE_FUEL_PRESS_100_40: // Fuel Pressure 40 psi
                    _att.setGaugeFull(40);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(-10);
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(135);
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(8);
                    break;
                case GaugeType.ATT_GAUGE_FUEL_RAIL_PRESS_29k_30k: // Fuel Rail Pressure(0-30000psi) </item>
                    _att.setGaugeFull(30000);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(32000);
                    _att.setGaugeMinReading(-2000);
                    _att.setGaugePointerWeight(400);
                    _att.setGaugeSweep(261 / 2); //TODO: CHECK ON THIS
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(15);
                    break;
                case GaugeType.ATT_GAUGE_OIL_PRESS_100: // Oil Pressure(0-100psi)</item>
                    _att.setGaugeFull(100);
                    _att.setGaugeHome(1);
                    _att.setGaugeMaxReading(150);
                    _att.setGaugeMinReading(-40);
                    _att.setGaugePointerWeight(4000);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(10);
                    break;
                case GaugeType.ATT_GAUGE_TURBO_BOOST_40: //Boost 40 gauge
                    _att.setGaugeFull(40);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(200);
                    _att.setGaugeMinReading(-10);
                    _att.setGaugePointerWeight(4000);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(2);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(8);
                    break;
                case GaugeType.ATT_GAUGE_TURBO_BOOST_60: //Boost 60 Gauge
                    _att.setGaugeFull(60);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(200);
                    _att.setGaugeMinReading(-10);
                    _att.setGaugePointerWeight(4000);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(2);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(9);
                    break;
                case GaugeType.ATT_GAUGE_TURBO_BOOST_100://Boost 100 gauge
                    _att.setGaugeFull(100);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(200);
                    _att.setGaugeMinReading(-10);
                    _att.setGaugePointerWeight(4000);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(2);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(10);
                    break;
                case GaugeType.ATT_GAUGE_VOLT_18V: // Voltmeter</item>
                    _att.setGaugeFull(16);
                    _att.setGaugeHome(8);
                    _att.setGaugeMaxReading(18);
                    _att.setGaugeMinReading(1);
                    _att.setGaugePointerWeight(800);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeCoefficient0(0.0f);
                    _att.setGaugeCoefficient1(0.004667295f);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(2);
                    break;
                case GaugeType.ATT_GAUGE_VOLT_36V: // Voltmeter</item>
                    _att.setGaugeFull(32);
                    _att.setGaugeHome(16);
                    _att.setGaugeMaxReading(36);
                    _att.setGaugeMinReading(1);
                    _att.setGaugePointerWeight(800);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeCoefficient0(0.0f);
                    _att.setGaugeCoefficient1(0.009249442f);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(2);
                    break;
                case GaugeType.ATT_GAUGE_PYRO_1600: // PYRO 1600
                    _att.setGaugeFull(1600);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(1800);
                    _att.setGaugeMinReading(-100);
                    _att.setGaugePointerWeight(2500);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(3);
                    break;
                case GaugeType.ATT_GAUGE_PYRO_2000: // PYRO 2000
                    _att.setGaugeFull(2000);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(2200);
                    _att.setGaugeMinReading(-100);
                    _att.setGaugePointerWeight(2500);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(3);
                    break;
                case GaugeType.ATT_GAUGE_SPEEDO: // SPEEDOMETER
                    _att.setGaugeFull(80);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(200);
                    _att.setGaugeMinReading(-100);
                    _att.setGaugePointerWeight(2500);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(0);
                    _att.setSpeedoPPM(8000);
                    _att.setUnits(1);
                    _att.setTotalAccumulationEnabled(1);
                    _att.setSpeedoSensor(0);
                    _att.setTripEnabled(1);
                    _att.setSpeedoTachOutput(0);
                    break;
                case GaugeType.ATT_GAUGE_TACH: // TACHOMETER
                    _att.setGaugeFull(3000);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(12000);
                    _att.setGaugeMinReading(-100);
                    _att.setGaugePointerWeight(2500);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(0);
                    _att.setTachPPR(25);
                    _att.setPPRprecision(0);
                    _att.setUnits(1);
                    _att.setTotalAccumulationEnabled(1);
                    _att.setTripEnabled(1);
                    _att.setSpeedoTachOutput(0);
                    break;
                case GaugeType.ATT_GAUGE_GPS_SPEEDO: // GPS SPEEDOMETER
                    _att.setGaugeFull(80);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(200);
                    _att.setGaugeMinReading(-100);
                    _att.setGaugePointerWeight(2500);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugeHysteresis(3);
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(0);
                    _att.setUnits(1);
                    _att.setTotalAccumulationEnabled(1);
                    _att.setTripEnabled(1);
                    _att.setSpeedoTachOutput(0);
                    break;
                case GaugeType.ATT_GAUGE_DEMO: //Demo Case
                    _att.setGaugeFull(3000);
                    _att.setGaugeHome(0);
                    _att.setGaugeMaxReading(3200);
                    _att.setGaugeMinReading(0);
                    _att.setGaugePointerWeight(800);
                    _att.setGaugeSweep(135); //in 2 degree units
                    _att.setGaugePointerType(0);
                    _att.setGaugeSensorScanRate(100);
                    _att.setGaugeMode(0);
                    _att.setGaugeSensorCurve(0);
                    break;
                case GaugeType.ATT_GAUGE_NONE: //invalid gauge (Title bar place)
                default:
                    break;
            }
            
            if (gaugeTye == GaugeType.ATT_GAUGE_DEMO)
                _att.setGaugeMode(1);
            else
                _att.setGaugeMode(0);
            */
            FillEntry(skipPickerUpdate: true);
            ComboBox_Changed(sender, e);
            
        }


        private async void OnExpander_TappedAsync(object sender, EventArgs e)
        {
            // We only want to show it if this is the first time showing it
            // and if the user is EXPANDING it.
            if (!_alreadyShowAdvanceWarning && 
                fineTuneExpander.State == Xamarin.CommunityToolkit.UI.Views.ExpandState.Expanding)
            {
                await DisplayAlert(Resourcer.Warning, Resourcer.getResStrVal("attributeAdvanceWarning"), Resourcer.Ok);
                _alreadyShowAdvanceWarning = true;
            }
            
        }

        // Reset the page to display the attribute that is on the gauge
        private async Task ResetForm(bool factoryReset = false)
        {
            if (_loadedAtt)
            {
                bool answer;
                if (factoryReset)
                {
                    answer = await DisplayAlert(Resourcer.Warning,
                                                Resourcer.getResStrVal("factoryResetWarning"),
                                                Resourcer.Ok, Resourcer.Cancel);
                    if (answer)
                    {
                        // If user wants to factory reset

                        DiagnosticPage.AddToLog("I: Resetting gauge to factory mode.");

                        _att.ResetFactoryDefaults();
                        // This is only part of the factory reset,
                        // the other part is done when gauge type is selected

                        Toast.ShowMessage(this, "Please select a gauge type");
                    }
                }
                else
                {
                    answer = await DisplayAlert(Resourcer.Warning,
                                                    Resourcer.getResStrVal("resetMsg"),
                                                    Resourcer.Ok, Resourcer.Cancel);


                    if (answer)
                    {

                        DiagnosticPage.AddToLog("I: Resetting attribute page to attribute stored on gauge.");
                        await _connection.TryWriteAsync(GaugeCommands.ATTRS);

                        _att.hasAttributes = true;
                    }
                }

                FillEntry(); // reload the form
                _loadedAtt = true;
            }
            else
            {
                // No diaglog (Maybe change) since the gauge isn't connected or have any stored value
                FillEntry(clear: true);
            }
            _changeDetected = false;
            vm.Haschanges = false;
        }
        
    }
}