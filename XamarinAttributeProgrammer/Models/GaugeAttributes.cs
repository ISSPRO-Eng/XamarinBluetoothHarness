using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Extensibility;
using XamarinAttributeProgrammer.ViewModels;

namespace XamarinAttributeProgrammer.Models
{
    // Class copied straight from the Android studio file.
    public class GaugeAttributes
    {

       
       // LOOK AT GaugeCommands.cs to learn more about the packets the commands fall into
        public bool hasAttributes = false;
        public readonly int a = 1; //not sure yet how to do this
        private readonly float[] ISSPRO_PCA_TYPES = {  0.0f,                         // ISSPRO_PCA_165S
                                        (float)(3.6 * (10000) / (10000 + 4700 )),    // ISSPRO_PCA_165S_001
                                        (float)(3.6 * (10000) / (10000 + 10000)),    // ISSPRO_PCA_165S_002
                                        (float)(3.6 * (10000) / (10000 + 100000)),   // ISSPRO_PCA_165S_003
                                        (float) 3.6,                                 // ISSPRO_PCA_165S_004
                                        (float)(3.6 * (4700)  / (10000 + 4700)),     // ISSPRO_PCA_165S_005
                                        (float)(3.6 * (100000)/ (10000 + 100000)),   // ISSPRO_PCA_165S_006
                                        (float)(3.6 * (200000)/ (10000 + 200000)),   // ISSPRO_PCA_165S_007
                                        (float) (3.6 * (100000)/ (100000 + 20000)),  // ISSPRO_PCA_176S
                                        (float) (3.6 * (54900)/ (54900 + 200000)),   // ISSPRO_PCA_182S
                                        (float) (3.6 * (4700)/ (4700 + 20000)),      // ISSPRO_PCA_182S_001
                                        (float) (3.6 * (10000)/ (10000 + 20000)),    // ISSPRO_PCA_182S_002
                                        (float) (3.6 * (20000)/ (20000 + 4700)) };   // ISSPRO_PCA_182S_003



    private readonly float ISSPRO_PCA_THRESHOLD = 0.1f;
        /* Connect Fragment Variables*/
        private static string device_Name;
        private static string device_version;
        private static string device_SN;
        private static int device_PCB_Type;
        private static Guid device_BTGUID;

        /* Color Editor Fragment Variables*/
        private static short RGBW_backlight_red;
        private static short RGBW_backlight_green;
        private static short RGBW_backlight_blue;
        private static short RGBW_backlight_white;
        private static short RGBW_pointer_red;
        private static short RGBW_pointer_green;
        private static short RGBW_pointer_blue;
        private static short RGBW_pointer_white;

        /* Attributes Fragment Variables*/
        private static int gauge_type; //This is now size short (16-bits; but need larger bits to store as unsigned)
        private static int gauge_home;
        private static int gauge_full;
        private static short gauge_sweep; //Note: Any Byte that goes over 127 needs to be stored as short to avoid unsigned issues
        private static short gauge_min_valid_reading;
        private static short gauge_max_valid_reading;
        private static short gauge_sensor_curve;
        private static short gauge_pointer_weight;
        private static short gauge_hysteresis; //Note: Any Byte that goes over 127 needs to be stored as short to avoid unsigned issues
        private static short gauge_sensor_scan_rate;
        private static float gauge_coefficient0 = 0.0000000000f; //0.0f = single precision float
        private static float gauge_coefficient1 = 0.0000000000f;
        private static byte gauge_mode;
        private static byte gauge_pointer_type;
        private static byte gauge_backlight_Flash_intensity;
        private static byte gauge_backlight_Flash_level;
        private static byte gauge_backlight_Flash_zone;
        private static float gauge_backlight_top_voltage; // When sending to gauge, we need to multiply by 10 and cast it to int
        private static float gauge_backlight_bot_voltage; // so that we can send it as a short, since it only allocates 2 byte for it
        private static byte gauge_warning_bot_level;
        private static byte gauge_warning_bot_zone;
        private static byte gauge_warning_top_level;
        private static byte gauge_warning_top_zone;
        private static byte gauge_output_bot_level;
        private static byte gauge_output_bot_zone;
        private static byte gauge_output_top_level;
        private static byte gauge_output_top_zone;
        private static byte gauge_output_startup_delay;
        private static byte gauge_output_activation_delay;
        private static short gauge_backlight_scan_rate;

        // Speedo/Tach Settings
        private static byte enable_accumulation;
        private static byte speedo_tach_units;
        private static byte speedo_sensor;
        private static byte speedo_tach_trip_enable;
        private static byte tach_ppr_precision;
        private static byte speedo_tach_output;
        private static short speedo_PPM;
        private static byte tach_PPR;
        private static float lcd_total_accumulation = 0.0000000000f;

