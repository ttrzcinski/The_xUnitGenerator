using System.Collections.Generic;

namespace The_xUnitGenerator
{
    public class FileInformation
    {
        public List<string> UsingsDeclaration { get; protected set; }
        public List<ClassInformation> ClassesDeclaration { get; protected set; }

        public FileInformation()
        {
            UsingsDeclaration = new List<string>();
            ClassesDeclaration = new List<ClassInformation>();
        }
    }
}
