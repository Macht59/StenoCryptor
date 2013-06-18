using StenoCryptor.Commons.Enums;
using StenoCryptor.Enums.Commons;

namespace StenoCryptor.Interfaces
{
    public interface IEmbederFactory
    {
        IEmbeder GetInstance(EmbedType embedType, EmbedingOptions options);
    }
}
