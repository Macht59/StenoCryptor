using StenoCryptor.Commons;
using StenoCryptor.Commons.Enums;

namespace StenoCryptor.Interfaces
{
    public interface IAlgorithmFactory
    {
        ICryptor GetInstance(CryptType key);
    }
}