        public GaugeAttributes()
        {
            init();
        }
        /* Default initalizer*/
        public void init()
        {
            device_Name = "new name";
            device_version = "00.00.00";
            device_PCB_Type = -4095; //put the value way out of range
            device_SN = "0x00000000";
            device_version = "00.00.00";
            device_BTGUID = new Guid();
            RGBW_backlight_red = 0;
            RGBW_backlight_green = 0;
            RGBW_backlight_blue = 0;
            RGBW_backlight_white = 0;
            RGBW_pointer_red = 0;
            RGBW_pointer_green = 0;
            RGBW_pointer_blue = 0;
            RGBW_pointer_white = 0;

            /* Attributes Fragment Variables*/
            gauge_type = 65535; //invalidate this until its set, note -1 is also 255 in byte format
            gauge_home = 0;
            gauge_full = 0;
            gauge_sweep = 0;
            gauge_min_valid_reading = 0;
            gauge_max_valid_reading = 0;
            gauge_sensor_curve = 0;
            gauge_pointer_weight = 0;
            gauge_hysteresis = 0;
            gauge_sensor_scan_rate = 30; //in milliseconds
            gauge_backlight_scan_rate = 100; // mSec
            gauge_coefficient0 = 0;
            gauge_coefficient1 = 0;
            gauge_mode = 0;
            gauge_pointer_type = 0;
            gauge_backlight_Flash_intensity = 0;
            gauge_backlight_Flash_level = 0;
            gauge_backlight_Flash_zone = 0; // Toggle for low/high
            gauge_backlight_top_voltage = 18.1f;
            gauge_backlight_bot_voltage = 7.8f;
            gauge_warning_bot_level = 0;
            gauge_warning_bot_zone = 0; // Toggle for low/high
            gauge_warning_top_level = 0;
            gauge_warning_top_zone = 0; // Toggle for low/high
            gauge_output_bot_level = 0;
            gauge_output_bot_zone = 0; // Toggle for low/high
            gauge_output_top_level = 0;
            gauge_output_top_zone = 0; // Toggle for low/high
            gauge_output_startup_delay = 0;
            gauge_output_activation_delay = 0;
            enable_accumulation = 0; // toggle enabled/disabled
            speedo_tach_units = 0; // toggle miles/km
            speedo_sensor = 0; // toggle enabled/disabled
            speedo_tach_trip_enable = 0; // toggle enabled/disabled
            tach_ppr_precision = 0; // toggle float/integer
            speedo_tach_output = 0; // toggle normal/one time/repeating
            speedo_PPM = 0;
            tach_PPR = 0;
            lcd_total_accumulation = 0;
    }

        public void ResetFactoryDefaults()
        {
            // Reset all the attributes that all the gauge types share in common.
            // The rest of the attributes at set when the user specified the gauge type in AttributesPage.xaml.cs > GaugeType_SelectedAsync()
            gauge_backlight_scan_rate = 100; // mSec
            gauge_coefficient0 = 0;
            gauge_coefficient1 = 0;
            gauge_pointer_type = 0;
            gauge_backlight_Flash_intensity = 0;
            gauge_backlight_Flash_level = 0;
            gauge_backlight_Flash_zone = 0; // Toggle for low/high
            gauge_backlight_top_voltage = 18.1f;
            gauge_backlight_bot_voltage = 7.8f;
            gauge_warning_bot_level = 0;
            gauge_warning_bot_zone = 0; // Toggle for low/high
            gauge_warning_top_level = 0;
            gauge_warning_top_zone = 0; // Toggle for low/high
            gauge_output_bot_level = 0;
            gauge_output_bot_zone = 0; // Toggle for low/high
            gauge_output_top_level = 0;
            gauge_output_top_zone = 0; // Toggle for low/high
            gauge_output_startup_delay = 0;
            gauge_output_activation_delay = 0;
            enable_accumulation = 0; // toggle enabled/disabled
            speedo_tach_units = 0; // toggle miles/km
            speedo_sensor = 0; // toggle enabled/disabled
            speedo_tach_trip_enable = 0; // toggle enabled/disabled
            tach_ppr_precision = 0; // toggle float/integer
            speedo_tach_output = 0; // toggle normal/one time/repeating
            speedo_PPM = 0;
            tach_PPR = 0;

            hasAttributes = true;
        }

        /* Connect Fragment Methods*/
        public string getDeviceName()
        {
            return device_Name;
        }
        public void setDeviceName(string name)
        {
            device_Name = name;
        }
        public string getDeviceVersion()
        {
            return device_version;
        }
        public void setDeviceVersion(string name)
        {
            device_version = name;
        }
        public void setDevice_SN(string SN)
        {
            device_SN = SN;
        }
        public string getDevice_SN()
        {
            return device_SN;
        }
        public void setDevice_PCB_Type(short pcb)
        {
            device_PCB_Type = (int)pcb;
        }
        public int getDevice_PCB_Type()
        { //TODO: varify this works properly
          //quick range check ; incase we were a negative value
            device_PCB_Type = device_PCB_Type > 4095 ? 0 : device_PCB_Type; //TODO: check on this
            float mBoard = (float)(device_PCB_Type * 3.6) / 4095;

            for (int i = 0; i < ISSPRO_PCA_TYPES.Length; i++)
            {
                if (Math.Abs(mBoard - ISSPRO_PCA_TYPES[i]) < ISSPRO_PCA_THRESHOLD)
                {
                    //we have a match
                    return i;
                }

            }
            return 255; //error here
        }

        public void setGUID(Guid id)
        {
            device_BTGUID = id;
        }
        public Guid getGUID()
        {
            return device_BTGUID;
        }

        /* Color Fragment Methods*/
        public void setBacklightColorRed(short val)
        {
            RGBW_backlight_red = val;
        }
        public void setPointerColorRed(short val)
        {
            RGBW_pointer_red = val;
        }
        public short getBacklightColorRed()
        {
            return RGBW_backlight_red;
        }
        public short getPointerColorRed()
        {
            return RGBW_pointer_red;
        }

