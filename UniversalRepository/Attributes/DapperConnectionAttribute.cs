namespace UniversalRepository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DapperConnectionAttribute : Attribute
    {
        public DapperConnectionAttribute(string connectionString = null)
        {
            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}