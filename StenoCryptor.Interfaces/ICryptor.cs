namespace StenoCryptor.Interfaces
{
    public interface ICryptor
    {
        void Encrypt(IContainer container, IKey key);
    }
}