        public void setBacklightColorGreen(short val)
        {
            RGBW_backlight_green = val;
        }
        public void setPointerColorGreen(short val)
        {
            RGBW_pointer_green = val;
        }
        public short getBacklightColorGreen()
        {
            return RGBW_backlight_green;
        }
        public short getPointerColorGreen()
        {
            return RGBW_pointer_green;
        }
        public void setBacklightColorBlue(short val)
        {
            RGBW_backlight_blue = val;
        }
        public void setPointerColorBlue(short val)
        {
            RGBW_pointer_blue = val;
        }
        public short getBacklightColorBlue()
        {
            return RGBW_backlight_blue;
        }
        public short getPointerColorBlue()
        {
            return RGBW_pointer_blue;
        }
        public void setBacklightColorWhite(short val)
        {
            RGBW_backlight_white = val;
        }
        public void setPointerColorWhite(short val)
        {
            RGBW_pointer_white = val;
        }
        public short getBacklightColorWhite()
        {
            return RGBW_backlight_white;
        }
        public short getPointerColorWhite()
        {
            return RGBW_pointer_white;
        }

        public void SetBackLightColor(short? r, short? g, short? b, short? w)
        {
            if (!w.HasValue && r.HasValue && g.HasValue && b.HasValue)
            {
                setBacklightColorRed(r.Value);
                setBacklightColorGreen(g.Value);
                setBacklightColorBlue(b.Value);
            }
            else
            {
                if (r.HasValue) setBacklightColorRed((short)Math.Min(r.Value + w.Value, 255));
                if (g.HasValue) setBacklightColorGreen((short)Math.Min(g.Value + w.Value, 255));
                if (b.HasValue) setBacklightColorBlue((short)Math.Min(b.Value + w.Value, 255));
            }
            setBacklightColorWhite(0);
        }

        public void SetPointerColor(short? r, short? g, short? b, short? w)
        {
            if (!w.HasValue && r.HasValue && g.HasValue && b.HasValue)
            {

                setPointerColorRed(r.Value);
                setPointerColorGreen(g.Value);
                setPointerColorBlue(b.Value);
            }
            else
            {
                if (r.HasValue) setPointerColorRed((short)Math.Min(r.Value + w.Value, 255));
                if (g.HasValue) setPointerColorGreen((short)Math.Min(g.Value + w.Value, 255));
                if (b.HasValue) setPointerColorBlue((short)Math.Min(b.Value + w.Value, 255));
            }
            if (w.HasValue) setPointerColorWhite(0);
        }

        /* Attributes Fragment Methods*/
        public void setGaugeType(int val)
        {
            gauge_type = (short)val;
        }
        public int getGaugeType()
        {
            return gauge_type;
        }

        public void setGaugeHome(int val)
        {
            gauge_home = val;
        }
        public int getGaugeHome()
        {
            return gauge_home;
        }

        public void setGaugeFull(int val)
        {
            gauge_full = val;
        }
        public int getGaugeFull()
        {
            return gauge_full;
        }

        public void setGaugeSweep(short val)
        {
            gauge_sweep = val;
        }
        short getGaugeSweep()
        {
            return gauge_sweep;
        }

        public void setGaugeMinReading(int val)
        {
            gauge_min_valid_reading = (short)val;
        }
        int getGaugeMinReading()
        {
            return gauge_min_valid_reading;
        }

        public void setGaugeMaxReading(int val)
        {
            gauge_max_valid_reading = (short)val;
        }
        int getGaugeMaxReading()
        {
            return gauge_max_valid_reading;
        }

        public void setGaugeSensorCurve(short val)
        {
            gauge_sensor_curve = val;
        }
        short getGaugeSensorCurve()
        {
            return gauge_sensor_curve;
        }

        public void setGaugePointerWeight(short val)
        {
            gauge_pointer_weight = val;
        }
        public short getGaugePointerWeight()
        {
            return gauge_pointer_weight;
        }

        public void setGaugeHysteresis(short val)
        {
            gauge_hysteresis = val;
        }
        public short getGaugeHysteresis()
        {
            return gauge_hysteresis;
        }

        public void setGaugeSensorScanRate(short val)
        {
            gauge_sensor_scan_rate = val;
        }
        public short getGaugeSensorScanRate()
        {
            return gauge_sensor_scan_rate;
        }
        public void setGaugeBacklightScanRate(short val)
        {
            gauge_backlight_scan_rate = val;
        }
        public short getGaugeBacklightScanRate()
        {
            return gauge_backlight_scan_rate;
        }

        public void setGaugeCoefficient0(float val)
        {
            gauge_coefficient0 = 0.0000000000f + val;
        }
        public float getGaugeCoefficient0()
        {
            return gauge_coefficient0;
        }

        public void setGaugeCoefficient1(float val)
        {
            gauge_coefficient1 = 0.0000000000f + val;
        }
        public float getGaugeCoefficient1()
        {
            return gauge_coefficient1;
        }

        public void setGaugeMode(byte val)
        {
            gauge_mode = val;
        }
        public byte getGaugeMode()
        {
            return gauge_mode;
        }

        public void setGaugePointerType(byte val)
        {
            gauge_pointer_type = val;
        }
        public byte getGaugePointerType()
        {
            return gauge_pointer_type;
        }

        public void setGaugeBacklightFlashIntensity(byte val)
        {
            gauge_backlight_Flash_intensity = val;
        }
        public byte getGaugeBacklightFlashIntensity()
        {
            return gauge_backlight_Flash_intensity;
        }

