using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace The_xUnitGenerator
{
    public class ConstructorInformation
    {
        public List<ParameterInformation> ParametersDeclaration { get; protected set; }

        public ConstructorInformation() => ParametersDeclaration = new List<ParameterInformation>();
    }
}
