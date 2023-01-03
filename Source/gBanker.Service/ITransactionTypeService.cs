﻿using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface ITransactionTypeService : IServiceBase<TransactionType>
    {


    }
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TransactionTypeService(ITransactionTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<TransactionType> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.TransactionId);
            return entities;
        }

        public TransactionType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public TransactionType Create(TransactionType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(TransactionType objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public TransactionType Get(Expression<Func<TransactionType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<TransactionType> GetMany(Expression<Func<TransactionType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }                
        public TransactionType GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        
    }
}
