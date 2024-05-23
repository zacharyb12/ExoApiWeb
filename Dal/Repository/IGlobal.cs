using Models.Dto.UserDto;
using Models.Entity.CreateUser;
using Models.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public interface IGlobal<T>
    {
        IEnumerable<T> Get();

        T GetById(Guid id);
        
        T Add(T entity);
        
        bool Update(T entity, Guid id);
        
        bool Delete(Guid id);
    }

}
