﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinAttributeProgrammer.Services
{
    public interface IToastMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
