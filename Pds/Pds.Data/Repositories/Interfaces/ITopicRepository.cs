﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pds.Data.Entities;

namespace Pds.Data.Repositories.Interfaces
{
    public interface ITopicRepository : IRepositoryBase<Topic>
    {
        Task<Topic> GetFirstWithPeopleAsync(Expression<Func<Topic, bool>> match);
        Task<List<Topic>> GetAllFullAsync();
    }
}