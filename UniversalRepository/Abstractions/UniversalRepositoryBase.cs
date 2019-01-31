namespace UniversalRepository.Abstractions
{
    using System.Data;
    using System.Data.SqlClient;

    using UniversalRepository.Models;

    public abstract class UniversalRepositoryBase
    {
        #region Constructor

        protected UniversalRepositoryBase(ConnectionConfig connectionConfig)
        {
            ConnectionConfig = connectionConfig;
        }

        #endregion

        #region Properties

        protected static ConnectionConfig ConnectionConfig;

        protected IDbConnection Connection => new SqlConnection(ConnectionConfig.ConnectionString);

        #endregion
    }
}