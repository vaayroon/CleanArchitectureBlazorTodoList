using System.Collections.ObjectModel;

namespace TodoList.Domain.SharedKernel.Extensions;

public static class LoopExtension
{
    public static IEnumerable<(T Item, int Index)> LoopIndex<T>(this IEnumerable<T> self) =>
        self.Select((item, index) => (item, index));

    public static IEnumerable<(T Item, int Index)> LoopIndex<T>(this Collection<T> self) =>
        self.Select((item, index) => (item, index));
}
