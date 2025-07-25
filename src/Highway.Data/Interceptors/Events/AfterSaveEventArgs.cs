using System;

namespace Highway.Data.Interceptors.Events;

/// <summary>
///     The Event Arguments for a Post-Save Interceptor to use
/// </summary>
public class AfterSaveEventArgs : EventArgs
{
    public static readonly AfterSaveEventArgs None = new();
}