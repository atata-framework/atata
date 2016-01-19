namespace Atata
{
    public class PressEnterAttribute : PressKeysAttribute
    {
        public PressEnterAttribute()
        {
            Keys = OpenQA.Selenium.Keys.Enter;
        }
    }
}
