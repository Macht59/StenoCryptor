using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Enums.Commons
{
    [Flags]
    public enum EmbedingOptions: int
    {
        EmbedPreLastBit,
        UseForeSides,
        UseGreen,
        UseRed,
        UseBlue,
        UseAlpha,
        EmbedKey,
        UseDesktopImage
    }
}
