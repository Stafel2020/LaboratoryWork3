using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // Класс, представляющий игру
    public class Game
    {
        // Конструктор для инициализации объекта
        public Game(DateTime date, TimeSpan time, Status status, string playerNameOne, string playerNameTwo)
        {
            Date = date;
            Time = time;
            this.status = status;
            PlayerNameOne = playerNameOne;
            PlayerNameTwo = playerNameTwo;


        }

        // Свойства игры
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Status status { get; set; }

        public string PlayerNameOne { get; set; }
        public string PlayerNameTwo { get; set; }

        // Переопределение метода ToString для удобного отображения информации об игре
        public override string ToString()
        {
            return $"Дата: {Date:d} \n" +
                $"Время партии: {Time:c} \n" +
                $"Статус партии: {status} \n" +
                $"Имя первого игрока: {PlayerNameOne} \n" +
                $"Имя второго игрока: {PlayerNameTwo} \n" +
                $"";
            
        }
    }

    // Перечисление возможных статусов игры
    public enum Status
    {
        FirstPlayerWon, SecondPlayerWon, Tie
    }


}
