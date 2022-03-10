using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace The_xUnitGenerator
{
    public class PatternGenerator
    {
        public static SyntaxNodeOrToken[] ActObjects(MethodInformation methodInformation)
        {
            List<SyntaxNodeOrToken> s = new();
            int counter = methodInformation.MethodParameters.Count;
            foreach (var p in methodInformation.MethodParameters)
            {
                s.Add(Argument(IdentifierName(p.Name)));
                if (counter > 1)
                {
                    s.Add(Token(SyntaxKind.CommaToken));
                    counter--;
                }
            }

            return s.ToArray();
        }

        public static LocalDeclarationStatementSyntax Variables(ParameterInformation parameterInformation)
        {
            return LocalDeclarationStatement(V(parameterInformation)).WithSemicolonToken(Helpers.End());
        }

        public static LocalDeclarationStatementSyntax x(ClassInformation classInformation, MethodInformation methodInformation)
        {
            return LocalDeclarationStatement(
    VariableDeclaration(
        Helpers.Var())
    .WithVariables(
        SingletonSeparatedList(
            VariableDeclarator(
                Identifier(
                    TriviaList(),
                    "actual",
                    TriviaList(
                        Space)))
            .WithInitializer(
                EqualsValueClause(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(classInformation.NamesDeclaration),
                            IdentifierName(methodInformation.MethodName)
                            )).WithArgumentList(
               ArgumentList(
                SeparatedList<ArgumentSyntax>(
                    ActObjects(methodInformation)))))

                .WithEqualsToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.EqualsToken,
                        TriviaList(
                            Space)))))));
        }

        public IEnumerable<PathInformation> GenerateCode(string source, FileInformation fileInformation)
        {
            if (source == null)
            {
                throw new ArgumentException("Source can't be null!");
            }

            List<PathInformation> resultList = new();
            List<UsingDirectiveSyntax> usings =
                fileInformation.UsingsDeclaration.ConvertAll((usingStr) =>
                UsingDirective(IdentifierName(usingStr)));
            usings.Add(UsingDirective(IdentifierName("Moq")));
            usings.Add(UsingDirective(IdentifierName("Xunit")));
            foreach (ClassInformation classInfo in fileInformation.ClassesDeclaration)
            {
                resultList.Add(new PathInformation(classInfo.NamesDeclaration + "Tests.cs", CompilationUnit()
                    .WithUsings(
                        List(
                            CreateUsings(classInfo, usings)))
                    .WithMembers(
                        SingletonList(
                            CreateTestClassWithNamespaceDeclaration(classInfo)))
                    .NormalizeWhitespace().ToFullString()));
            }
            return resultList;
        }

        protected static ClassDeclarationSyntax CreateClassDeclaration(ClassInformation classInformation)
        {
            ClassDeclarationSyntax classDeclaration = ClassDeclaration($"{classInformation.NamesDeclaration}UnitTest");
            classDeclaration = classDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));

            ParameterInformation p = new(classInformation.NamesDeclaration,
            new TypeInformation(classInformation.NamesDeclaration));
            classDeclaration = classDeclaration.AddMembers(
                    MethodField(p));
            foreach (var x in classInformation.ConstructorsDeclaration.ParametersDeclaration)
            {
                classDeclaration = classDeclaration.AddMembers(Field(x));
            }
            //
            classDeclaration = classDeclaration.AddMembers(Constructors.CreateTestMethodConstructor(classInformation));
            foreach (MethodInformation methodinfo in classInformation.MethodsDeclaration)
            {
                MethodDeclarationSyntax TestMethod = CreateTestMethodDeclaration(methodinfo, classInformation);
                classDeclaration = classDeclaration.AddMembers(TestMethod);
            }
            return classDeclaration;
        }

        private static FieldDeclarationSyntax Field(ParameterInformation parameterInformation)
        {

            return FieldDeclaration(
                                VariableDeclaration(
                                      GenericName(
                        Identifier(
                            TriviaList(
                                new[]{
                                    LineFeed,
                                    Whitespace("    ")}),
                            "Mock",
                            TriviaList()))
                    .WithTypeArgumentList(
                        TypeArgumentList(
                            SingletonSeparatedList<TypeSyntax>(
                                IdentifierName(parameterInformation.Type.Typename)))
                        .WithGreaterThanToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.GreaterThanToken,
                                TriviaList(
                                    Space)))))
                                .WithVariables(
                                    SingletonSeparatedList(F(parameterInformation))))
                                    .WithModifiers(
                TokenList(
                    Token(
                        TriviaList(),
                        SyntaxKind.ReadOnlyKeyword,
                        TriviaList(
                            Space)))).WithSemicolonToken(Helpers.End());
        }

        private static FieldDeclarationSyntax MethodField(ParameterInformation parameterInformation)
        {

            return FieldDeclaration(
                    VariableDeclaration(
                    IdentifierName(
                        Identifier(
                            TriviaList(
                                CarriageReturnLineFeed),
                            parameterInformation.Type.Typename,
                            TriviaList(
                                Space))))
                    .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                            Identifier($"{parameterInformation.Type.Typename}")))))
                            .WithModifiers(
                TokenList(
                    Token(
                        TriviaList(),
                        SyntaxKind.ReadOnlyKeyword,
                        TriviaList(
                            Space))))
                            .WithSemicolonToken(Helpers.End());
        }
        protected static MemberDeclarationSyntax CreateTestClassWithNamespaceDeclaration(ClassInformation classInformation)
        {
            return NamespaceDeclaration(
                IdentifierName(classInformation.NamespacesDeclaration + ".UnitTests"))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    CreateClassDeclaration(classInformation)));
        }


        protected static MethodDeclarationSyntax CreateTestMethodDeclaration(MethodInformation methodInformation, ClassInformation classInformation)
        {
            return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.VoidKeyword)),
                Identifier($"ShouldAbleTo{methodInformation.MethodName}"))
            .WithAttributeLists(
                SingletonList(
                    AttributeList(
                        SingletonSeparatedList(
                            Attribute(
                                IdentifierName("Fact"))))))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
            .WithBody(block(classInformation, methodInformation));
        }
        protected static UsingDirectiveSyntax[] CreateUsings(ClassInformation classInformation, List<UsingDirectiveSyntax> fileUsings)
        {
            return new List<UsingDirectiveSyntax>(fileUsings)
            {
                UsingDirective(IdentifierName(classInformation.NamespacesDeclaration))
            }.ToArray();
        }

        private static ExpressionStatementSyntax Assert()
        {
            return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("Assert"),
                        IdentifierName("NotNull")))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                IdentifierName("actual"))))));
        }

        private static VariableDeclarationSyntax V(ParameterInformation parameterInformation)
        {
            var y = EqualsValueClause(ObjectCreationExpression(
                                                IdentifierName(parameterInformation.Type.Typename))
                                            .WithNewKeyword(
                                                Token(
                                                    TriviaList(),
                                                    SyntaxKind.NewKeyword,
                                                    TriviaList(
                                                        Space))).WithArgumentList(ArgumentList()));
            if (parameterInformation.Type.Typename == "bool")
            {
                y = EqualsValueClause(LiteralExpression(SyntaxKind.FalseLiteralExpression));
            }
            if (parameterInformation.Type.Typename == "string")
            {
                y = EqualsValueClause(LiteralExpression(SyntaxKind.StringLiteralExpression,
                                    Literal("Example")));
            }
            if (parameterInformation.Type.Typename == "int")
            {
                y = EqualsValueClause(LiteralExpression(SyntaxKind.NumericLiteralExpression,
                                    Literal(0)));
            }
            var x = VariableDeclaration(Helpers.Var())
                         .WithVariables(
                             SingletonSeparatedList(
                                 VariableDeclarator(
                                     Identifier(parameterInformation.Name))
                                     .WithInitializer(y)));
            return x;
        }

        private static VariableDeclaratorSyntax F(ParameterInformation parameterInformation)
        {
            var x = VariableDeclarator(Identifier($"Mock{parameterInformation.Name}"));
            return x.WithInitializer(
                            EqualsValueClause(
                                ImplicitObjectCreationExpression()
                                .WithNewKeyword(
                                    Token(
                                        TriviaList(),
                                        SyntaxKind.NewKeyword,
                                        TriviaList(
                                            Space))))
                            .WithEqualsToken(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.EqualsToken,
                                    TriviaList(
                                        Space))));
        }
        static BlockSyntax block(ClassInformation classInformation, MethodInformation methodInformation)
        {
            //arrange
            List<StatementSyntax> statementSyntaxes = new();
            foreach (var x in methodInformation.MethodParameters)
            {
                statementSyntaxes.Add(Variables(x));
            }
            //act
            statementSyntaxes.Add(x(classInformation, methodInformation).WithSemicolonToken(Helpers.End()));
            //add Assert condition
            statementSyntaxes.Add(Assert());
            return Helpers.CloseBracket(statementSyntaxes);
        }

    }
}
