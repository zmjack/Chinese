using System;

namespace BuiltinGenerator
{
    public interface IGenerator
    {
        event Action<string> OnOutput;
        void Output(string output);
        string Generate();
    }

}