namespace Atata;

public interface IObjectConverter
{
    TDestination Convert<TDestination>(object sourceValue);

    object Convert(object sourceValue, Type destinationType);
}
