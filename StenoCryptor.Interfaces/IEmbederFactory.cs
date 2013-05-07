using StenoCryptor.Commons.Enums;

namespace StenoCryptor.Interfaces
{
    public interface IEmbederFactory
    {
        IEmbeder GetInstance(EmbedType embedType);
    }
}