        public void setGaugeBacklightLevel(byte val)
        {
            gauge_backlight_Flash_level = val;
        }
        public byte getGaugeBacklightLevel()
        {
            return gauge_backlight_Flash_level;
        }

        public void setGaugeBacklightZone(byte val)
        {
            gauge_backlight_Flash_zone = val;
        }
        public byte getGaugeBacklightZone()
        {
            return gauge_backlight_Flash_zone;
        }

        public void setGaugeBacklightTopVoltage(float val)
        {
            gauge_backlight_top_voltage = val;
        }
        public float getGaugeBacklightTopVoltage()
        {
            return gauge_backlight_top_voltage;
        }

        public void setGaugeBacklightBotVoltage(float val)
        {
            gauge_backlight_bot_voltage = val;
        }
        public float getGaugeBacklightBotVoltage()
        {
            return gauge_backlight_bot_voltage;
        }

        public void setGaugeWarningBotLevel(byte val)
        {
            gauge_warning_bot_level = val;
        }
        public byte getGaugeWarningBotLevel()
        {
            return gauge_warning_bot_level;
        }

        public void setGaugeWarningBotZone(byte val)
        {
            gauge_warning_bot_zone = val;
        }
        public byte getGaugeWarningBotZone()
        {
            return gauge_warning_bot_zone;
        }

        public void setGaugeWarningTopLevel(byte val)
        {
            gauge_warning_top_level = val;
        }
        public byte getGaugeWarningTopLevel()
        {
            return gauge_warning_top_level;
        }

        public void setGaugeWarningTopZone(byte val)
        {
            gauge_warning_top_zone = val;
        }
        public byte getGaugeWarningTopZone()
        {
            return gauge_warning_top_zone;
        }

        public void setGaugeOutputBotLevel(byte val)
        {
            gauge_output_bot_level = val;
        }
        public byte getGaugeOutputBotLevel()
        {
            return gauge_output_bot_level;
        }

        public void setGaugeOutputBotZone(byte val)
        {
            gauge_output_bot_zone = val;
        }
        public byte getGaugeOutputBotZone()
        {
            return gauge_output_bot_zone;
        }

        public void setGaugeOutputTopLevel(byte val)
        {
            gauge_output_top_level = val;
        }
        public byte getGaugeOutputTopLevel()
        {
            return gauge_output_top_level;
        }

        public void setGaugeOutputTopZone(byte val)
        {
            gauge_output_top_zone = val;
        }
        public byte getGaugeOutputTopZone()
        {
            return gauge_output_top_zone;
        }

        public void setGaugeOutputStartupDelay(byte val)
        {
            gauge_output_startup_delay = val;
        }
        public byte getGaugeOutputStartupDelay()
        {
            return gauge_output_startup_delay;
        }

