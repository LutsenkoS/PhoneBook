using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBook.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using System.Data.Objects;

namespace PhoneBook
{
    public class FioRepository : IRepository<Fio>
    {
        
        private CatalogContext db;
        
        public FioRepository()
        {
            this.db = new CatalogContext();
        }
        
        public IEnumerable<Fio> GetList()
        {
            IEnumerable<Fio> fio =  db.Fios;
            return fio;
        }
        public Fio GetObject(int Id)
        {
            return db.Fios.Find(Id);
        }
        public void Create(Fio fio)
        {       
            db.Fios.Add(fio);
        }
       
         public void Update(Fio fio)
        {          
            var oldfio=db.Fios.Where(x => x.Id == fio.Id).FirstOrDefault();
            oldfio.Name = fio.Name;
            oldfio.Surname = fio.Surname;
        }
        public  void Delete(int Id)
        {           
            Fio fio = db.Fios.Find(Id);
            if (fio != null)
            {
                
                db.Fios.Remove(fio);
                
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
