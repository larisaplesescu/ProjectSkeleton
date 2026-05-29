namespace TheAdventure.Models.Shop;

public class Inventory<T> where T : notnull
{
    private readonly Dictionary<T, int> _items = new();

    public IReadOnlyDictionary<T, int> Items => _items;

    public void Add(T item, int count = 1)
    {
        if (_items.ContainsKey(item))
            _items[item] += count;
        else
            _items[item] = count;
    }

    public bool Remove(T item, int count = 1)
    {
        if (!_items.TryGetValue(item, out var current) || current < count)
            return false;
        _items[item] -= count;
        if (_items[item] <= 0)
            _items.Remove(item);
        return true;
    }

    public int Count(T item) => _items.TryGetValue(item, out var c) ? c : 0;
    public bool Has(T item, int count = 1) => Count(item) >= count;
    public void Clear() => _items.Clear();
}