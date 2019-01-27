namespace UniversalRepository.Abstractions
{
    using System.Data;
    using System.Data.SqlClient;

    using UniversalRepository.Models;

    public abstract class RepositoryBase
    {
        #region PrivateFields

        private readonly ConnectionConfig _connectionConfig;

        #endregion

        #region Constructor

        protected RepositoryBase(ConnectionConfig connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        #endregion

        #region Properties

        protected IDbConnection Connection => new SqlConnection(_connectionConfig.ConnectionString);

        #endregion
    }
}