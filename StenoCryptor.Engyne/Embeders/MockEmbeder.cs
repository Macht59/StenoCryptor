using StenoCryptor.Commons;
using StenoCryptor.Interfaces;

namespace StenoCryptor.Engyne.Embeders
{
    public class MockEmbeder: IEmbeder
    {
        public void Embed(Commons.Container container, byte[] message)
        {
        }


        public byte[] Extract(Commons.Container container, Key key)
        {
            return new byte[0];
        }
    }
}
