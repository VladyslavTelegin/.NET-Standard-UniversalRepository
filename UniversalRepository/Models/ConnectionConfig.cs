namespace UniversalRepository.Models
{
    public class ConnectionConfig
    {
        #region PrivateFields

        private readonly string _connectionString;

        #endregion

        #region Constructor

        public ConnectionConfig(string connectionString = "")
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Properties

        public string DataSource { get; set; }

        public string InitialCatalog { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public bool IntegratedSecurity { get; set; }

        public bool PersistSecurityInfo { get; set; }

        public string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_connectionString))
                {
                    return _connectionString;
                }

                return $"Data Source={this.DataSource};Initial Catalog={this.InitialCatalog};" +
                       $"Integrated Security={this.IntegratedSecurity.ToString()};User{this.User};Password={this.Password};" +
                       $"Persist Security Info={this.PersistSecurityInfo.ToString()};";
            }
        }

        #endregion
    }
}