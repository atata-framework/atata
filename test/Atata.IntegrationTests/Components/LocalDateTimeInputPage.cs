namespace Atata.IntegrationTests;

using _ = LocalDateTimeInputPage;

[Url("localdatetimeinput")]
[VerifyTitle("Local Date/Time Input")]
public class LocalDateTimeInputPage : Page<_>
{
    public LocalDateTimeInput<_> Regular { get; private set; }

    [FindById]
    [ValueGetFormat("yyyy-MM-ddTHH:mm")]
    public DateTime<_> RegularOutput { get; private set; }
}
