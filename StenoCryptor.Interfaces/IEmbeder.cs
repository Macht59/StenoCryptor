﻿using StenoCryptor.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Interfaces
{
    public interface IEmbeder
    {
        void Embed(Container container, Stream message);
    }
}
