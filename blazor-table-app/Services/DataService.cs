using BlazorTableApp.Models;

namespace BlazorTableApp.Services;

public class DataService
{
    private List<ValueItem> _items = new();

    public DataService()
    {
        Generate(300);
    }

    public IReadOnlyList<ValueItem> GetAll() => _items;

    public void Generate(int count)
    {
        var rnd = new Random(12345);
        _items = Enumerable.Range(1, count).Select(i => new ValueItem
        {
            Id = i,
            Value = Math.Round((decimal)(rnd.NextDouble() * 1000), 2),
            Timestamp = DateTime.UtcNow.AddMinutes(-i)
        }).ToList();
    }

    public void Refresh() => Generate(_items.Count);
}
