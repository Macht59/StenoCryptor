using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne.Embeders
{
    class FridrichEmbeder: IEmbeder
    {
        public void Embed(Commons.Container container, byte[] message)
        {
            throw new NotImplementedException();
        }

        public byte[] Extract(Commons.Container container, Commons.Key key)
        {
            throw new NotImplementedException();
        }
    }
}
