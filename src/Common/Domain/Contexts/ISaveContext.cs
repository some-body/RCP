using Domain.Entities;
using System.Data.Entity;

namespace Domain.Contexts
{
    public interface ISaveContext
    {
        int SaveChanges();
    }
}
