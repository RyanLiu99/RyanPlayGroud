using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Ringba.Devtools.AerospikeCodeAnalyzer;

namespace Ringba.Devtools.AerospikeCodeAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AerospikeClientConstructorCodeFixProvider)), Shared]
    public class AerospikeClientConstructorCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AerospikeClientConstructorAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            if (!(root.FindNode(diagnosticSpan) is ObjectCreationExpressionSyntax objectCreation)) return;

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Use constructor with ClientPolicy",
                    createChangedDocument: c => FixAsync(context.Document, objectCreation, c),
                    equivalenceKey: "Use constructor with ClientPolicy"),
                diagnostic);
        }

        private async Task<Document> FixAsync(Document document, ObjectCreationExpressionSyntax objectCreation, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            // Create new ClientPolicy with writePolicyDefault = new WritePolicy() { durableDelete = true }
            var newClientPolicy = SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.IdentifierName("ClientPolicy"))
                .WithArgumentList(SyntaxFactory.ArgumentList()) // No constructor parameters
                .WithInitializer(SyntaxFactory.InitializerExpression(
                    SyntaxKind.ObjectInitializerExpression,
                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("writePolicyDefault"),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.IdentifierName("WritePolicy"))
                            .WithArgumentList(SyntaxFactory.ArgumentList()) // No constructor parameters
                            .WithInitializer(SyntaxFactory.InitializerExpression(
                                SyntaxKind.ObjectInitializerExpression,
                                SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                    SyntaxFactory.AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        SyntaxFactory.IdentifierName("durableDelete"),
                                        SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)
                                    )
                                )
                            ))
                        )
                    )
                ));

            var newArgument = SyntaxFactory.Argument(newClientPolicy);

            // Ensure that the new ClientPolicy is inserted **as the first argument**
            ArgumentListSyntax updatedArgumentList;
            if (objectCreation.ArgumentList != null && objectCreation.ArgumentList.Arguments.Count > 0)
            {
                // Insert at the beginning
                updatedArgumentList = objectCreation.ArgumentList.WithArguments(
                    objectCreation.ArgumentList.Arguments.Insert(0, newArgument));
            }
            else
            {
                // No arguments exist, create a new argument list
                updatedArgumentList = SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(newArgument));
            }

            // Replace the object creation's argument list
            var updatedObjectCreation = objectCreation.WithArgumentList(updatedArgumentList);
            editor.ReplaceNode(objectCreation, updatedObjectCreation);
            return editor.GetChangedDocument();
        }
    }

}