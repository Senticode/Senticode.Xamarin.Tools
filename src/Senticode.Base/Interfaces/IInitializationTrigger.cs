﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Senticode.Base.Interfaces
{
    public interface IInitializationTrigger
    {
        bool IsInitialized { get; }

        bool IsInitializing{ get; }

        bool IsReleasing { get; }
    }
}