        public void setGaugeOutputActivationDelay(byte val)
        {
            gauge_output_activation_delay = val;
        }
        public byte getGaugeOutputActivationDelay()
        {
            return gauge_output_activation_delay;
        }
        public void setTotalAccumulationEnabled(byte val)
        {
            enable_accumulation = val;
        }
        public byte getTotalAccumulationEnabled()
        {
            return enable_accumulation;
        }
        public void setUnits(byte val)
        {
            speedo_tach_units = val;
        }
        public byte getUnits()
        {
            return speedo_tach_units;
        }
        public void setSpeedoSensor(byte val)
        {
            speedo_sensor= val;
        }
        public byte getSpeedoSensor()
        {
            return speedo_sensor;
        }
        public void setTripEnabled(byte val)
        {
            speedo_tach_trip_enable = val;
        }
        public byte getTripEnabled()
        {
            return speedo_tach_trip_enable;
        }
        public void setPPRprecision(byte val)
        {
            tach_ppr_precision = val;
        }
        public byte getPPRprecision()
        {
            return tach_ppr_precision;
        }
        public void setSpeedoTachOutput(byte val)
        {
            speedo_tach_output = val;
        }
        public byte getSpeedoTachOutput()
        {
            return speedo_tach_output;
        }
        public void setSpeedoPPM(short val)
        {
            speedo_PPM = val;
        }
        public short getSpeedoPPM()
        {
            return speedo_PPM;
        }
        public void setTachPPR(byte val)
        {
            tach_PPR = val;
        }
        public byte getTachPPR()
        {
            return tach_PPR;
        }
        public void setTotalAccum(float val)
        {
            lcd_total_accumulation = 0.0000000000f + val;
        }
        public float getTotalAccum()
        {
            return lcd_total_accumulation;
        }
        public void LoadAttFrom(GaugeAttributes profile)
        {
            setGaugeBacklightTopVoltage(profile.getGaugeBacklightTopVoltage());
            setGaugeBacklightBotVoltage(profile.getGaugeBacklightBotVoltage());
            setGaugeWarningTopLevel(profile.getGaugeWarningTopLevel());
            setGaugeWarningTopZone(profile.getGaugeWarningTopZone());
            setGaugeWarningBotLevel(profile.getGaugeWarningBotLevel());
            setGaugeWarningBotZone(profile.getGaugeWarningBotZone());
            setGaugeBacklightLevel(profile.getGaugeBacklightLevel());

            setGaugeBacklightFlashIntensity(profile.getGaugeBacklightFlashIntensity());
            setGaugeOutputTopLevel(profile.getGaugeOutputTopLevel());
            setGaugeOutputTopZone(profile.getGaugeOutputTopZone());
            setGaugeOutputBotLevel(profile.getGaugeOutputBotLevel());
            setGaugeOutputBotZone(profile.getGaugeOutputBotZone());
            setGaugeOutputActivationDelay(profile.getGaugeOutputActivationDelay());
            setGaugeOutputStartupDelay(profile.getGaugeOutputStartupDelay());

            setGaugePointerWeight(profile.getGaugePointerWeight());
            setGaugeHysteresis(profile.getGaugeHysteresis());
            setGaugeCoefficient0(profile.getGaugeCoefficient0());
            setGaugeCoefficient1(profile.getGaugeCoefficient1());
            setGaugeSensorScanRate(profile.getGaugeSensorScanRate());
            setGaugeBacklightScanRate(profile.getGaugeBacklightScanRate());
            setTotalAccumulationEnabled(profile.getTotalAccumulationEnabled());
            setUnits(profile.getUnits());
            setSpeedoSensor(profile.getSpeedoSensor());
            setTripEnabled(profile.getTripEnabled());
            setPPRprecision(profile.getPPRprecision());
            setSpeedoTachOutput(profile.getSpeedoTachOutput());
            setSpeedoPPM(profile.getSpeedoPPM());
            setTachPPR(profile.getTachPPR());
            setTotalAccum(profile.getTotalAccum());

        }
        /*
                Navgation panel load/save/send methods
             */
        public void loadPacket(string packet, string type)
        {
            int mixer;
            switch (type)
            {
                case "0x1":
                    //NOTE: These have issues when converting data that is unsigned over to signed, to work around this
                    //      the values will be fully converted into long before being reduced to proper size.

                    //Update from Xamarin: very losely saying radix 16 just mean we're expecting a hexadecimal.
                    //We'll be keeping long.parse because we don't know if the error occurs with c#
                    Console.WriteLine($"Packet Length: {packet.Length}\t PacketMsg: {packet}");
                    Console.WriteLine(SafeSubstring(packet,0, 4));
                    Console.WriteLine(SafeSubstring(packet,4, 8));
                    Console.WriteLine(SafeSubstring(packet,8, 12));
                    Console.WriteLine(SafeSubstring(packet,12, 16));
                    gauge_type = (int)ExtractFromPacketLong(packet,0, 4);
                    gauge_home = (int)ExtractFromPacketLong(packet,4, 8);
                    gauge_full = (int)ExtractFromPacketLong(packet,8, 12);
                    gauge_min_valid_reading = (short)ExtractFromPacketLong(packet,12, 16);
                    hasAttributes = true;
                    break;
                case "0x2":
                    gauge_max_valid_reading = (short)ExtractFromPacketLong(packet,0, 4);
                    gauge_sensor_curve = (byte)ExtractFromPacketLong(packet,4, 6);
                    gauge_pointer_weight = (short)ExtractFromPacketLong(packet,6, 10);
                    gauge_hysteresis = (short)ExtractFromPacketLong(packet,10, 12);
                    gauge_sensor_scan_rate = (short)ExtractFromPacketLong(packet,12, 14);
                    gauge_sweep = (short)ExtractFromPacketLong(packet,14, 16);
                    hasAttributes = true;
                    break;
                case "0x3":
                    mixer = (int)ExtractFromPacketLong(packet,0, 2);
                    gauge_mode = (byte)((mixer & 0xF0) >> 4);
                    gauge_pointer_type = (byte)((mixer & 0xc) >> 2);
                    gauge_backlight_Flash_intensity = (byte)(mixer & 0x3);
                    mixer = (int)ExtractFromPacketLong(packet,2, 4);
                    gauge_backlight_Flash_level = (byte)((mixer & 0xFE) >> 1);
                    gauge_backlight_Flash_zone = (byte)(mixer & 1);
                    gauge_backlight_top_voltage = (int)ExtractFromPacketLong(packet,4, 8) / 10.0f; // our data is a short, so we need to divide by 10 to get decimals
                    gauge_backlight_bot_voltage = (int)ExtractFromPacketLong(packet,8, 12) / 10.0f;
                    mixer = (int)ExtractFromPacketLong(packet,12, 14);
                    gauge_warning_bot_level = (byte)((mixer & 0xFE) >> 1);
                    gauge_warning_bot_zone = (byte)(mixer & 0x1);
                    mixer = (int)ExtractFromPacketLong(packet,14, 16);
                    gauge_warning_top_level = (byte)((mixer & 0xFE) >> 1);
                    gauge_warning_top_zone = (byte)(mixer & 0x1);
                    hasAttributes = true;
                    break;
                case "0x4":
                    RGBW_backlight_red = (short)ExtractFromPacketLong(packet,0, 2);
                    RGBW_backlight_green = (short)ExtractFromPacketLong(packet,2, 4);
                    RGBW_backlight_blue = (short)ExtractFromPacketLong(packet,4, 6);
                    RGBW_backlight_white = (short)ExtractFromPacketLong(packet,6, 8);
                    RGBW_pointer_red = (short)ExtractFromPacketLong(packet,8, 10);
                    RGBW_pointer_green = (short)ExtractFromPacketLong(packet,10, 12);
                    RGBW_pointer_blue = (short)ExtractFromPacketLong(packet,12, 14);
                    RGBW_pointer_white = (short)ExtractFromPacketLong(packet,14, 16);
                    hasAttributes = true;
                    break;
                case "0x5":
                    // Converting the values from int to float... Not sure why but the original Android app does it.
                    mixer = (int)ExtractFromPacketLong(packet,0, 8);
                    byte[] bytesForConversion = BitConverter.GetBytes(mixer);

                    gauge_coefficient0 = BitConverter.ToSingle(bytesForConversion, 0);

                    mixer = (int)ExtractFromPacketLong(packet,8, 16);
                    bytesForConversion = BitConverter.GetBytes(mixer);

                    gauge_coefficient1 = BitConverter.ToSingle(bytesForConversion, 0);
                    hasAttributes = true;
                    break;
                case "0x6":
                    mixer = (int)ExtractFromPacketLong(packet,0, 2);
                    gauge_output_startup_delay = (byte)((mixer & 0xC0) >> 6);
                    gauge_output_activation_delay = (byte)(mixer & 0x3F);
                    mixer = (int)ExtractFromPacketLong(packet,2, 4);
                    gauge_output_bot_level = (byte)((mixer & 0xFE) >> 1);
                    gauge_output_bot_zone = (byte)(mixer & 0x1);
                    mixer = (int)ExtractFromPacketLong(packet,4, 6);
                    gauge_output_top_level = (byte)((mixer & 0xFE) >> 1);
                    gauge_output_top_zone = (byte)(mixer & 0x1);
                    gauge_backlight_scan_rate = (short)ExtractFromPacketLong(packet,6, 8);
                    hasAttributes = true;
                    break;
                case "0x7":
                    mixer = (int)ExtractFromPacketLong(packet, 0, 8);
                    bytesForConversion = BitConverter.GetBytes(mixer);
                    lcd_total_accumulation = BitConverter.ToSingle(bytesForConversion, 0);

                    mixer = (int)ExtractFromPacketLong(packet, 8, 10);
                    enable_accumulation = (byte)((mixer & 0x80) >> 7);
                    speedo_tach_units = (byte)((mixer & 0x20) >> 5);
                    speedo_sensor = (byte)((mixer & 0x10) >> 4);
                    speedo_tach_trip_enable = (byte)((mixer & 0x08) >> 3);
                    tach_ppr_precision = (byte)((mixer & 0x04) >> 2);
                    speedo_tach_output = (byte)(mixer & 0x03);

                    speedo_PPM = (short)ExtractFromPacketLong(packet, 10, 14);
                    tach_PPR = (byte)ExtractFromPacketLong(packet, 14, 16);

                    hasAttributes = true;
                    break;
                default:
                    //Log.e(TAG, "BLE message not recognized!" + type); //TODO: write a logger message
                    break;

            }
        }
        public string attributeMessageColorPreviewPackage()
        {
            StringBuilder message = new StringBuilder();
            //Attach 16 character payloads to each 3 character message header (total of 19 character messages will be sent over BLE per attribute message)
            message.Append("0x0");
            message.Append(RGBW_backlight_red.ToString("X2"));
            message.Append(RGBW_backlight_green.ToString("X2"));
            message.Append(RGBW_backlight_blue.ToString("X2"));
            message.Append(RGBW_backlight_white.ToString("X2"));
            message.Append(RGBW_pointer_red.ToString("X2"));
            message.Append(RGBW_pointer_green.ToString("X2"));
            message.Append(RGBW_pointer_blue.ToString("X2"));
            message.Append(RGBW_pointer_white.ToString("X2"));
            return message.ToString();
        }
        

