using StenoCryptor.Commons.Enums;
using StenoCryptor.Enums.Commons;
using StenoCryptor.Interfaces;

namespace StenoCryptor.Engyne.Embeders
{
    public class EmbederFactory: IEmbederFactory
    {
        public IEmbeder GetInstance(EmbedType embedType, EmbedingOptions options)
        {
            switch (embedType)
            {
                case EmbedType.Lsb:
                    return new LsbEmbeder(options);
                case EmbedType.None:
                    return new MockEmbeder();
                default:
                    return new LsbEmbeder(options);
            }
        }
    }
}
