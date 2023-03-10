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
        private static int gauge_type;
        private static byte quadrant_selected;

        /* Connect Fragment Variables*/
        private static string device_Name;
        private static string device_version;
        private static Guid device_BTGUID;

        /* Quad0 Specific Attributes FFA1 */
        // Byte 1
        private static byte quad0Location = 0;
        private static byte quad0SensorType;
        private static byte quad0_gauge; //This is now size short (16-bits; but need larger bits to store as unsigned)

        // Byte 2
        private static short quad0_hysteresis;
        private static short quad0_gauge_pointer_weight;
        private static byte quad0_curve;

        // Byte 3
        private static byte quad0_warning_zone;
        private static byte quad0_warning_threshold;

        // Byte 4
        private static int quad0_gauge_home;

        // Byte 5 and lower 5 bits of Byte 6
        private static int quad0_gauge_full;

        /* Quad1 Specific Attributes FFA1 */
        // Byte 1
        private static byte quad1Location = 1;
        private static byte quad1SensorType;
        private static byte quad1_gauge; //This is now size short (16-bits; but need larger bits to store as unsigned)

        // Byte 2
        private static short quad1_hysteresis;
        private static short quad1_gauge_pointer_weight;
        private static byte quad1_curve;

        // Byte 3
        private static byte quad1_warning_zone;
        private static byte quad1_warning_threshold;

        // Byte 4
        private static int quad1_gauge_home;

        // Byte 5 and lower 5 bits of Byte 6
        private static int quad1_gauge_full;

        /* Quad2 Specific Attributes FFA1 */
        // Byte 1
        private static byte quad2Location = 2;
        private static byte quad2SensorType;
        private static byte quad2_gauge; //This is now size short (16-bits; but need larger bits to store as unsigned)

        // Byte 2
        private static short quad2_hysteresis;
        private static short quad2_gauge_pointer_weight;
        private static byte quad2_curve;

        // Byte 3
        private static byte quad2_warning_zone;
        private static byte quad2_warning_threshold;

        // Byte 4
        private static int quad2_gauge_home;

        // Byte 5 and lower 5 bits of Byte 6
        private static int quad2_gauge_full;

        /* Quad3 Specific Attributes FFA1 */
        // Byte 1
        private static byte quad3Location = 3;
        private static byte quad3SensorType;
        private static byte quad3_gauge; //This is now size short (16-bits; but need larger bits to store as unsigned)

        // Byte 2
        private static short quad3_hysteresis;
        private static short quad3_gauge_pointer_weight;
        private static byte quad3_curve;

        // Byte 3
        private static byte quad3_warning_zone;
        private static byte quad3_warning_threshold;

        // Byte 4
        private static int quad3_gauge_home;

        // Byte 5 and lower 5 bits of Byte 6
        private static int quad3_gauge_full;

        // Color Bytes 6 7 and 8
        private static short RGBW_backlight_red_quad0;
        private static short RGBW_backlight_green_quad0;
        private static short RGBW_backlight_blue_quad0;
        private static short RGBW_backlight_white_quad0;

        private static short RGBW_backlight_red_quad1;
        private static short RGBW_backlight_green_quad1;
        private static short RGBW_backlight_blue_quad1;
        private static short RGBW_backlight_white_quad1;

        private static short RGBW_backlight_red_quad2;
        private static short RGBW_backlight_green_quad2;
        private static short RGBW_backlight_blue_quad2;
        private static short RGBW_backlight_white_quad2;

        private static short RGBW_backlight_red_quad3;
        private static short RGBW_backlight_green_quad3;
        private static short RGBW_backlight_blue_quad3;
        private static short RGBW_backlight_white_quad3;

        /* FFA2 */

        // Byte 1
        private static byte backlight_input;
        private static byte warning_light_backlight_flash;
        private static byte gauge_pointer_type;
        private static byte daytime_brightness;

        // Bytes 2, 3, 4
        private static float gauge_backlight_top_voltage; // When sending to gauge, we need to multiply by 10 and cast it to int
        private static float gauge_backlight_bot_voltage; // so that we can send it as a short, since it only allocates 2 byte for it

        // Byte 5
        private static byte max_brightness;

        /* Shared Color Editor Fragment Variables*/
        // Bytes 6, 7, 8
        private static short RGBW_pointer_red;
        private static short RGBW_pointer_green;
        private static short RGBW_pointer_blue;
        private static short RGBW_pointer_white;


        /* FFA3 */

        // Byte 1
        private static byte enable_accumulation;
        private static byte nvm_coeff_pair;
        private static byte speedo_tach_units;
        private static byte speedo_sensor;
        private static byte tach_sensor;
        private static byte coeff_quad;
        private static byte output_quad;
        private static byte canbus_speed;

        // Byte 2, 3, 4
        private static float lcd_total_accumulation = 0.0000000000f;

        // Byte 5 and 6
        private static short speedo_PPM;

        // Byte 7
        private static byte tach_PPR;

        //Byte 8
        private static byte gauge_output_level;
        private static byte gauge_output_zone;

        /* FFA4 */
        private static float gauge_coefficient0 = 0.0000000000f; //0.0f = single precision float
        private static float gauge_coefficient1 = 0.0000000000f;


        public GaugeAttributes()
        {
            init();
        }
        /* Default initalizer*/
        public void init()
        {
            device_Name = "new name";
            device_version = "00.00.00";
            device_BTGUID = new Guid();

            RGBW_backlight_red_quad0 = 0;
            RGBW_backlight_green_quad0 = 255;
            RGBW_backlight_blue_quad0 = 0;
            RGBW_backlight_white_quad0 = 0;

            RGBW_backlight_red_quad1 = 0;
            RGBW_backlight_green_quad1 = 255;
            RGBW_backlight_blue_quad1 = 0;
            RGBW_backlight_white_quad1 = 0;

            RGBW_backlight_red_quad2 = 0;
            RGBW_backlight_green_quad2 = 255;
            RGBW_backlight_blue_quad2 = 0;
            RGBW_backlight_white_quad2 = 0;

            RGBW_backlight_red_quad3 = 0;
            RGBW_backlight_green_quad3 = 255;
            RGBW_backlight_blue_quad3 = 0;
            RGBW_backlight_white_quad3 = 0;

            quad0SensorType = 0;
            quad0_gauge = 0;           //This is now size short (16-bits; but need larger bits to store as unsigned)
            quad0_hysteresis = 0;
            quad0_gauge_pointer_weight = 0;
            quad0_curve = 0;
            quad0_warning_zone = 0;
            quad0_warning_threshold = 0;
            quad0_gauge_home = 0;
            quad0_gauge_full = 0;

            quad1SensorType = 0;
            quad1_gauge = 0;           //This is now size short (16-bits; but need larger bits to store as unsigned)
            quad1_hysteresis = 0;
            quad1_gauge_pointer_weight = 0;
            quad1_curve = 0;
            quad1_warning_zone = 0;
            quad1_warning_threshold = 0;
            quad1_gauge_home = 0;
            quad1_gauge_full = 0;

            quad2SensorType = 0;
            quad2_gauge = 0;           //This is now size short (16-bits; but need larger bits to store as unsigned)
            quad2_hysteresis = 0;
            quad2_gauge_pointer_weight = 0;
            quad2_curve = 0;
            quad2_warning_zone = 0;
            quad2_warning_threshold = 0;
            quad2_gauge_home = 0;
            quad2_gauge_full = 0;

            quad3SensorType = 0;
            quad3_gauge = 0;           //This is now size short (16-bits; but need larger bits to store as unsigned)
            quad3_hysteresis = 0;
            quad3_gauge_pointer_weight = 0;
            quad3_curve = 0;
            quad3_warning_zone = 0;
            quad3_warning_threshold = 0;
            quad3_gauge_home = 0;
            quad3_gauge_full = 0;

            /* FFA2 */
            // Byte 1
            backlight_input = 0;
            warning_light_backlight_flash = 0;
            gauge_pointer_type = 0;
            daytime_brightness = 100;

            // Bytes 2, 3, 4
            gauge_backlight_top_voltage = 18; // When sending to gauge, we need to multiply by 10 and cast it to int
            gauge_backlight_bot_voltage = 7; // so that we can send it as a short, since it only allocates 2 byte for it

            // Byte 5
            max_brightness = 100;

            /* Shared Color Editor Fragment Variables*/
            // Bytes 6, 7, 8
            RGBW_pointer_red = 255;
            RGBW_pointer_green = 0;
            RGBW_pointer_blue = 0;
            RGBW_pointer_white = 0;


            /* FFA3 */

            // Byte 1
            enable_accumulation = 0;
            nvm_coeff_pair = 0;
            speedo_tach_units = 0;
            speedo_sensor = 0;
            tach_sensor = 0;
            coeff_quad = 0;
            output_quad = 0;
            canbus_speed = 0;

            // Byte 2, 3, 4
            lcd_total_accumulation = 0.0000000000f;

            // Byte 5 and 6
            speedo_PPM = 4000;

            // Byte 7
            tach_PPR = 255;

            //Byte 8
            gauge_output_level = 0;
            gauge_output_zone = 0;

            /* FFA4 */
            gauge_coefficient0 = 0.0000000000f; //0.0f = single precision float
            gauge_coefficient1 = 0.0000000000f;
        }

        public void ResetFactoryDefaults()
        {
            // Reset all the attributes that all the gauge types share in common.
            // The gauge specific attributes at set when the user specified the gauge type in AttributesPage.xaml.cs > GaugeType_SelectedAsync()
            //FFA2
            backlight_input = 0;
            warning_light_backlight_flash = 0;
            gauge_pointer_type = 0;
            daytime_brightness = 100;
            gauge_backlight_top_voltage = 18.1f;
            gauge_backlight_bot_voltage = 7.8f;
            max_brightness = 100;
            RGBW_pointer_red = 255;
            RGBW_pointer_green = 0;
            RGBW_pointer_blue = 0;
            RGBW_pointer_white = 0;

            //FFA3
            enable_accumulation = 0;
            lcd_total_accumulation = 0;
            nvm_coeff_pair = 0;
            speedo_tach_units = 1;
            speedo_sensor = 0;
            tach_sensor = 0;
            coeff_quad = 0;
            output_quad = 0;
            canbus_speed = 0;
            speedo_PPM = 4000;
            tach_PPR = 255;
            gauge_output_level = 0;
            gauge_output_zone = 0;
            
            //FFA4
            gauge_coefficient0 = 0;
            gauge_coefficient1 = 0;
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

        public void setGUID(Guid id)
        {
            device_BTGUID = id;
        }
        public Guid getGUID()
        {
            return device_BTGUID;
        }
        public void setQuadrantSlected(byte val)
        {
            quadrant_selected = val;
        }
        public byte getQuadrantSelected()
        {
            return quadrant_selected;
        }
        public void setBacklightInput(byte val)
        {
            backlight_input = val;
        }
        public byte getBacklightInput()
        {
            return backlight_input;
        }
        public void setBacklightFlash(byte val)
        {
            warning_light_backlight_flash = val;
        }
        public byte getBacklightFlash()
        {
            return warning_light_backlight_flash;
        }
        public byte getDaytimeBrightness()
        {
            return max_brightness;
        }
        public void setDaytimeBrightness(byte val)
        {
            daytime_brightness = val;
        }
        public byte getMaxBrightness()
        {
            return daytime_brightness;
        }
        public void setMaxBrightness(byte val)
        {
            max_brightness = val;
        }
        public byte getNvmPair()
        {
            return nvm_coeff_pair;
        }
        public void setNvmPair(byte val)
        {
            nvm_coeff_pair = val;
        }
        public byte getCoeffQuad()
        {
            return coeff_quad;
        }
        public void setCoeffQuad(byte val)
        {
            coeff_quad = val;
        }
        public byte getCanbusSpeed()
        {
            return canbus_speed;
        }
        public void setCanbusSPeed(byte val)
        {
            canbus_speed = val;
        }
        public void setQuad0GaugeType(byte val)
        {
            quad0_gauge = val;
        }
        public byte getQuad0GaugeType()
        {
            return quad0_gauge;
        }
        public void setQuad1GaugeType(byte val)
        {
            quad1_gauge = val;
        }
        public byte getQuad1GaugeType()
        {
            return quad1_gauge;
        }
        public void setQuad2GaugeType(byte val)
        {
            quad2_gauge = val;
        }
        public byte getQuad2GaugeType()
        {
            return quad2_gauge;
        }
        public void setQuad3GaugeType(byte val)
        {
            quad3_gauge = val;
        }
        public byte getQuad3GaugeType()
        {
            return quad3_gauge;
        }
        public void setQuad0SensorType(byte val)
        {
            quad0SensorType = val;
        }
        public byte getQuad0SensorType()
        {
            return quad0SensorType;
        }
        public void setQuad1SensorType(byte val)
        {
            quad1SensorType = val;
        }
        public byte getQuad1SensorType()
        {
            return quad1SensorType;
        }
        public void setQuad2SensorType(byte val)
        {
            quad2SensorType = val;
        }
        public byte getQuad2SensorType()
        {
            return quad2SensorType;
        }
        public void setQuad3SensorType(byte val)
        {
            quad3SensorType = val;
        }
        public byte getQuad3SensorType()
        {
            return quad3SensorType;
        }
        public void setQuad0SensorCurve(byte val)
        {
            quad0_curve = val;
        }
        public byte getQuad0SensorCurve()
        {
            return quad0_curve;
        }
        public void setQuad1SensorCurve(byte val)
        {
            quad1_curve = val;
        }
        public byte getQuad1SensorCurve()
        {
            return quad1_curve;
        }
        public void setQuad2SensorCurve(byte val)
        {
            quad2_curve = val;
        }
        public byte getQuad2SensorCurve()
        {
            return quad2_curve;
        }
        public void setQuad3SensorCurve(byte val)
        {
            quad3_curve = val;
        }
        public byte getQuad3SensorCurve()
        {
            return quad3_curve;
        }

        /* Color Fragment Methods*/
        public void setQuad0BacklightColorRed(short val)
        {
            RGBW_backlight_red_quad0 = val;
        }
        public short getQuad0BacklightColorRed()
        {
            return RGBW_backlight_red_quad0;
        }

        public void setQuad1BacklightColorRed(short val)
        {
            RGBW_backlight_red_quad1 = val;
        }
        public short getQuad1BacklightColorRed()
        {
            return RGBW_backlight_red_quad1;
        }

        public void setQuad2BacklightColorRed(short val)
        {
            RGBW_backlight_red_quad2 = val;
        }
        public short getQuad2BacklightColorRed()
        {
            return RGBW_backlight_red_quad2;
        }

        public void setQuad3BacklightColorRed(short val)
        {
            RGBW_backlight_red_quad3 = val;
        }
        public short getQuad3BacklightColorRed()
        {
            return RGBW_backlight_red_quad3;
        }

        public void setPointerColorRed(short val)
        {
            RGBW_pointer_red = val;
        }
        public short getPointerColorRed()
        {
            return RGBW_pointer_red;
        }

        public void setQuad0BacklightColorGreen(short val)
        {
            RGBW_backlight_green_quad0 = val;
        }

        public short getQuad0BacklightColorGreen()
        {
            return RGBW_backlight_green_quad0;
        }

        public void setQuad1BacklightColorGreen(short val)
        {
            RGBW_backlight_green_quad1 = val;
        }

        public short getQuad1BacklightColorGreen()
        {
            return RGBW_backlight_green_quad1;
        }

        public void setQuad2BacklightColorGreen(short val)
        {
            RGBW_backlight_green_quad2 = val;
        }

        public short getQuad2BacklightColorGreen()
        {
            return RGBW_backlight_green_quad2;
        }

        public void setQuad3BacklightColorGreen(short val)
        {
            RGBW_backlight_green_quad0 = val;
        }

        public short getQuad3BacklightColorGreen()
        {
            return RGBW_backlight_green_quad3;
        }

        public void setPointerColorGreen(short val)
        {
            RGBW_pointer_green = val;
        }
        public short getPointerColorGreen()
        {
            return RGBW_pointer_green;
        }

        public void setQuad0BacklightColorBlue(short val)
        {
            RGBW_backlight_blue_quad0 = val;
        }
        public short getQuad0BacklightColorBlue()
        {
            return RGBW_backlight_blue_quad0;
        }

        public void setQuad1BacklightColorBlue(short val)
        {
            RGBW_backlight_blue_quad1 = val;
        }
        public short getQuad1BacklightColorBlue()
        {
            return RGBW_backlight_blue_quad1;
        }

        public void setQuad2BacklightColorBlue(short val)
        {
            RGBW_backlight_blue_quad2 = val;
        }
        public short getQuad2BacklightColorBlue()
        {
            return RGBW_backlight_blue_quad2;
        }

        public void setQuad3BacklightColorBlue(short val)
        {
            RGBW_backlight_blue_quad3 = val;
        }
        public short getQuad3BacklightColorBlue()
        {
            return RGBW_backlight_blue_quad3;
        }

        public void setPointerColorBlue(short val)
        {
            RGBW_pointer_blue = val;
        }
        public short getPointerColorBlue()
        {
            return RGBW_pointer_blue;
        }

        public void SetQuad0BackLightColor(short? r, short? g, short? b)
        {
            if (r.HasValue && g.HasValue && b.HasValue)
            {
                setQuad0BacklightColorRed(r.Value);
                setQuad0BacklightColorGreen(g.Value);
                setQuad0BacklightColorBlue(b.Value);
            }
        }

        public void SetQuad1BackLightColor(short? r, short? g, short? b)
        {
            if (r.HasValue && g.HasValue && b.HasValue)
            {
                setQuad1BacklightColorRed(r.Value);
                setQuad1BacklightColorGreen(g.Value);
                setQuad1BacklightColorBlue(b.Value);
            }
        }

        public void SetQuad2BackLightColor(short? r, short? g, short? b)
        {
            if (r.HasValue && g.HasValue && b.HasValue)
            {
                setQuad2BacklightColorRed(r.Value);
                setQuad2BacklightColorGreen(g.Value);
                setQuad2BacklightColorBlue(b.Value);
            }
        }

        public void SetQuad3BackLightColor(short? r, short? g, short? b)
        {
            if (r.HasValue && g.HasValue && b.HasValue)
            {
                setQuad3BacklightColorRed(r.Value);
                setQuad3BacklightColorGreen(g.Value);
                setQuad3BacklightColorBlue(b.Value);
            }
        }

        public void SetPointerColor(short? r, short? g, short? b)
        {
            if (r.HasValue && g.HasValue && b.HasValue)
            {

                setPointerColorRed(r.Value);
                setPointerColorGreen(g.Value);
                setPointerColorBlue(b.Value);
            }
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

        public void setQuad0GaugeHome(int val)
        {
            quad0_gauge_home = val;
        }
        public int getQuad0GaugeHome()
        {
            return quad0_gauge_home;
        }

        public void setQuad1GaugeHome(int val)
        {
            quad1_gauge_home = val;
        }
        public int getQuad1GaugeHome()
        {
            return quad1_gauge_home;
        }

        public void setQuad2GaugeHome(int val)
        {
            quad2_gauge_home = val;
        }
        public int getQuad2GaugeHome()
        {
            return quad2_gauge_home;
        }

        public void setQuad3GaugeHome(int val)
        {
            quad3_gauge_home = val;
        }
        public int getQuad3GaugeHome()
        {
            return quad3_gauge_home;
        }

        public void setQuad0GaugeFull(int val)
        {
            quad0_gauge_full = val;
        }
        public int getQuad0GaugeFull()
        {
            return quad0_gauge_full;
        }

        public void setQuad1GaugeFull(int val)
        {
            quad1_gauge_full = val;
        }
        public int getQuad1GaugeFull()
        {
            return quad1_gauge_full;
        }

        public void setQuad2GaugeFull(int val)
        {
            quad2_gauge_full = val;
        }
        public int getQuad2GaugeFull()
        {
            return quad2_gauge_full;
        }

        public void setQuad3GaugeFull(int val)
        {
            quad3_gauge_full = val;
        }
        public int getQuad3GaugeFull()
        {
            return quad3_gauge_full;
        }

        public void setQuad0GaugePointerWeight(short val)
        {
            quad0_gauge_pointer_weight = val;
        }
        public short getQuad0GaugePointerWeight()
        {
            return quad0_gauge_pointer_weight;
        }

        public void setQuad1GaugePointerWeight(short val)
        {
            quad1_gauge_pointer_weight = val;
        }
        public short getQuad1GaugePointerWeight()
        {
            return quad1_gauge_pointer_weight;
        }

        public void setQuad2GaugePointerWeight(short val)
        {
            quad2_gauge_pointer_weight = val;
        }
        public short getQuad2GaugePointerWeight()
        {
            return quad2_gauge_pointer_weight;
        }

        public void setQuad3GaugePointerWeight(short val)
        {
            quad3_gauge_pointer_weight = val;
        }
        public short getQuad3GaugePointerWeight()
        {
            return quad3_gauge_pointer_weight;
        }

        public void setQuad0GaugeHysteresis(short val)
        {
            quad0_hysteresis = val;
        }
        public short getQuad0GaugeHysteresis()
        {
            return quad0_hysteresis;
        }

        public void setQuad1GaugeHysteresis(short val)
        {
            quad1_hysteresis = val;
        }
        public short getQuad1GaugeHysteresis()
        {
            return quad1_hysteresis;
        }
        public void setQuad2GaugeHysteresis(short val)
        {
            quad2_hysteresis = val;
        }
        public short getQuad2GaugeHysteresis()
        {
            return quad2_hysteresis;
        }
        public void setQuad3GaugeHysteresis(short val)
        {
            quad3_hysteresis = val;
        }
        public short getQuad3GaugeHysteresis()
        {
            return quad3_hysteresis;
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


        public void setGaugePointerType(byte val)
        {
            gauge_pointer_type = val;
        }
        public byte getGaugePointerType()
        {
            return gauge_pointer_type;
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


        public void setQuad0GaugeWarningLevel(byte val)
        {
            quad0_warning_threshold = val;
        }
        public byte getQuad0GaugeWarningLevel()
        {
            return quad0_warning_threshold;
        }

        public void setQuad0GaugeWarningZone(byte val)
        {
            quad0_warning_zone = val;
        }
        public byte getQuad0GaugeWarningZone()
        {
            return quad0_warning_zone;
        }

        public void setQuad1GaugeWarningLevel(byte val)
        {
            quad1_warning_threshold = val;
        }
        public byte getQuad1GaugeWarningLevel()
        {
            return quad1_warning_threshold;
        }

        public void setQuad1GaugeWarningZone(byte val)
        {
            quad1_warning_zone = val;
        }
        public byte getQuad1GaugeWarningZone()
        {
            return quad1_warning_zone;
        }

        public void setQuad2GaugeWarningLevel(byte val)
        {
            quad2_warning_threshold = val;
        }
        public byte getQuad2GaugeWarningLevel()
        {
            return quad2_warning_threshold;
        }

        public void setQuad2GaugeWarningZone(byte val)
        {
            quad2_warning_zone = val;
        }
        public byte getQuad2GaugeWarningZone()
        {
            return quad2_warning_zone;
        }

        public void setQuad3GaugeWarningLevel(byte val)
        {
            quad3_warning_threshold = val;
        }
        public byte getQuad3GaugeWarningLevel()
        {
            return quad3_warning_threshold;
        }

        public void setQuad3GaugeWarningZone(byte val)
        {
            quad3_warning_zone = val;
        }
        public byte getQuad3GaugeWarningZone()
        {
            return quad3_warning_zone;
        }
        public void setGaugeOutputLevel(byte val)
        {
            gauge_output_level = val;
        }
        public byte getGaugeOutputLevel()
        {
            return gauge_output_level;
        }

        public void setGaugeOutputZone(byte val)
        {
            gauge_output_zone = val;
        }
        public byte getGaugeOutputZone()
        {
            return gauge_output_zone;
        }
        public void setGaugeOutputQuad(byte val)
        {
            output_quad = val;
        }
        public byte getGaugeOutputQuad()
        {
            return output_quad;
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
        public void setTachSensor(byte val)
        {
            tach_sensor = val;
        }
        public byte getTachSensor()
        {
            return tach_sensor;
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

            setQuad0GaugeWarningLevel(profile.getQuad0GaugeWarningLevel());
            setQuad0GaugeWarningZone(profile.getQuad0GaugeWarningZone());
            setQuad0GaugePointerWeight(profile.getQuad0GaugePointerWeight());
            setQuad0GaugeHysteresis(profile.getQuad0GaugeHysteresis());

            setQuad1GaugeWarningLevel(profile.getQuad1GaugeWarningLevel());
            setQuad1GaugeWarningZone(profile.getQuad1GaugeWarningZone());
            setQuad1GaugePointerWeight(profile.getQuad1GaugePointerWeight());
            setQuad1GaugeHysteresis(profile.getQuad1GaugeHysteresis());

            setQuad2GaugeWarningLevel(profile.getQuad2GaugeWarningLevel());
            setQuad2GaugeWarningZone(profile.getQuad2GaugeWarningZone());
            setQuad2GaugePointerWeight(profile.getQuad2GaugePointerWeight());
            setQuad2GaugeHysteresis(profile.getQuad2GaugeHysteresis());

            setQuad3GaugeWarningLevel(profile.getQuad3GaugeWarningLevel());
            setQuad3GaugeWarningZone(profile.getQuad3GaugeWarningZone());
            setQuad3GaugePointerWeight(profile.getQuad3GaugePointerWeight());
            setQuad3GaugeHysteresis(profile.getQuad3GaugeHysteresis());

            setGaugeOutputLevel(profile.getGaugeOutputLevel());
            setGaugeOutputZone(profile.getGaugeOutputZone());

            setGaugeCoefficient0(profile.getGaugeCoefficient0());
            setGaugeCoefficient1(profile.getGaugeCoefficient1());
            setTotalAccumulationEnabled(profile.getTotalAccumulationEnabled());
            setUnits(profile.getUnits());
            setSpeedoSensor(profile.getSpeedoSensor());
            setTachSensor(profile.getTachSensor());
            setSpeedoPPM(profile.getSpeedoPPM());
            setTachPPR(profile.getTachPPR());
            setTotalAccum(profile.getTotalAccum());

        }

        // A function to get the "SAVE ALL" commands
        public string attributeMessagePackage()
        {
            //BLE message format-
            //message 1 will be "0x1" + "FFFFFFFF" + "FFFFFFFF" FFA1
            //message 2 will be "0x2" + "FFFFFFFF" + "FFFFFFFF" FFA1 
            //message 3 will be "0x3" + "FFFFFFFF" + "FFFFFFFF" FFA1
            //message 4 will be "0x4" + "FFFFFFFF" + "FFFFFFFF" FFA1
            //message 5 will be "0x5" + "FFFFFFFF" + "FFFFFFFF" FFA2
            //message 6 will be "0x6" + "FFFFFFFF" + "FFFFFFFF" FFA3
            //message 7 will be "0x7" + "FFFFFFFF" + "FFFFFFFF" FFA4
            //message n will be "0xn" + "FFFFFFFF" + "FFFFFFFF"

            int mixer = 0;
            StringBuilder message = new StringBuilder();
            //Attach 16 character payloads to each 3 character message header (total of 19 character messages will be sent over BLE per attribute message)
            // FFA1 Quad 0
            message.Append(" 0x1");
            mixer = ((0xC0 & quad0Location) | (quad0SensorType & 0x20) | (quad0_gauge & 0x1F));
            message.Append(mixer.ToString("X2"));
            mixer = ((0xC0 & quad0_hysteresis) | (0x30 & quad0_gauge_pointer_weight) | (0x0F & quad0_curve));
            message.Append(mixer.ToString("X2"));
            message.Append(quad0_gauge_home.ToString("X2"));
            message.Append(quad0_gauge_full.ToString("X2"));
            mixer = ((0x3F & quad0_gauge_full >> 8) | (RGBW_backlight_red_quad0 << 2 & 0xC0));
            message.Append(mixer.ToString("X2"));
            mixer = ((0x3F & RGBW_backlight_green_quad0 >> 8) | (RGBW_backlight_red_quad0 << 4 & 0xC0));
            message.Append(mixer.ToString("X2"));
            mixer = ((0x3F & RGBW_backlight_blue_quad0 >> 8) | (RGBW_backlight_red_quad0 << 6 & 0xC0));
            message.Append(mixer.ToString("X2"));

            // FFA1 Quad 1
            message.Append(" 0x2"); //insert a space character for seperating each message
            mixer = ((0xC0 & quad1Location) | (quad1SensorType & 0x20) | (quad1_gauge & 0x1F));
            message.Append(mixer.ToString("X2"));
            mixer = ((0xC0 & quad1_hysteresis) | (0x30 & quad1_gauge_pointer_weight) | (0x0F & quad1_curve));
            message.Append(mixer.ToString("X2"));
            message.Append(quad1_gauge_home.ToString("X2"));
            message.Append(quad1_gauge_full.ToString("X2"));
            mixer = ((0x3F & quad1_gauge_full >> 8) | (RGBW_backlight_red_quad1 << 2 & 0xC0));
            message.Append(mixer.ToString("X2"));
            mixer = ((0x3F & RGBW_backlight_green_quad1 >> 8) | (RGBW_backlight_red_quad1 << 4 & 0xC0));
            message.Append(mixer.ToString("X2"));
            mixer = ((0x3F & RGBW_backlight_blue_quad1 >> 8) | (RGBW_backlight_red_quad1 << 6 & 0xC0));
            message.Append(mixer.ToString("X2"));

            // Only append these messages for 4 in 1 gauges
            if (gauge_type == 16 || gauge_type == 18)
            {
                // FFA1 Quad 2
                message.Append(" 0x3");
                mixer = ((0xC0 & quad2Location) | (quad2SensorType & 0x20) | (quad2_gauge & 0x1F));
                message.Append(mixer.ToString("X2"));
                mixer = ((0xC0 & quad2_hysteresis) | (0x30 & quad2_gauge_pointer_weight) | (0x0F & quad2_curve));
                message.Append(mixer.ToString("X2"));
                message.Append(quad2_gauge_home.ToString("X2"));
                message.Append(quad2_gauge_full.ToString("X2"));
                mixer = ((0x3F & quad2_gauge_full >> 8) | (RGBW_backlight_red_quad2 << 2 & 0xC0));
                message.Append(mixer.ToString("X2"));
                mixer = ((0x3F & RGBW_backlight_green_quad2 >> 8) | (RGBW_backlight_red_quad2 << 4 & 0xC0));
                message.Append(mixer.ToString("X2"));
                mixer = ((0x3F & RGBW_backlight_blue_quad2 >> 8) | (RGBW_backlight_red_quad2 << 6 & 0xC0));
                message.Append(mixer.ToString("X2"));

                //FFA1 Quad 3
                message.Append(" 0x4");
                mixer = ((0xC0 & quad3Location) | (quad3SensorType & 0x20) | (quad3_gauge & 0x1F));
                message.Append(mixer.ToString("X2"));
                mixer = ((0xC0 & quad3_hysteresis) | (0x30 & quad3_gauge_pointer_weight) | (0x0F & quad3_curve));
                message.Append(mixer.ToString("X2"));
                message.Append(quad3_gauge_home.ToString("X2"));
                message.Append(quad3_gauge_full.ToString("X2"));
                mixer = ((0x3F & quad3_gauge_full >> 8) | (RGBW_backlight_red_quad3 << 2 & 0xC0));
                message.Append(mixer.ToString("X2"));
                mixer = ((0x3F & RGBW_backlight_green_quad3 >> 8) | (RGBW_backlight_red_quad3 << 4 & 0xC0));
                message.Append(mixer.ToString("X2"));
                mixer = ((0x3F & RGBW_backlight_blue_quad3 >> 8) | (RGBW_backlight_red_quad3 << 6 & 0xC0));
                message.Append(mixer.ToString("X2"));

            }

            //FFA2
            message.Append(" 0x5");
            // Need to convert it back to the accepted byte value
            mixer = (0x80 & backlight_input << 7 | (0x40 & warning_light_backlight_flash) | (0x0F & daytime_brightness));
            message.Append(mixer.ToString("X2"));
            message.Append((0xFF & (int)(gauge_backlight_bot_voltage * 10)).ToString("X2"));
            mixer = ((0xF0 & (int)(gauge_backlight_bot_voltage * 10) >> 4) | (0x0F & (int)(gauge_backlight_top_voltage * 10) >> 8));
            message.Append(mixer.ToString("X2"));
            message.Append((0xFF & (int)(gauge_backlight_top_voltage * 10)).ToString("X2"));
            message.Append(max_brightness.ToString("X2"));
            message.Append((0x3F & RGBW_pointer_red).ToString("X2"));
            message.Append((0x3F & RGBW_pointer_green).ToString("X2"));
            message.Append((0x3F & RGBW_pointer_blue).ToString("X2"));

            //FFA3
            message.Append(" 0x6");
            mixer = (0x80 & (enable_accumulation << 7)) | (0x40 & (nvm_coeff_pair << 6)) | (0x20 & (speedo_tach_units << 5)) | (0x10 & (speedo_sensor << 4))
                    | (0x08 & (tach_sensor << 3)) | (0x06 & (output_quad << 1)) | (0x01 & canbus_speed);
            message.Append(mixer.ToString("X2"));
            if (enable_accumulation == 1)
            {
                int tempInt = BitConverter.ToInt32(BitConverter.GetBytes(lcd_total_accumulation), 0);
                message.Append((tempInt >> 16 & 0xFF).ToString("X2"));
                message.Append((tempInt >> 8 & 0xFF).ToString("X2"));
                message.Append((tempInt & 0xFF).ToString("X2"));

            }
            else
            {
                int tempInt = 0;
                message.Append((tempInt >> 16 & 0xFF).ToString("X2"));
                message.Append((tempInt >> 8 & 0xFF).ToString("X2"));
                message.Append((tempInt & 0xFF).ToString("X2"));
            }

            message.Append(speedo_PPM.ToString("X4"));
            message.Append(tach_PPR.ToString("X2"));
            mixer = ((0x80 & gauge_output_zone) | (0x7F & gauge_output_level));
            message.Append(mixer.ToString("X2"));

            // FFA4
            message.Append(" 0x7");
            // Need to convert it back to the accepted byte value
            int tmpInt = BitConverter.ToInt32(BitConverter.GetBytes(gauge_coefficient0), 0);
            message.Append(tmpInt.ToString("X8"));
            tmpInt = BitConverter.ToInt32(BitConverter.GetBytes(gauge_coefficient1), 0);
            message.Append(tmpInt.ToString("X8"));
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

            StringBuilder message = new StringBuilder();

            // 0x1
            message.Append("quad0_gauge_type: " + quad0_gauge.ToString() + Environment.NewLine);
            message.Append("quad0_sensor_type: " + quad0SensorType.ToString() + Environment.NewLine);
            message.Append("quad0_gauge_home: " + quad0_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad0_gauge_full: " + quad0_gauge_full.ToString() + Environment.NewLine);
            message.Append("quad0_hysteresis: " + quad0_gauge.ToString() + Environment.NewLine);
            message.Append("quad0_motor_weight: " + quad0SensorType.ToString() + Environment.NewLine);
            message.Append("quad0_curve: " + quad0_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad0_warning_zone: " + quad0_warning_zone.ToString() + Environment.NewLine);
            message.Append("quad0_warning_level: " + quad0_warning_threshold.ToString() + Environment.NewLine);
            message.Append("quad0_RGBW_backlight_red: " + RGBW_backlight_red_quad0.ToString() + Environment.NewLine);
            message.Append("quad0_RGBW_backlight_green: " + RGBW_backlight_green_quad0.ToString() + Environment.NewLine);
            message.Append("quad0_RGBW_backlight_blue: " + RGBW_backlight_blue_quad0.ToString() + Environment.NewLine);

            // 0x2
            message.Append("quad1_gauge_type: " + quad1_gauge.ToString() + Environment.NewLine);
            message.Append("quad1_sensor_type: " + quad1SensorType.ToString() + Environment.NewLine);
            message.Append("quad1_gauge_home: " + quad1_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad1_gauge_full: " + quad1_gauge_full.ToString() + Environment.NewLine);
            message.Append("quad1_hysteresis: " + quad1_gauge.ToString() + Environment.NewLine);
            message.Append("quad1_motor_weight: " + quad1SensorType.ToString() + Environment.NewLine);
            message.Append("quad1_curve: " + quad1_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad1_warning_zone: " + quad1_warning_zone.ToString() + Environment.NewLine);
            message.Append("quad1_warning_level: " + quad1_warning_threshold.ToString() + Environment.NewLine);
            message.Append("quad1_RGBW_backlight_red: " + RGBW_backlight_red_quad1.ToString() + Environment.NewLine);
            message.Append("quad1_RGBW_backlight_green: " + RGBW_backlight_green_quad1.ToString() + Environment.NewLine);
            message.Append("quad1_RGBW_backlight_blue: " + RGBW_backlight_blue_quad1.ToString() + Environment.NewLine);

            // 0x3
            message.Append("quad2_gauge_type: " + quad2_gauge.ToString() + Environment.NewLine);
            message.Append("quad2_sensor_type: " + quad2SensorType.ToString() + Environment.NewLine);
            message.Append("quad2_gauge_home: " + quad2_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad2_gauge_full: " + quad2_gauge_full.ToString() + Environment.NewLine);
            message.Append("quad2_hysteresis: " + quad2_gauge.ToString() + Environment.NewLine);
            message.Append("quad2_motor_weight: " + quad2SensorType.ToString() + Environment.NewLine);
            message.Append("quad2_curve: " + quad2_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad2_warning_zone: " + quad2_warning_zone.ToString() + Environment.NewLine);
            message.Append("quad2_warning_level: " + quad2_warning_threshold.ToString() + Environment.NewLine);
            message.Append("quad2_RGBW_backlight_red: " + RGBW_backlight_red_quad2.ToString() + Environment.NewLine);
            message.Append("quad2_RGBW_backlight_green: " + RGBW_backlight_green_quad2.ToString() + Environment.NewLine);
            message.Append("quad2_RGBW_backlight_blue: " + RGBW_backlight_blue_quad2.ToString() + Environment.NewLine);

            // 0x4
            message.Append("quad3_gauge_type: " + quad3_gauge.ToString() + Environment.NewLine);
            message.Append("quad3_sensor_type: " + quad3SensorType.ToString() + Environment.NewLine);
            message.Append("quad3_gauge_home: " + quad3_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad3_gauge_full: " + quad3_gauge_full.ToString() + Environment.NewLine);
            message.Append("quad3_hysteresis: " + quad3_gauge.ToString() + Environment.NewLine);
            message.Append("quad3_motor_weight: " + quad3SensorType.ToString() + Environment.NewLine);
            message.Append("quad3_curve: " + quad3_gauge_home.ToString() + Environment.NewLine);
            message.Append("quad3_warning_zone: " + quad3_warning_zone.ToString() + Environment.NewLine);
            message.Append("quad3_warning_level: " + quad3_warning_threshold.ToString() + Environment.NewLine);
            message.Append("quad3_RGBW_backlight_red: " + RGBW_backlight_red_quad3.ToString() + Environment.NewLine);
            message.Append("quad3_RGBW_backlight_green: " + RGBW_backlight_green_quad3.ToString() + Environment.NewLine);
            message.Append("quad3_RGBW_backlight_blue: " + RGBW_backlight_blue_quad3.ToString() + Environment.NewLine);


            // 0x5
            message.Append("gauge_backlight_input: " + backlight_input.ToString() + Environment.NewLine);
            message.Append("gauge_warning_flash: " + warning_light_backlight_flash.ToString() + Environment.NewLine);
            message.Append("gauge_pointer_type: " + gauge_pointer_type.ToString() + Environment.NewLine);
            message.Append("gauge_backlight_top_voltage: " + ((int)(gauge_backlight_top_voltage * 10)).ToString() + Environment.NewLine);
            message.Append("gauge_backlight_bot_voltage: " + ((int)(gauge_backlight_bot_voltage * 10)).ToString() + Environment.NewLine);
            message.Append("lcd_brightness: " + daytime_brightness.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_red: " + RGBW_pointer_red.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_green: " + RGBW_pointer_green.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_blue: " + RGBW_pointer_blue.ToString() + Environment.NewLine);
            message.Append("RGBW_pointer_white: " + RGBW_pointer_white.ToString() + Environment.NewLine);

            // 0x6
            message.Append("lcd_total_accumulationulation: " + lcd_total_accumulation.ToString() + Environment.NewLine);
            message.Append("enable_accumulation:" + enable_accumulation.ToString() + Environment.NewLine);
            message.Append("speedo_tach_units: " + speedo_tach_units.ToString() + Environment.NewLine);
            message.Append("speedo_sensor: " + speedo_sensor.ToString() + Environment.NewLine);
            message.Append("tach_sensor: " + tach_sensor.ToString() + Environment.NewLine);
            message.Append("Speedo PPM: " + speedo_PPM.ToString() + Environment.NewLine);
            message.Append("Tach PPR: " + tach_PPR.ToString() + Environment.NewLine);
            message.Append("gauge_output_level: " + gauge_output_level.ToString() + Environment.NewLine);
            message.Append("gauge_output_zone: " + gauge_output_zone.ToString() + Environment.NewLine);

            // 0x7
            message.Append("gauge_coefficient0: " + gauge_coefficient0 + Environment.NewLine);
            message.Append("gauge_coefficient1: " + gauge_coefficient1 + Environment.NewLine);


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