        // A function to get the "SAVE ALL" commands
        public string attributeMessagePackage()
        {
            //BLE message format-
            //message 1 will be "0x1" + "FFFFFFFF" + "FFFFFFFF"
            //message 2 will be "0x2" + "FFFFFFFF" + "FFFFFFFF"
            //message 3 will be "0x3" + "FFFFFFFF" + "FFFFFFFF"
            //message 4 will be "0x4" + "FFFFFFFF" + "FFFFFFFF"
            //message 5 will be "0x5" + "FFFFFFFF" + "FFFFFFFF"
            //message 6 will be "0x6" + "FFFFFFFF" + "FFFFFFFF"
            //message 6 will be "0x7" + "FFFFFFFF" + "FFFFFFFF"
            //message n will be "0xn" + "FFFFFFFF" + "FFFFFFFF"

            int mixer = 0;
            StringBuilder message = new StringBuilder();
            //Attach 16 character payloads to each 3 character message header (total of 19 character messages will be sent over BLE per attribute message)
            message.Append(" 0x1");
            message.Append(gauge_type.ToString("X4"));
            message.Append(gauge_home.ToString("X4"));
            message.Append(gauge_full.ToString("X4"));
            message.Append(gauge_min_valid_reading.ToString("X4"));

            message.Append(" 0x2"); //insert a space character for seperating each message
            message.Append(gauge_max_valid_reading.ToString("X4"));
            message.Append(gauge_sensor_curve.ToString("X2"));
            message.Append(gauge_pointer_weight.ToString("X4"));
            message.Append(gauge_hysteresis.ToString("X2"));
            message.Append(gauge_sensor_scan_rate.ToString("X2")); //TODO: Test if this does 255 correctly
            message.Append(gauge_sweep.ToString("X2"));

            message.Append(" 0x3");
            mixer = (0xF0 & (gauge_mode << 4)) | (0xc & (gauge_pointer_type << 2)) | (0x3 & (gauge_backlight_Flash_intensity));
            message.Append(mixer.ToString("X2"));
            mixer = (0xFE & (gauge_backlight_Flash_level << 1)) | (0x1 & (gauge_backlight_Flash_zone));
            message.Append(mixer.ToString("X2"));
            message.Append(((int)(gauge_backlight_top_voltage * 10)).ToString("X4"));
            message.Append(((int)(gauge_backlight_bot_voltage * 10)).ToString("X4"));
            //rearranged this from attributed message order to keep colors together
            mixer = (0xFE & (gauge_warning_bot_level << 1)) | (0x1 & (gauge_warning_bot_zone));
            message.Append(mixer.ToString("X2"));
            mixer = (0xFE & (gauge_warning_top_level << 1)) | (0x1 & (gauge_warning_top_zone));
            message.Append(mixer.ToString("X2"));

            message.Append(" 0x4");
            // 53 characters by this point
            message.Append(RGBW_backlight_red.ToString("X2"));
            message.Append(RGBW_backlight_green.ToString("X2"));
            message.Append(RGBW_backlight_blue.ToString("X2"));
            message.Append(RGBW_backlight_white.ToString("X2"));
            message.Append(RGBW_pointer_red.ToString("X2"));
            message.Append(RGBW_pointer_green.ToString("X2"));
            message.Append(RGBW_pointer_blue.ToString("X2"));
            message.Append(RGBW_pointer_white.ToString("X2"));


            message.Append(" 0x5");
            // Need to convert it back to the accepted byte value
            int tmpInt = BitConverter.ToInt32(BitConverter.GetBytes(gauge_coefficient0), 0);
            message.Append(tmpInt.ToString("X8"));

            tmpInt = BitConverter.ToInt32(BitConverter.GetBytes(gauge_coefficient1), 0);
            message.Append(tmpInt.ToString("X8"));

            message.Append(" 0x6");
            mixer = (0xC0 & (gauge_output_startup_delay << 6)) | (0x3F & (gauge_output_activation_delay));
            message.Append(mixer.ToString("X2"));
            mixer = (0xFE & (gauge_output_bot_level << 1)) | (0x1 & (gauge_output_bot_zone));
            message.Append(mixer.ToString("X2"));
            mixer = (0xFE & (gauge_output_top_level << 1)) | (0x1 & (gauge_output_top_zone));
            message.Append(mixer.ToString("X2"));
            message.Append(gauge_backlight_scan_rate.ToString("X2"));
            //room for 8 Bytes left, CRC will take up part of that
            message.Append("0000");//-filler
            message.Append("0000");//-CRC algorithm TBD

            // Speedo/Tach Message
            message.Append(" 0x7");
            // Need to convert it back to the accepted byte value
            tmpInt = BitConverter.ToInt32(BitConverter.GetBytes(lcd_total_accumulation), 0);
            message.Append(tmpInt.ToString("X8"));
            mixer = (0x80 & (enable_accumulation << 7)) | (0x20 & (speedo_tach_units << 5)) | (0x10 & (speedo_sensor << 4))
                    | (0x08 & (speedo_tach_trip_enable << 3)) | (0x04 & (tach_ppr_precision << 2)) | (0x03 & speedo_tach_output);
            message.Append(mixer.ToString("X2"));
            message.Append(speedo_PPM.ToString("X4"));
            message.Append(tach_PPR.ToString("X2"));

            return message.ToString();
        }

