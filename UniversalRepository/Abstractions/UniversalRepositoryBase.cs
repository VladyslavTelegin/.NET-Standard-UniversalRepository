namespace UniversalRepository.Abstractions
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using UniversalRepository.Attributes;
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

        protected ConnectionConfig ConnectionConfig;

        protected IDbConnection Connection => new SqlConnection(ConnectionConfig.ConnectionString);

        #endregion

        #region Methods

        protected IDbConnection ResolveConnection<T>()
        {
            var connectionAttributeObject = typeof(T).GetCustomAttributes(typeof(DapperConnectionAttribute), true).SingleOrDefault();
            if (connectionAttributeObject != null)
            {
                var connectionAttribute = connectionAttributeObject as DapperConnectionAttribute;
                if (!(connectionAttribute == null || string.IsNullOrEmpty(connectionAttribute.ConnectionString)))
                {
                    this.ConnectionConfig = new ConnectionConfig(connectionAttribute.ConnectionString);
                }
            }

            return this.Connection;
        }

        #endregion
    }
}