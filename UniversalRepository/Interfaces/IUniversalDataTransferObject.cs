namespace UniversalRepository.Interfaces
{
    public interface IUniversalDataTransferObject
    {
        int Id { get; set; }
    }

    public interface IUniversalDataTransferObject<TDomain> : IUniversalDataTransferObject 
        where TDomain : class, IUniversalDomainObject { }
}