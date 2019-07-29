using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Base.Repositories
{
    public interface IAppRepositoryBase<T>: IRepositoryBase<T>
        where T: Entity
    {
    }
}
