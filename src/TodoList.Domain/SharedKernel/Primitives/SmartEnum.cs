using System.Reflection;

namespace TodoList.Domain.SharedKernel.Primitives;

public abstract record SmartEnum<TEnum>
    where TEnum : SmartEnum<TEnum>
{
    private static readonly Lazy<Dictionary<int, TEnum>> Enumerations = new(() => CreateEnumerations());

    protected SmartEnum(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public int Value { get; protected init; }

    public string Name { get; protected init; }

    public static IReadOnlyCollection<TEnum> GetValues => [.. Enumerations.Value.Values];

    public static TEnum? FromValue(int value)
    {
        return Enumerations.Value.TryGetValue(
            value,
            out TEnum? enumeration) ?
                enumeration :
                default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations
            .Value
            .Values
            .SingleOrDefault(e => e.Name == name);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        Type enumerationType = typeof(TEnum);

        IEnumerable<TEnum> fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumerationType.IsAssignableFrom(fieldInfo.DeclaringType!))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(e => e.Value);
    }
}
