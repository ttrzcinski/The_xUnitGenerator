using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace The_xUnitGenerator
{
    public static class Constructors
    {
        public static ConstructorDeclarationSyntax CreateTestMethodConstructor(ClassInformation classInformation)
        {
            List<StatementSyntax> statementSyntaxes = new();
            statementSyntaxes.Add(InitCUT(classInformation));
            return ConstructorDeclaration(
                    Identifier($"{classInformation.NamesDeclaration}UnitTest")
                )
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword))).WithBody(Helpers.CloseBracket(statementSyntaxes));
        }


        static ExpressionStatementSyntax InitCUT(ClassInformation classInformation)
        {
            return
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(
                        Identifier(
                            TriviaList(),
                            classInformation.NamesDeclaration,
                            TriviaList(
                                Space))),
                    ObjectCreationExpression(
                        IdentifierName(classInformation.NamesDeclaration))
                    .WithNewKeyword(
                        Token(
                            TriviaList(),
                            SyntaxKind.NewKeyword,
                            TriviaList(
                                Space)))
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(AddMockObjects(classInformation)))))
                .WithOperatorToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.EqualsToken,
                        TriviaList(
                            Space))));
        }

        private static SyntaxNodeOrToken[] AddMockObjects(ClassInformation classInformation)
        {
            List<SyntaxNodeOrToken> arguments = new();
            int counter = classInformation.ConstructorsDeclaration.ParametersDeclaration.Count;
            foreach (var p in classInformation.ConstructorsDeclaration.ParametersDeclaration)
            {
                arguments.Add(Argument(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName($"Mock{p.Name}"),
                                            IdentifierName("Object"))));

                if (counter > 1)
                {
                    arguments.Add(Token(SyntaxKind.CommaToken));
                    counter--;
                }
            }
            return arguments.ToArray();

        }
    }
}
