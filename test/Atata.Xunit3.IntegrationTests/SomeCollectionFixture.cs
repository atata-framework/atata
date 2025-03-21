﻿namespace Atata.Xunit3.IntegrationTests;

public sealed class SomeCollectionFixture : AtataCollectionFixture
{
    public SomeCollectionFixture()
        : base(SomeCollection.Name)
    {
    }

    protected override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        base.ConfigureAtataContext(builder);

        builder.UseVariable(nameof(SomeCollectionFixture), true);
    }
}
