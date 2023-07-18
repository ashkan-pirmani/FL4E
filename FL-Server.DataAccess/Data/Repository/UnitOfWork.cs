using FL_Server.Data;
using FL_Server.DataAccess.Data.Repository.IRepository;

namespace FL_Server.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            SP_Call = new SP_Call(_db);
            User = new UserRepository(_db);
        }


        public ISP_Call SP_Call { get; private set; }
        public IUserRepository User { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
