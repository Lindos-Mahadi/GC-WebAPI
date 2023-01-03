﻿using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IFileUploadRepository : IRepository<FileUpload>
    {

    }
    public class FileUploadRepository : RepositoryBaseCodeFirst<FileUpload>, IFileUploadRepository
    {
        public FileUploadRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
