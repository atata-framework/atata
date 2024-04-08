namespace Atata;

public class WebDriverSessionBuilder : AtataSessionBuilder
{
    public override AtataSession Build(AtataContext context)
    {
        var session = new WebDriverSession(context);

        session.Start();

        return session;
    }
}
