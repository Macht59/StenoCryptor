using StenoCryptor.Commons.Enums;
using StenoCryptor.Interfaces;

namespace StenoCryptor.Engyne.Embeders
{
    public class EmbederFactory: IEmbederFactory
    {
        public IEmbeder GetInstance(EmbedType embedType)
        {
            switch (embedType)
            {
                case EmbedType.Lsb:
                    return new LsbEmbeder();
                case EmbedType.None:
                    return new MockEmbeder();
                default:
                    return new MockEmbeder();
            }
        }
    }
}
