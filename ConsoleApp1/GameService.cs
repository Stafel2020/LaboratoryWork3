using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp1
{
    // Сервис для работы с играми
    internal class GameService
    {

        // Путь к файлу, где хранятся игры
        string PATH {get;set;}

        // Конструктор для инициализации сервиса
        public GameService(string Path) 
        { 
            PATH = Path;

            if(!File.Exists(PATH))
            {
                Games = new List<Game>();
                return;
            }
            string FILE = File.ReadAllText(PATH);

            try
            {
                Games = JsonSerializer.Deserialize<List<Game>>(FILE);
            }
            catch (Exception)
            {
                Console.WriteLine("Не удалось десириализовать файл, будет создан новый список игр.");
                Games = new List<Game>();
            }
        }

        // Список игр
        public List<Game> Games { get; set; }

        // Метод для добавления новой игры
        public void AddGame(Game GAME)
        {
            Games.Add(GAME);
            Save();
        }

        // Метод для удаления игры по дате
        public void RemoveGame(DateTime date) 
        {
            try 
            { 
                Games.Remove(Games.First(g => g.Date == date)); 
            }

            catch (Exception)
            {
                Console.WriteLine("Запись не найдена");
            }

            Save();
        }

        // Метод для сохранения списка игр в файл
        public void Save()
        {
            File.WriteAllText(PATH, JsonSerializer.Serialize(Games));
        }

        // Метод для получения игр за определённый день
        public List<Game> DayEntry(DateTime date)
        {
           return Games.Where(g =>  g.Date == date).ToList();
        }

        // Метод для получения списка игроков за определённый день
        public List<string> PlayersOrDay(DateTime date)
        {
            List<string> OnePlayers = Games.Where(g => g.Date == date).Select(g => g.PlayerNameOne).ToList();
            List<string> TwoPlayers = Games.Where(g => g.Date == date).Select(g => g.PlayerNameTwo).ToList();
            return OnePlayers.Concat(TwoPlayers).Distinct().ToList();
        }

        // Метод для вычисления процента поражений и побед определенного игрока
        public double WinRate(string PlayerName)
        {
            int WinOne = Games.Where(g => g.PlayerNameOne == PlayerName).Count(g => g.status == Status.FirstPlayerWon);
            int WinTwo = Games.Where(g => g.PlayerNameTwo == PlayerName).Count(g => g.status == Status.SecondPlayerWon);

            try
            {
                return (WinOne + WinTwo) / Games.Where(g => g.PlayerNameOne == PlayerName || g.PlayerNameTwo == PlayerName).Count();
            }

            catch 
            { 
                Console.WriteLine("Такой игрок не найден"); return 0; 
            }




        }

        // Метод для получения игр по времени продолжительности
        public List<Game> GamesByTimes (int MinMinutes, int MaxMinutes)
        {
            return Games.Where(g => g.Time.TotalMinutes > MinMinutes && g.Time.TotalMinutes < MaxMinutes).ToList();
        }
    }

}
