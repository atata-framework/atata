﻿namespace Atata.NUnit.IntegrationTests.SomeNamespace;

public sealed class NamespaceFixture : AtataNamespaceFixture
{
    protected override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        base.ConfigureAtataContext(builder);

        builder.UseVariable(nameof(NamespaceFixture), true);
    }
}
