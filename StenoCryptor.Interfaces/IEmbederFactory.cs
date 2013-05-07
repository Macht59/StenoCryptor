﻿using StenoCryptor.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Interfaces
{
    public interface IEmbederFactory
    {
        IEmbeder GetInstance(EmbedType embedType);
    }
}
