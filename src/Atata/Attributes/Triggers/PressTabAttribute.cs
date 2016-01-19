namespace Atata
{
    public class PressTabAttribute : PressKeysAttribute
    {
        public PressTabAttribute()
        {
            Keys = OpenQA.Selenium.Keys.Tab;
        }
    }
}
