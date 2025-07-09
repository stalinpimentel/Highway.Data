using System;

namespace Highway.Data.Interceptors.Events;

/// <summary>
///     The Event Arguments for a Pre-Save Interceptor to use
/// </summary>
public class BeforeSaveEventArgs : EventArgs
{
    public static readonly BeforeSaveEventArgs None = new();
}