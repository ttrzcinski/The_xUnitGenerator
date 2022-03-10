using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace The_xUnitGenerator
{

    public class CSCodeAnalyzer : ICSCodeAnalyzer
    {
        CSharpCompilation Compilation;
        //SemanticModel sem;
        public FileInformation AnalyzeCode(string code)
        {
            FileInformation fileInformation = new();
            CompilationUnitSyntax compilationUnit = CSharpSyntaxTree.ParseText(code).GetCompilationUnitRoot();
            //PrepareSemanticModel(code);

            foreach (UsingDirectiveSyntax usingDeclaration in compilationUnit.Usings)
            {
                fileInformation.UsingsDeclaration.Add(usingDeclaration.Name.ToString());
            }

            foreach (ClassDeclarationSyntax classDeclaration in
                compilationUnit.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                fileInformation.ClassesDeclaration.Add(CreateClassInformation(classDeclaration));
            }

            return fileInformation;
        }
        internal static ClassInformation CreateClassInformation(ClassDeclarationSyntax classDeclaration)
        {
            string ClassName = classDeclaration.Identifier.ValueText;
            var temp = GetAvailableNameSpace(classDeclaration);
            string NameSpace = temp.Name.ToString();
            var constructor = GetExtendedConstructor(classDeclaration);
            ClassInformation classInformation =
                new(ClassName,
                NameSpace,
                constructor);
            foreach (MethodDeclarationSyntax methodDeclaration in classDeclaration.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where((methodDeclaration) => methodDeclaration.Modifiers.Any((modifier) =>
                modifier.IsKind(SyntaxKind.PublicKeyword))))
            {
                classInformation.MethodsDeclaration.Add(CreateMethodInformation(methodDeclaration));
            }
            return classInformation;
        }

        internal static MethodInformation CreateMethodInformation(MethodDeclarationSyntax methodDeclaration)
        {
            MethodInformation result =
                new(methodDeclaration.Identifier.ValueText,
                new TypeInformation(methodDeclaration.ReturnType.ToString()));
            foreach (ParameterSyntax parameter in methodDeclaration.ParameterList.Parameters)
            {
                result.MethodParameters.Add(new ParameterInformation(parameter.Identifier.ValueText,
                    new TypeInformation(parameter.Type.ToString())));
            }
            foreach (var invocation in methodDeclaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                //var symbol = (IMethodSymbol)SemanticModel.GetSymbolInfo(invocation).Symbol;
                //var invokedSymbol = sem.GetSymbolInfo(invocation).Symbol;
                //var text = invocation.GetText();
                //var x=SemanticModel.GetSymbolInfo(invocation.SyntaxTree);
            }

            return result;
        }

        internal static ConstructorInformation GetExtendedConstructor(ClassDeclarationSyntax classDeclaration)
        {
            ConstructorInformation result = new();
            ConstructorDeclarationSyntax extendConstructorDeclaration = classDeclaration.DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .Where((constructor) => constructor.Modifiers.Any((modifier) => modifier.IsKind(SyntaxKind.PublicKeyword)))
                .OrderByDescending((constructor) => constructor.ParameterList.Parameters.Count)
                .FirstOrDefault();
            if (extendConstructorDeclaration != null)
            {
                foreach (ParameterSyntax parameter in extendConstructorDeclaration.ParameterList.Parameters)
                {
                    result.ParametersDeclaration.Add(new ParameterInformation(parameter.Identifier.ValueText,
                        new TypeInformation(parameter.Type.ToString())));
                }
            }

            return result;
        }

        // private void PrepareSemanticModel(string code)
        // {
        //     SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        //     CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
        //     Compilation = CSharpCompilation.Create("HelloWorld");
        //     Compilation = Compilation.AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location));
        //     var releaseDlls = Directory.GetFiles("/Users/palasac/Projects/beat-fms-edge-permission-mgmt/src/BEAT.Authentication.API/bin/Debug/net6.0", "*.dll");
        //     List<MetadataReference> references = new List<MetadataReference>();
        //     foreach (string dllpath in releaseDlls)
        //     {
        //         references.Add(MetadataReference.CreateFromFile(dllpath));
        //     }
        //     Compilation = Compilation.AddReferences(references);
        //     Compilation = Compilation.AddSyntaxTrees(tree);
        //     sem = Compilation.GetSemanticModel(tree);
        //     Compilation = Compilation.AddSyntaxTrees(tree);
        //     sem = Compilation.GetSemanticModel(tree);
        // }

        private static NamespaceDeclarationSyntax GetAvailableNameSpace(ClassDeclarationSyntax classDeclaration)
        {
            NamespaceDeclarationSyntax temp = classDeclaration.Parent as NamespaceDeclarationSyntax;
            if (temp == null) { temp = classDeclaration.Parent.Parent as NamespaceDeclarationSyntax; }
            return temp;
        }
    }
}
