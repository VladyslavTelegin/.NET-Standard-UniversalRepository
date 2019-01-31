namespace UniversalRepository.Interfaces
{
    public interface IUniversalDataTransferObject { }

    public interface IUniversalDataTransferObject<TDomain> : IUniversalDataTransferObject 
        where TDomain : class, IUniversalDomainObject { }
}