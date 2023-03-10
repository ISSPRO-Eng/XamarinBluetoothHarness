using System;
using System.Collections.Generic;
using System.Text;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.ViewModels;

namespace XamarinAttributeProgrammer.Resources
{
    public static class GaugeCommands
    {
        // To learn more about the command, you can look at the following function in the gauge's executable source code
        // - isspro_ble_com.c : nus_data_handler() : to see which how a packet is defined to what category (look at appBLEMessageEventT enum in appManager.h)
        // - attribute.c : attributeFillRAMCopy() : to see how it process the categories
        //COMMANDS FOR ATTRIBUTE PACKETS
        static readonly string BLE_COM_ATTR_PACK_0             =  "0x0"; // value for "0x0" NOTE: LSB is the first ASCII Character
        static readonly string BLE_COM_ATTR_PACK_1             =  "0x1"; // value for "0x1"
        static readonly string BLE_COM_ATTR_PACK_2             =  "0x2"; // value for "0x2"
        static readonly string BLE_COM_ATTR_PACK_3             =  "0x3"; // value for "0x3"
        static readonly string BLE_COM_ATTR_PACK_4             =  "0x4"; // value for "0x4" This is for saving the color
        static readonly string BLE_COM_ATTR_PACK_5             =  "0x5"; // value for "0x5"
        static readonly string BLE_COM_ATTR_PACK_6             =  "0x6"; // value for "0x6"
        static readonly string BLE_COM_ATTR_PACK_7             =  "0x7"; // value for "0x7"
        //COMMANDS FOR REQUESTING GAUGE INFO
        static readonly string BLE_COM_CMD_SEND_ATTRS          = "1x0"; // value for "1x0"
        static readonly string BLE_COM_CMD_SEND_SN_BIN         = "1x1"; // value for "1x1" - send mobile app the gauges programmed serial ID #
        static readonly string BLE_COM_CMD_SEND_REV            = "1x2"; // value for "1x2" - send mobile app the gauges Board ID #
        static readonly string BLE_COM_CMD_FACT_RESET          = "1x3"; // value for "1x3" - use attributes from Factory block location
        static readonly string BLE_COM_CMD_WARN_TOGGLE         = "1x4"; // value for "1x4" - toggle warning light
        static readonly string BLE_COM_CMD_WARN_FLASH_TOGGLE   = "1x5"; // value for "1x5" - toggle warning light Flash Effect
        static readonly string BLE_COM_CMD_OUTPUT_TOGGLE       = "1x6"; // value for "1x6" - toggle output driver voltage
        static readonly string BLE_COM_CMD_RAWADC_TOGGLE       = "1x7"; // value for "1x7" - toggle raw ADC output via BLE messages

        //COMMANDS FOR PRODUCTION/FACTORY APPLICATION PROGRAMMING ONLY
        static readonly string BLE_COM_PROD_PRG_ON             = "9p0"; // value for "9p0"
        static readonly string BLE_COM_PROD_SN_PACK            = "9p1"; // value for "9p1"
        static readonly string BLE_COM_PROD_TEST_ON            = "9p2"; // value for "9p2"
        static readonly string BLE_COM_PROD_SEND_EEPROM        = "9p3"; // value for "9p3" - send mobile app the data inside our NVM Block

        static readonly string BLE_COM_NAME_CHANGE             = "GNC"; // value for "GNC"
        static readonly string BLE_COM_MESSAGE_ACK             = "-:'ACK'"; // "-:'ACK'" message header
        static readonly string BLE_COM_MESSAGE_NACK            = "-:'NAK'"; // " -:'NAK'" message header


        static public byte[] ATTR_PACK_0            { get => StringToBytes(BLE_COM_ATTR_PACK_0 );}
        static public byte[] ATTR_PACK_1            { get => StringToBytes(BLE_COM_ATTR_PACK_1 );}
        static public byte[] ATTR_PACK_2            { get => StringToBytes(BLE_COM_ATTR_PACK_2 );}
        static public byte[] ATTR_PACK_3            { get => StringToBytes(BLE_COM_ATTR_PACK_3 );}
        static public byte[] ATTR_PACK_4            { get => StringToBytes(BLE_COM_ATTR_PACK_4 );}
        static public byte[] ATTR_PACK_5            { get => StringToBytes(BLE_COM_ATTR_PACK_5 );}
        static public byte[] ATTR_PACK_6            { get => StringToBytes(BLE_COM_ATTR_PACK_6 );}
        static public byte[] ATTR_PACK_7            { get => StringToBytes(BLE_COM_ATTR_PACK_7); }
        static public byte[] ATTRS                  { get => StringToBytes(BLE_COM_CMD_SEND_ATTRS); }
        static public byte[] SN_BIN                 { get => StringToBytes(BLE_COM_CMD_SEND_SN_BIN);}
        static public byte[] REV                    { get => StringToBytes(BLE_COM_CMD_SEND_REV);}
        static public byte[] FACT_RESET             { get => StringToBytes(BLE_COM_CMD_FACT_RESET);}
        static public byte[] WARN_TOGGLE            { get => StringToBytes(BLE_COM_CMD_WARN_TOGGLE);}
        static public byte[] WARN_FLASH_TOGGLE      { get => StringToBytes(BLE_COM_CMD_WARN_FLASH_TOGGLE);}
        static public byte[] OUTPUT_TOGGLE          { get => StringToBytes(BLE_COM_CMD_OUTPUT_TOGGLE);}
        static public byte[] RAWADC_TOGGLE          { get => StringToBytes(BLE_COM_CMD_RAWADC_TOGGLE);}
        static public byte[] PROD_PRG_ON            { get => StringToBytes(BLE_COM_PROD_PRG_ON );}
        static public byte[] PROD_SN_PACK           { get => StringToBytes(BLE_COM_PROD_SN_PACK);}
        static public byte[] PROD_TEST_ON           { get => StringToBytes(BLE_COM_PROD_TEST_ON);}
        static public byte[] PROD_SEND_EEPROM       { get => StringToBytes(BLE_COM_PROD_SEND_EEPROM); }
        static public byte[] NAME_CHANGE            { get => StringToBytes(BLE_COM_NAME_CHANGE );}
        static public byte[] MESSAGE_ACK            { get => StringToBytes(BLE_COM_MESSAGE_ACK );}
        static public byte[] MESSAGE_NACK           { get => StringToBytes(BLE_COM_MESSAGE_NACK); }

