using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace The_xUnitGenerator
{
    public static class Helpers
    {
        public static SyntaxToken End()
        {
            return Token(
                             TriviaList(),
                             SyntaxKind.SemicolonToken,
                             TriviaList(
                                 CarriageReturnLineFeed));
        }

        public static IdentifierNameSyntax Var()
        {
            return IdentifierName(
                        Identifier(
                            TriviaList(
                                Whitespace("            ")),
                            SyntaxKind.VarKeyword,
                            "var",
                            "var",
                            TriviaList(
                                Space)));
        }

        public static BlockSyntax CloseBracket(List<StatementSyntax> statementSyntaxes)
        {
            return OpenBracket(statementSyntaxes)
                            .WithCloseBraceToken(
                                Token(
                                    TriviaList(
                                        Whitespace("        ")),
                                    SyntaxKind.CloseBraceToken,
                                    TriviaList(
                                        CarriageReturnLineFeed)));
        }

        private static BlockSyntax OpenBracket(List<StatementSyntax> statementSyntaxes)
        {
            return Block(statementSyntaxes).WithOpenBraceToken(
                                Token(
                                    TriviaList(
                                        Whitespace("        ")),
                                    SyntaxKind.OpenBraceToken,
                                    TriviaList(
                                        CarriageReturnLineFeed)));
        }
    }
}
