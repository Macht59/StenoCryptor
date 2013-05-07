using StenoCryptor.Commons.Enums;
using StenoCryptor.Interfaces;

namespace StenoCryptor.Engyne.Embeders
{
    public class EmbederFactory: IEmbederFactory
    {
        public IEmbeder GetInstance(EmbedType embedType)
        {
            return new MockEmbeder();
        }
    }
}
