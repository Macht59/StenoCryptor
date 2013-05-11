using StenoCryptor.Commons;

namespace StenoCryptor.Interfaces
{
    public interface IKeyAware
    {
        bool ValidateKey(string key);

        byte[] ParseKey(string key);
    }
}
