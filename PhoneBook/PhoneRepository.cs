using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PhoneBook.Model;
using System.Data.Entity.Infrastructure;

namespace PhoneBook
{
    class PhoneRepository: IRepository<Phone>
    {
         private CatalogContext db;
        
        public PhoneRepository()
        {
            this.db = new CatalogContext();
        }
        public IEnumerable<Phone> GetList()
        {
            IEnumerable<Phone> phone = db.Phones;
            return phone;
        }
        public Phone GetObject(int Id)
        {
            return db.Phones.Find(Id);
        }
        public void Create(Phone phone)
        {
            db.Phones.Add(phone);
        }
        public  void Update(Phone phone)
        {          
            var oldphone = db.Phones.Where(x => x.Id == phone.Id).FirstOrDefault();
            oldphone.pType = phone.pType;
            oldphone.Number = phone.Number;
        }
        public void Delete(int Id)
        {
            Phone phone = db.Phones.Find(Id);
            if (phone != null)
            {
                db.Phones.Remove(phone);
            }

        }
        async public void Save()
        {
            await db.SaveChangesAsync();
          
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
       
    }
}
