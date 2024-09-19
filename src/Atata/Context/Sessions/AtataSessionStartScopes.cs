﻿namespace Atata;

/// <summary>
/// Specifies <see cref="AtataContext"/> scopes for which an <see cref="AtataSession"/> should automatically start.
/// </summary>
[Flags]
public enum AtataSessionStartScopes
{
    /// <summary>
    /// Should not start automatically, but can be started on-demand.
    /// </summary>
    None = 0,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.Test"/> scope.
    /// </summary>
    Test = 0x0001,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.TestSuite"/> scope.
    /// </summary>
    TestSuite = 0x0010,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.Global"/> scope.
    /// </summary>
    Global = 0x0100
}