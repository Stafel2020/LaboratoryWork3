using ConsoleApp1;
using System.Formats.Tar;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    // Сервис для работы с играми
    static GameService GameService { get; set; }

    // Главный метод программы
    private static void Main(string[] args)
    {
        Console.WriteLine("Вас приветствует менеджер учёта партий!");

        WriteHelp();

        // Инициализация сервиса с указанием пути к файлу
        GameService = new GameService("Games.json");

        // Основной цикл программы для обработки команд
        while (true)
        {

            switch (Console.ReadLine())
            {
                case "1":
                    Add();
                    break;

                case "2":
                    Del();
                    break;

                case "3":
                    FullEntry();
                    break;

                case "4":
                    EntryDay();
                    break;

                case "5":
                    EntryPlayerDay();
                    break;

                case "6":
                    EntryTime();
                    break;

                case "7":
                    EntryWinLose();
                    break;

                case "8":
                    CountPlayerDay();
                    break;

                case "9":
                    Console.Clear();
                    break; 

                case "help":
                    WriteHelp();
                    break;

            }
        }
    }


    // Метод для вывода списка команд
    public static void WriteHelp()
    {
        Console.WriteLine("Список команд менеджера: ");
        Console.WriteLine("1. Добавить запись \n" +
            "2. Удалить запись \n" +
            "3. Получить список всех записей \n" +
            "4. Получить список за день \n" +
            "5. Список всех участников за день \n" +
            "6. Список партий по затраченому времени \n" +
            "7. Список побед и порожений определенного человека \n" +
            "8. Список игроков за день \n" +
            "9. Очистить консоль");
        Console.WriteLine("help - помощь");
    }

    // Метод для добавления новой игры
    public static void Add()
    {
        Console.WriteLine("Введите дату: ");
        DateTime DATE = GetDate();

        Console.WriteLine("Введите имя первого участника: ");
        string One = Console.ReadLine();

        Console.WriteLine("Введите имя второго участника: ");
        string Two = Console.ReadLine();

        Console.WriteLine("Введите статус для первого участника: ");

        Console.WriteLine("1. Победа у первого игрока \n" +
            "2. Поражение у первого игрока \n" +
            "3. Ничья");

        Status STATUS = Status.Tie;

        switch (Console.ReadLine())
        {
            case "1":
                STATUS = Status.FirstPlayerWon;
                break;

            case "2":
                STATUS = Status.SecondPlayerWon;
                break;
                
            case "3":
                STATUS = Status.Tie;
                break; 

        }

        Console.WriteLine("Введите длительность партии: ");
        TimeSpan TIMESPAN = GetTime();

        Game GAME = new Game(DATE, TIMESPAN, STATUS, One, Two);
        GameService.AddGame(GAME);

        Console.WriteLine("Вы успешно добавили запись!");
    }

    // Метод для удаления игры
    public static void Del()
    {
        DateTime DATATIME = GetDate();
        GameService.RemoveGame(DATATIME);

        Console.WriteLine("Вы успешно удалили запись!");
    }

    // Метод для вывода всех игр
    public static void FullEntry()
    {

        foreach(Game EntryGame in GameService.Games)
        {
            Console.WriteLine(EntryGame);
        }
    }

    // Метод для вывода игр за определённый день
    public static void EntryDay()
    {
        Console.WriteLine("Введите дату для поиска партии:");

        foreach (Game DayEntry in GameService.DayEntry(GetDate()))
        {
            Console.WriteLine(DayEntry);
        }
    }

    // Метод для вывода игроков за определённый день
    public static void EntryPlayerDay() 
    {
        Console.WriteLine("Введите дату для поиска игроков:");

        foreach (string PlayerDayEntry in GameService.PlayersOrDay(GetDate()))
        {
            Console.WriteLine(PlayerDayEntry);
        }
    }

    // Метод для вывода игр по продолжительности партии
    public static void EntryTime()
    {
        Console.WriteLine("Введите минимальное количество минут");
        int OneMinutes = GetInt();

        Console.WriteLine("Введите максимальное количество минут");
        int TwoMinutes = GetInt();

        foreach (Game TimeEntry in GameService.GamesByTimes(OneMinutes, TwoMinutes))
        {
            Console.WriteLine(TimeEntry);
        }
    }

    // Метод для вывода статистики побед и поражений определенного игрока
    public static void EntryWinLose()
    {
        Console.WriteLine("Введите имя игрока: ");
        string value = Console.ReadLine();

        double winrate = GameService.WinRate(value);

        Console.WriteLine($"Процент побед игрока: {winrate * 100}%");
        Console.WriteLine($"Процент поражений игрока: {100 - (winrate * 100)}%");
    }

    // Метод для получения времени в формате TimeSpan
    public static TimeSpan GetTime()
    {
        int TimeMinutes = GetInt();
        return TimeSpan.FromMinutes(TimeMinutes);
    }

    // Метод для получения целого числа с обработкой ошибок
    public static int GetInt()
    {
        string ValueTime = Console.ReadLine();

        try
        {
            return int.Parse(ValueTime);
        }
        catch
        {
            Console.WriteLine("Вы ввели неправильное значение! Менеджер возвращает 0");
            return 0;
        }

    }

    // Метод для получения даты с обработкой ошибок
    public static DateTime GetDate()
    {
        Console.WriteLine("Введите год!");
        int Year = GetInt();

        Console.WriteLine("Введите месяц!");
        int Month = GetInt();

        Console.WriteLine("Введите день!");
        int Day = GetInt();

        try
        {
            return new DateTime(Year, Month, Day);
        }
        catch
        {
            Console.WriteLine("Неправильно введена дата, возвращаем вас в прошлое!");
            return new DateTime (0001, 01, 01);
        }
    }

    // Метод для вывода количества игроков за определённый день
    private static void CountPlayerDay()
    {
        Console.WriteLine("Введите дату для поиска числа игроков:");

        Console.WriteLine(GameService.PlayersOrDay(GetDate()).Count() + " игроков найдено за день");
    }
}