        public static byte[][] ProductInfoCmd()
        {
            byte[][] cmds = new byte[][]{ SN_BIN, REV };
            return cmds;
        }

        public static void ProcessGaugeResponse(string msg)
        {
            string Cmd = msg.Substring(0, Math.Min(3, msg.Length));
            switch (Cmd)
            {
                case "0x1": // These are all the attributes
                case "0x2":
                case "0x3":
                case "0x4":
                case "0x5":
                case "0x6":
                case "0x7":
                    break;
                case "1x1": // gauges programmed serial ID #System.Globalization.NumberStyles.HexNumber));
                    break;
                case "1x2": // gauges Board ID #
                    App.AttributeManager.setDeviceVersion(msg.Substring(Math.Min(msg.Length, 3)));
                    break;
                case "EX\u0006": // ACK from the server, but this prevented a crash on android
                    break;
                default:
                    //Log.e(TAG, "BLE message not recognized!" + Cmd);
                    break;
            }
        }

        public static byte[] ColorPreviewReq(ColorContainer backLightColor, ColorContainer needlColor)
        {
            StringBuilder message = new StringBuilder();
            GaugeAttributes att = App.AttributeManager;
            //Attach 16 character payloads to each 3 character message header (total of 19 character messages will be sent over BLE per attribute message)
            message.Append("0x0");
            if (backLightColor != null)
            {
                message.Append(backLightColor.R.ToString("X2"));
                message.Append(backLightColor.G.ToString("X2"));
                message.Append(backLightColor.B.ToString("X2"));
                message.Append("00");
            }
            else
            {
                message.Append(att.getQuad0BacklightColorRed().ToString("X2"));
                message.Append(att.getQuad0BacklightColorGreen().ToString("X2"));
                message.Append(att.getQuad0BacklightColorBlue().ToString("X2"));
            }

            if (needlColor != null)
            {
                message.Append(needlColor.R.ToString("X2"));
                message.Append(needlColor.G.ToString("X2"));
                message.Append(needlColor.B.ToString("X2"));
                message.Append("00");
            }
            else
            {
                message.Append(att.getPointerColorRed().ToString("X2"));
                message.Append(att.getPointerColorGreen().ToString("X2"));
                message.Append(att.getPointerColorBlue().ToString("X2"));
            }

            return StringToBytes(message.ToString());
        }

        // The following does not work. Not sure why to be honest, but I'll leave it here encase we ever need it
        public static byte[] FlashColorCmd()
        {
            StringBuilder message = new StringBuilder();
            GaugeAttributes att = App.AttributeManager;
            message.Append("0x4");
            // 53 characters by this point
            message.Append(att.getQuad0BacklightColorRed().ToString("X2"));
            message.Append(att.getQuad0BacklightColorGreen().ToString("X2"));
            message.Append(att.getQuad0BacklightColorBlue().ToString("X2"));
            message.Append(att.getPointerColorRed().ToString("X2")); ;
            message.Append(att.getPointerColorGreen().ToString("X2"));
            message.Append(att.getPointerColorBlue().ToString("X2"));

            byte[] cmd = StringToBytes(message.ToString());
            return cmd;
        }

        public static byte[][] FlashCommand()
        {
            List<byte[]> cmd = new List<byte[]>();
            string[] msgs = App.AttributeManager.attributeMessagePackage().Split(' ');
            foreach (string s in msgs)
            {
                cmd.Add(StringToBytes(s));
            }
            return cmd.ToArray();
        }

        public static byte[] ReqNameChange(string newName)
        {
            newName = newName.Substring(0, Math.Min(17, newName.Length)); // Max of 17 chars. Gauge does not have a length check!
            string msg = $"GNC {newName}";
            return StringToBytes(msg);
        }
        public static byte[] StringToBytes(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        public static string SafeSubstring(string value, int startIndex, int length)
        {
            return value.Substring(Math.Max(0, startIndex), Math.Min(length, value.Length - startIndex));
        }
    }
}
