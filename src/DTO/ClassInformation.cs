using System;
using System.Collections.Generic;

namespace The_xUnitGenerator
{
    public class ClassInformation
    {
        public List<MethodInformation> MethodsDeclaration { get; protected set; }
        public string NamespacesDeclaration { get; protected set; }
        public string NamesDeclaration { get; protected set; }
        public ConstructorInformation ConstructorsDeclaration { get; protected set; }

        public ClassInformation(string name, string namespaceDeclaration, ConstructorInformation constructorInformation)
        {
            if ((name == null) || (namespaceDeclaration == null) || (constructorInformation == null))
                throw new ArgumentException("Arguments can't be null!");
            NamesDeclaration = name;
            NamespacesDeclaration = namespaceDeclaration;
            MethodsDeclaration = new List<MethodInformation>();
            ConstructorsDeclaration = constructorInformation;
        }
    }
}