        public string GetAttributeLog()
        {
            //BLE message format-
            //message 1 will be "0x1" + "FFFFFFFF" + "FFFFFFFF"
            //message 2 will be "0x2" + "FFFFFFFF" + "FFFFFFFF"
            //message 3 will be "0x3" + "FFFFFFFF" + "FFFFFFFF"
            //message 4 will be "0x4" + "FFFFFFFF" + "FFFFFFFF"
            //message 5 will be "0x5" + "FFFFFFFF" + "FFFFFFFF"
            //message 6 will be "0x6" + "FFFFFFFF" + "FFFFFFFF"
            //message 7 will be "0x7" + "FFFFFFFF" + "FFFFFFFF"
            //message n will be "0xn" + "FFFFFFFF" + "FFFFFFFF"

            int mixer = 0;
            StringBuilder message = new StringBuilder();
            //Attach 16 character payloads to each 3 character message header (total of 19 character messages will be sent over BLE per attribute message)
            message.Append("gauge_type: " + gauge_type.ToString() + Environment.NewLine);
            message.Append("gauge_home: " + gauge_home.ToString() + Environment.NewLine);
            message.Append("gauge_full: " + gauge_full.ToString() + Environment.NewLine);
            message.Append("gauge_min_valid_reading: " + gauge_min_valid_reading.ToString() + Environment.NewLine);

            message.Append("gauge_max_valid_reading: " + gauge_max_valid_reading.ToString() + Environment.NewLine);
            message.Append("gauge_sensor_curve: " + gauge_sensor_curve.ToString() + Environment.NewLine);
            message.Append("gauge_pointer_weight: " + gauge_pointer_weight.ToString() + Environment.NewLine);
            message.Append("gauge_hysteresis: " + gauge_hysteresis.ToString() + Environment.NewLine);
            message.Append("gauge_sensor_scan_rate: " + gauge_sensor_scan_rate.ToString() + Environment.NewLine); //TODO: Test if this does 255 correctly
            message.Append("gauge_sweep: " + gauge_sweep.ToString() + Environment.NewLine);

            message.Append("gauge_mode: " + gauge_mode.ToString() + Environment.NewLine);
            message.Append("gauge_pointer_type: " + gauge_pointer_type.ToString() + Environment.NewLine);
            message.Append("gauge_backlight_Flash_intensity: " + gauge_backlight_Flash_intensity.ToString() + Environment.NewLine);
            mixer = (0xFE & (gauge_backlight_Flash_level << 1)) | (0x1 & (gauge_backlight_Flash_zone));
            message.Append("gauge_backlight_Flash_level: " + gauge_backlight_Flash_level.ToString() + Environment.NewLine);
            message.Append("gauge_backlight_Flash_zone: " + gauge_backlight_Flash_zone.ToString() + Environment.NewLine);
            message.Append("gauge_backlight_top_voltage: " + ((int)(gauge_backlight_top_voltage * 10)).ToString() + Environment.NewLine);
            message.Append("gauge_backlight_bot_voltage: " + ((int)(gauge_backlight_bot_voltage * 10)).ToString() + Environment.NewLine);
            //rearranged this from attributed message order to keep colors together
            message.Append("gauge_warning_bot_level: " + gauge_warning_bot_level.ToString() + Environment.NewLine);
            message.Append("gauge_warning_bot_zone: " + gauge_warning_bot_zone.ToString() + Environment.NewLine);
            message.Append("gauge_warning_top_level: " + gauge_warning_top_level.ToString() + Environment.NewLine);
            message.Append("gauge_warning_top_zone: " + gauge_warning_top_zone.ToString() + Environment.NewLine);

            // 53 characters by this point
            message.Append("RGBW_backlight_red: " + RGBW_backlight_red.ToString() + Environment.NewLine);
            message.Append("RGBW_backlight_green: " + RGBW_backlight_green.ToString() + Environment.NewLine);
            message.Append("RGBW_backlight_blue: " + RGBW_backlight_blue.ToString() + Environment.NewLine);
            message.Append("RGBW_backlight_white: " + RGBW_backlight_white.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_red: " + RGBW_pointer_red.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_green: " + RGBW_pointer_green.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_blue: " + RGBW_pointer_blue.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_white: " + RGBW_pointer_white.ToString() + Environment.NewLine);


            // Need to convert it back to the accepted byte value
            message.Append("gauge_coefficient0: " + gauge_coefficient0 + Environment.NewLine);

            message.Append("gauge_coefficient1: " + gauge_coefficient1 + Environment.NewLine);
            //0x6
            message.Append("gauge_output_startup_delay: " + gauge_output_startup_delay.ToString() + Environment.NewLine);
            message.Append("gauge_output_activation_delay: " + gauge_output_activation_delay.ToString() + Environment.NewLine);
            message.Append("gauge_output_bot_level: " + gauge_output_bot_level.ToString() + Environment.NewLine);
            message.Append("gauge_output_bot_zone: " + gauge_output_bot_zone.ToString() + Environment.NewLine);
            message.Append("gauge_output_top_level: " + gauge_output_top_level.ToString() + Environment.NewLine);
            message.Append("gauge_output_top_zone: " + gauge_output_top_zone.ToString() + Environment.NewLine);
            message.Append("gauge_backlight_scan_rate: " + gauge_backlight_scan_rate.ToString() + Environment.NewLine);
            
            //0x7
            message.Append("lcd_total_accumulationulation: " + lcd_total_accumulation.ToString() + Environment.NewLine);
            message.Append("enable_accumulation:" + enable_accumulation.ToString() + Environment.NewLine);
            message.Append("speedo_tach_units: " + speedo_tach_units.ToString() + Environment.NewLine);
            message.Append("speedo_sensor: " + speedo_sensor.ToString() + Environment.NewLine);
            message.Append("speedo_tach_trip_enable: " + speedo_tach_trip_enable.ToString() + Environment.NewLine);
            message.Append("tach_ppr_precision: " + tach_ppr_precision.ToString() + Environment.NewLine);
            message.Append("speedo_tach_output: " + speedo_tach_output.ToString() + Environment.NewLine);
            message.Append("Speedo PPM: " + speedo_PPM.ToString() + Environment.NewLine);
            message.Append("Tach PPR: " + tach_PPR.ToString() + Environment.NewLine);

            return message.ToString();
        }

