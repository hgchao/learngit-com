using App.Base.EntityFramework;
using App.Core.Common.Entities;
using App.Core.Common.EntityFramework;
using App.Core.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Base.Repositories
{
    public class AppRepositoryBase<T> : RepositoryBase<T>, IAppRepositoryBase<T>
        where T : Entity
    {
        private IAppDbContextProvider _dbContextProvider;
        public override AppCoreDbContext DbContext => _dbContextProvider.GetDbContext();
        public AppRepositoryBase(IAppDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
    }
}
