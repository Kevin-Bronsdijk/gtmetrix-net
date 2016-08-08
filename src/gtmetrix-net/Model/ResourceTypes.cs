using System;

namespace GTmetrix.Model
{
    [Flags]
    public enum ResourceTypes
    {
        Har = 1,
        PageSpeed = 2,
        Yslow = 4,
        // Not all types have been implemented yet
    }
}