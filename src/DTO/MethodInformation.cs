using System;
using System.Collections.Generic;

namespace The_xUnitGenerator
{
    public class MethodInformation
    {
        public string MethodName { get; protected set; }
        public List<ParameterInformation> MethodParameters { get; protected set; }
        public TypeInformation MethodReturnType { get; protected set; }

        public MethodInformation(string methodName, TypeInformation methodReturnType)
        {
            if ((methodName == null) || (methodReturnType == null))
                throw new ArgumentException("Arguments can't be null!");
            MethodName = methodName;
            MethodReturnType = methodReturnType;
            MethodParameters = new List<ParameterInformation>();
        }
    }
}
