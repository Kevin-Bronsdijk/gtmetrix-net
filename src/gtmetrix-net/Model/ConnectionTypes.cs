using System;

namespace GTmetrix.Model
{
    [Flags]
    public enum ConnectionTypes
    {
        Unthrottled = 1,
        Cable = 2,
        DSL = 4,
        Mobile3G = 8,
        Mobile2G = 16,
        DailUp56K = 32
    }
}