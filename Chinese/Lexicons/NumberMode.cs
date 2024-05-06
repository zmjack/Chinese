using System;

namespace Chinese.Lexicons;

[Flags]
public enum NumberMode
{
    Default = 0,

    Traditional = 0b0010,
    Classical = 0b0100,
    Concise = 0b1000,

    Upper = 0b1_00000000,
    Code = 0b10_00000000,
}
