using StenoCryptor.Commons.Enums;
using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
