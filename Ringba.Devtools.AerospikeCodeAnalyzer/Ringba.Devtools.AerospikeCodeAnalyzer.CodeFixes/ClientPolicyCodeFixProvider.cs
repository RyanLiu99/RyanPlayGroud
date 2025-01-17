
namespace Ringba.Devtools.AerospikeCodeAnalyzer
{
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

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ClientPolicyCodeFixProvider)), Shared]
    public class ClientPolicyCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ClientPolicyAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            if (!(root.FindNode(diagnosticSpan) is ObjectCreationExpressionSyntax objectCreation)) return;

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Set writePolicyDefault with durableDelete = true",
                    createChangedDocument: c => FixAsync(context.Document, objectCreation, c),
                    equivalenceKey: "Set writePolicyDefault"),
                diagnostic);
        }

        private async Task<Document> FixAsync(Document document, ObjectCreationExpressionSyntax objectCreation, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            // Create new WritePolicy with durableDelete = true
            var newWritePolicy = SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.IdentifierName("WritePolicy"))
                .WithArgumentList(SyntaxFactory.ArgumentList())
                .WithInitializer(SyntaxFactory.InitializerExpression(
                    SyntaxKind.ObjectInitializerExpression,
                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("durableDelete"),
                            SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)
                        )
                    )
                ));

            var newAssignment = SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName("writePolicyDefault"),
                newWritePolicy
            );

            var initializer = objectCreation.Initializer;
            if (initializer != null)
            {
                // Look for an existing assignment to writePolicyDefault
                var existingAssignment = initializer.Expressions
                    .OfType<AssignmentExpressionSyntax>()
                    .FirstOrDefault(assign => assign.Left.ToString() == "writePolicyDefault");

                if (existingAssignment != null)
                {
                    // If writePolicyDefault is assigned to null, replace it
                    if (existingAssignment.Right.IsKind(SyntaxKind.NullLiteralExpression))
                    {
                        editor.ReplaceNode(existingAssignment, newAssignment);
                    }
                }
                else
                {
                    // Add writePolicyDefault if it's missing
                    var updatedInitializer = initializer.AddExpressions(newAssignment);
                    editor.ReplaceNode(initializer, updatedInitializer);
                }
            }
            else
            {
                // If no initializer exists, create one with writePolicyDefault
                var newInitializer = SyntaxFactory.InitializerExpression(
                    SyntaxKind.ObjectInitializerExpression,
                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(newAssignment));

                var updatedObjectCreation = objectCreation.WithInitializer(newInitializer);
                editor.ReplaceNode(objectCreation, updatedObjectCreation);
            }

            return editor.GetChangedDocument();
        }
    }

}