﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pds.Data;
using Pds.Data.Entities;
using Pds.Services.Interfaces;

namespace Pds.Services.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<(Person[] people, int total)> GetAsync(int limit = 10, int offset = 0)
        {
            var result = await unitOfWork.Persons.GetAllWithResourcesAsync(limit, offset);
            var total = await unitOfWork.Persons.Count();

            return (result, total);
        }

        public async Task<Guid> CreateAsync(Person newPerson)
        {
            if (newPerson == null)
            {
                throw new ArgumentNullException(nameof(newPerson));
            }
            
            newPerson.CreatedAt = DateTime.UtcNow;
            var result = await unitOfWork.Persons.InsertAsync(newPerson);

            return result.Id;
        }
    }
}