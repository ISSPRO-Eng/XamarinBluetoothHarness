using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using XamarinAttributeProgrammer.Resources;

namespace XamarinAttributeProgrammer.Models
{
    /// <summary>
    /// Purpose is to retrive values from the Resources.resx
    /// </summary>
    public static class Resourcer
    {
        private static ResourceManager ResMngr = StringValues.ResourceManager;

        public static string Warning { get => getResStrVal("warning"); }
        public static string Error { get => getResStrVal("errorTitle"); }
        public static string Success { get => getResStrVal("successTitle"); }
        public static string Cancel { get => getResStrVal("Cancel"); }
        public static string Ok { get => getResStrVal("Ok"); }
        public static string getResStrVal(string key)
        {
            // Return the value for the key, otherwise return null
            try
            {
                string value = ResMngr.GetString(key) ?? null;
                return value;
            }
            catch
            {
                return null;
            }
        }
    }
}
