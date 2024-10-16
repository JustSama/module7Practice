using System;

public interface ICostCalculationStrategy
{
    decimal CalculateCost(TravelDetails travelDetails);
}

public class AirplaneStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(TravelDetails travelDetails)
    {
        decimal baseCost = travelDetails.Distance * 0.5m;
        if (travelDetails.Class == "Business")
            baseCost *= 1.5m;

        return ApplyDiscount(baseCost, travelDetails);
    }

    private decimal ApplyDiscount(decimal cost, TravelDetails travelDetails)
    {
        if (travelDetails.IsChild)
            return cost * 0.8m;
        if (travelDetails.IsSenior)
            return cost * 0.9m;
        return cost;
    }
}

public class TrainStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(TravelDetails travelDetails)
    {
        decimal baseCost = travelDetails.Distance * 0.3m;
        if (travelDetails.Class == "Business")
            baseCost *= 1.2m;

        return ApplyDiscount(baseCost, travelDetails);
    }

    private decimal ApplyDiscount(decimal cost, TravelDetails travelDetails)
    {
        if (travelDetails.IsChild)
            return cost * 0.8m;
        if (travelDetails.IsSenior)
            return cost * 0.85m;
        return cost;
    }
}

public class BusStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(TravelDetails travelDetails)
    {
        decimal baseCost = travelDetails.Distance * 0.2m;
        if (travelDetails.Class == "Business")
            baseCost *= 1.1m;

        return ApplyDiscount(baseCost, travelDetails);
    }

    private decimal ApplyDiscount(decimal cost, TravelDetails travelDetails)
    {
        if (travelDetails.IsChild)
            return cost * 0.7m;
        if (travelDetails.IsSenior)
            return cost * 0.85m;
        return cost;
    }
}

public class TravelBookingContext
{
    private ICostCalculationStrategy _strategy;

    public void SetStrategy(ICostCalculationStrategy strategy)
    {
        _strategy = strategy;
    }

    public decimal CalculateTravelCost(TravelDetails travelDetails)
    {
        if (_strategy == null)
            throw new InvalidOperationException("Стратегия не установлена");
        return _strategy.CalculateCost(travelDetails);
    }
}

public class TravelDetails
{
    public decimal Distance { get; set; }
    public string Class { get; set; }
    public bool IsChild { get; set; }
    public bool IsSenior { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        TravelBookingContext bookingContext = new TravelBookingContext();
        TravelDetails travelDetails = new TravelDetails();

        Console.WriteLine("Выберите тип транспорта (1 - Самолет, 2 - Поезд, 3 - Автобус): ");
        int choice = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите расстояние (км): ");
        travelDetails.Distance = decimal.Parse(Console.ReadLine());

        Console.WriteLine("Выберите класс (Эконом, Бизнес): ");
        travelDetails.Class = Console.ReadLine();

        Console.WriteLine("Дети (да/нет)? ");
        travelDetails.IsChild = Console.ReadLine().ToLower() == "да";

        Console.WriteLine("Пенсионеры (да/нет)? ");
        travelDetails.IsSenior = Console.ReadLine().ToLower() == "да";

        switch (choice)
        {
            case 1:
                bookingContext.SetStrategy(new AirplaneStrategy());
                break;
            case 2:
                bookingContext.SetStrategy(new TrainStrategy());
                break;
            case 3:
                bookingContext.SetStrategy(new BusStrategy());
                break;
            default:
                Console.WriteLine("Неверный выбор");
                return;
        }

        decimal cost = bookingContext.CalculateTravelCost(travelDetails);
        Console.WriteLine($"Стоимость поездки: {cost}");
    }
}
