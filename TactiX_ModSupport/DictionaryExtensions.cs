namespace TactiX_ModSupport;

public static class DictionaryExtensions
{
    public static void EnsureKeyExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TValue : new()
    {
        if (!dictionary.ContainsKey(key)) dictionary[key] = new TValue();
    }

    public static void EnsureKeyExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue defaultValue)
    {
        if (!dictionary.ContainsKey(key)) dictionary[key] = defaultValue;
    }
}