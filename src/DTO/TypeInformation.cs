using System;

namespace The_xUnitGenerator
{
    public class TypeInformation
    {
        public string Typename { get; protected set; }
        public bool IsInterface { get; protected set; }

        public TypeInformation(string typename)
        {
            Typename = typename ?? throw new ArgumentException("Typename can't be null!");
            IsInterface = typename.StartsWith("I");
        }
    }
}
