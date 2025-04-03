﻿namespace Atata;

/// <summary>
/// Represents the base class for popup boxes.
/// </summary>
/// <typeparam name="TPopupBox">The type of the popup box.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public abstract class PopupBox<TPopupBox, TOwner>
    where TPopupBox : PopupBox<TPopupBox, TOwner>
    where TOwner : PageObject<TOwner>
{
    private protected PopupBox(TOwner owner) =>
        Owner = owner;

    /// <summary>
    /// Gets the text of popup box.
    /// </summary>
    public ValueProvider<string, TPopupBox> Text => new(
        (TPopupBox)this,
        new LazyObjectSource<string>(() => Alert.Text),
        $"{KindName} text",
        Owner.Session.ExecutionUnit);

    private protected TOwner Owner { get; }

    private protected IAlert Alert { get; private set; }

    private protected abstract string KindName { get; }

    internal TPopupBox WaitForAppearance(TimeSpan? waitTimeout, TimeSpan? waitRetryInterval)
    {
        Owner.Log.ExecuteSection(
            new LogSection($"Wait for {KindName}", LogLevel.Trace),
            () =>
            {
                RetryWait wait = new(waitTimeout ?? Owner.Session.WaitingTimeout, waitRetryInterval ?? Owner.Session.WaitingRetryInterval);
                IAlert alert = null;

                wait.Until(
                    driver =>
                    {
                        try
                        {
                            alert = driver.SwitchTo().Alert();
                            return true;
                        }
                        catch (NoAlertPresentException)
                        {
                            return false;
                        }
                    },
                    Owner.Driver);

                Alert = alert
                    ?? throw new TimeoutException($"Timed out waiting for {KindName}.");
            });

        return (TPopupBox)this;
    }

    /// <summary>
    /// Accepts the popup box.
    /// </summary>
    /// <returns>The owner page object.</returns>
    public TOwner Accept()
    {
        Owner.Log.ExecuteSection(
            new LogSection($"Accept {KindName}"),
            Alert.Accept);

        return Owner;
    }
}
