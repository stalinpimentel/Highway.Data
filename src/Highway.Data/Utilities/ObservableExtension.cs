using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq;

public static class ObservableExtension
{
    public static ObservableCollection<T> ToObservableList<T>(this IEnumerable<T> data) => new (data);
}