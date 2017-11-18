using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PhoneBook.Model
{
    class CatalogInitializer: DropCreateDatabaseIfModelChanges<CatalogContext>
    {

        protected override void Seed(CatalogContext context)
        {
            var fio = new List<Fio>
            {
                new Fio {Name="Sergey",Surname="Lutsenko"}
            };
            fio.ForEach(s => context.Fios.Add(s));
            context.SaveChanges();
            var phone = new List<Phone>
            {
                new Phone {FioId = 1, Number = "80972332222", pType = PhoneType.Домашний},
                new Phone {FioId = 1, Number = "02392948", pType = PhoneType.Рабочий}
            };
            phone.ForEach(s=>context.Phones.Add(s));
            context.SaveChanges();
        }
    }
}
