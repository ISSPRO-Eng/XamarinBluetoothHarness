using XamarinAttributeProgrammer.Converters.Base;

namespace XamarinAttributeProgrammer.Converters
{
    public class InvertedBooleanConverter : BaseConverter<bool, bool>
    {
        protected override bool ConvertFrom(bool value, object parameter)
        {
            return !value;
        }

        protected override bool ConvertBackTo(bool value, object parameter)
        {
            return !value;
        }
    }
}