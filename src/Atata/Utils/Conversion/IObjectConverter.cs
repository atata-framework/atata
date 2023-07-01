namespace Atata;

public interface IObjectConverter
{
    object Convert(object sourceValue, Type destinationType);
}
