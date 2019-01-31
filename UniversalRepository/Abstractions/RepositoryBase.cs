namespace UniversalRepository.Abstractions
{
    using System.Data;
    using System.Data.SqlClient;

    using UniversalRepository.Models;

    public abstract class RepositoryBase
    {
        #region Constructor

        protected RepositoryBase(ConnectionConfig connectionConfig)
        {
            ConnectionConfig = connectionConfig;
        }

        #endregion

        #region Properties

        public static ConnectionConfig ConnectionConfig;

        protected IDbConnection Connection => new SqlConnection(ConnectionConfig.ConnectionString);

        #endregion
    }
}