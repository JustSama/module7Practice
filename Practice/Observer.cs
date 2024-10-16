using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IObserver
{
    void Update(string stockSymbol, decimal newPrice);
}

public interface ISubject
{
    void Attach(IObserver observer, string stockSymbol);
    void Detach(IObserver observer, string stockSymbol);
    void Notify(string stockSymbol, decimal newPrice);
}

public class StockExchange : ISubject
{
    private readonly Dictionary<string, List<IObserver>> _observers = new Dictionary<string, List<IObserver>>();

    public void Attach(IObserver observer, string stockSymbol)
    {
        if (!_observers.ContainsKey(stockSymbol))
        {
            _observers[stockSymbol] = new List<IObserver>();
        }
        _observers[stockSymbol].Add(observer);
    }

    public void Detach(IObserver observer, string stockSymbol)
    {
        if (_observers.ContainsKey(stockSymbol))
        {
            _observers[stockSymbol].Remove(observer);
        }
    }

    public void Notify(string stockSymbol, decimal newPrice)
    {
        if (_observers.ContainsKey(stockSymbol))
        {
            foreach (var observer in _observers[stockSymbol])
            {
                observer.Update(stockSymbol, newPrice);
            }
        }
    }

    public void UpdateStockPrice(string stockSymbol, decimal newPrice)
    {
        Notify(stockSymbol, newPrice);
    }
}

public class Trader : IObserver
{
    public void Update(string stockSymbol, decimal newPrice)
    {
        Console.WriteLine($"Trader notified: {stockSymbol} price updated to {newPrice}");
    }
}

public class AutoTrader : IObserver
{
    private readonly decimal _buyThreshold;
    private readonly decimal _sellThreshold;

    public AutoTrader(decimal buyThreshold, decimal sellThreshold)
    {
        _buyThreshold = buyThreshold;
        _sellThreshold = sellThreshold;
    }

    public void Update(string stockSymbol, decimal newPrice)
    {
        if (newPrice <= _buyThreshold)
        {
            Console.WriteLine($"AutoTrader: Buying {stockSymbol} at {newPrice}");
        }
        else if (newPrice >= _sellThreshold)
        {
            Console.WriteLine($"AutoTrader: Selling {stockSymbol} at {newPrice}");
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        StockExchange stockExchange = new StockExchange();
        Trader trader = new Trader();
        AutoTrader autoTrader = new AutoTrader(100m, 150m);

        stockExchange.Attach(trader, "AAPL");
        stockExchange.Attach(autoTrader, "AAPL");

        stockExchange.UpdateStockPrice("AAPL", 95m);
        await Task.Delay(1000);
        stockExchange.UpdateStockPrice("AAPL", 105m);
        await Task.Delay(1000);
        stockExchange.UpdateStockPrice("AAPL", 155m);
        await Task.Delay(1000);
        stockExchange.UpdateStockPrice("AAPL", 90m);
    }
}
