using Lab6.Database;
using Lab6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lab6.Repositories.Implementations
{
    public class DocumentRepository : BaseRepository<Document>
    {
        public DocumentRepository(ApplicationContext context) : base(context) { }

        public override Document Get(Guid id)
        {
            return Context.Documents
                          .Include(d => d.Worker)
                          .Include(d => d.Car)
                          .FirstOrDefault(m => m.Id == id);
        }
    }
}
