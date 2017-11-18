using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Model
{
    public class Phone
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public PhoneType pType { get; set; }
        public int FioId { get; set; }
        public virtual Fio Fio { get; set; }
        //public Fio Fio { get; set; }
    }

    public enum PhoneType { Мобильный, Рабочий, Домашний }
}
