using XamarinAttributeProgrammer.Helpers.EnumBindableViews.DataTemplates;
using XamarinAttributeProgrammer.Helpers.EnumBindableViews.Models;

using System;

using Xamarin.Forms;

namespace XamarinAttributeProgrammer.Helpers.EnumBindableViews
{
    public class EnumBindableCheckboxes<T> : EnumBindableCollectionView<T> where T : struct, Enum
    {

        public EnumBindableCheckboxes()
        {
            BindableLayout.SetItemTemplate(this, new DataTemplate(typeof(EnumCheckboxDataTemplate)));
        }


    }
}