        public string SaveColorToGauge()
        {
            StringBuilder message = new StringBuilder();
            message.Append("0x4");
            // 53 characters by this point
            message.Append(string.Format("%02X", (byte)RGBW_backlight_red));
            message.Append(string.Format("%02X", (byte)RGBW_backlight_green));
            message.Append(string.Format("%02X", (byte)RGBW_backlight_blue));
            message.Append(string.Format("%02X", (byte)RGBW_backlight_white));
            message.Append(string.Format("%02X", (byte)RGBW_pointer_red));
            message.Append(string.Format("%02X", (byte)RGBW_pointer_green));
            message.Append(string.Format("%02X", (byte)RGBW_pointer_blue));
            message.Append(string.Format("%02X", (byte)RGBW_pointer_white));

            return message.ToString();
        }

        public long ExtractFromPacketLong(string packet, int start, int end)
        {
            // the packet is a hexadecimal value, we want to extract parts of it for our attributes.
            // But because this code is ported over from Java, the substring function acts differently
            // java substring takes start and end index, where c# takes start and length from start
            return long.Parse(SafeSubstring(packet, start, end - start), System.Globalization.NumberStyles.HexNumber);
        }

        public string SafeSubstring(string value, int startIndex, int length)
        {
            return value.Substring(Math.Max(0, startIndex), Math.Min(length, value.Length-startIndex));
        }
    }
}
