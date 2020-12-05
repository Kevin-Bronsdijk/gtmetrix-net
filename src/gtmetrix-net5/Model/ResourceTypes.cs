using System;

namespace GTmetrix5.Model
{
    [Flags]
    public enum ResourceTypes
    {
        Har = 1,
        PageSpeed = 2,
        Yslow = 4,
        PagespeedFiles = 8,
        Screenshot = 16,
        PdfReport = 32,
        PdfReportExtended = 64,
        Video = 128
    }
}
