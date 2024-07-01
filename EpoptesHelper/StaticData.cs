using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace EpoptesHelper
{
    internal static class StaticData //Статический класс
    {
        public static List<User> _Users = ReadFile("assets/Users.txt"); //Список пользователей, текстовый файл в качестве источника
        public static Window _previousWindow; //Поле для предыдущего окна

        public static List<User> UsersInit()//Обновление списка пользователей с текстовым файлом в качетсве источника
        {
            return _Users = ReadFile("assets/Users.txt");
        }

        private static List<User> ReadFile(string path) //Получение данных из текстового файла и преобразование их в список объектов класса "Пользователь"
        {
            List<User> users = [];

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int i = 0;
                    string name;

                    while ((name = sr.ReadLine()) != null)
                    {
                        User user = new(i, name);
                        users.Add(user);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return users;
        }
    }
}