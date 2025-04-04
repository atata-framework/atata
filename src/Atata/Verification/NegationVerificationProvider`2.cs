﻿namespace Atata;

public abstract class NegationVerificationProvider<TVerificationProvider, TOwner> : VerificationProvider<TVerificationProvider, TOwner>
    where TVerificationProvider : VerificationProvider<TVerificationProvider, TOwner>
{
    protected NegationVerificationProvider(IVerificationProvider<TOwner> verificationProvider)
        : base(verificationProvider.ExecutionUnit, isNegation: true)
    {
        Timeout = verificationProvider.Timeout;
        RetryInterval = verificationProvider.RetryInterval;
        Strategy = verificationProvider.Strategy;
        TypeEqualityComparerMap = verificationProvider.TypeEqualityComparerMap;
    }
}
