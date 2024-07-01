using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpoptesHelper
{
    internal class User //Пользователь
    {
        private int _id; //ID
        private string _name; //Имя

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public User(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}
