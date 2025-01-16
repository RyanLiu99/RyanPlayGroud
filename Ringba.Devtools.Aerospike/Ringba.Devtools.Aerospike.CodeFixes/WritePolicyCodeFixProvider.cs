
namespace Ringba.Devtools.Aerospike
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


    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WritePolicyCodeFixProvider)), Shared]
    public class WritePolicyCodeFixProvider : CodeFixProvider
    {
        private const string TITLE = "Set durableDelete to true";

        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(WritePolicyAnalyzer.DiagnosticId);

        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md 
        public override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the object creation expression identified by the diagnostic.
            var objectCreation = root.FindNode(diagnosticSpan) as ObjectCreationExpressionSyntax;
            if (objectCreation == null) return;

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: TITLE,
                    createChangedDocument: c => FixDurableDeleteAsync(context.Document, objectCreation, c),
                    equivalenceKey: TITLE),
                diagnostic);
        }

        private async Task<Document> FixDurableDeleteAsync(Document document, ObjectCreationExpressionSyntax objectCreation, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            // Check if the initializer exists
            var initializer = objectCreation.Initializer;
            if (initializer != null)
            {
                // Look for an existing assignment to durableDelete
                var durableDeleteAssignment = initializer.Expressions
                    .OfType<AssignmentExpressionSyntax>()
                    .FirstOrDefault(assign => assign.Left.ToString() == "durableDelete");

                if (durableDeleteAssignment != null)
                {
                    // Replace the existing assignment with durableDelete = true
                    var updatedAssignment = durableDeleteAssignment.WithRight(
                        SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression));

                    editor.ReplaceNode(durableDeleteAssignment, updatedAssignment);
                }
                else
                {
                    // Add durableDelete = true if it doesn't exist
                    var newAssignment = SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName("durableDelete"),
                        SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression));

                    var updatedInitializer = initializer.WithExpressions(initializer.Expressions.Add(newAssignment));
                    editor.ReplaceNode(initializer, updatedInitializer);
                }
            }
            else
            {
                // Add an initializer with durableDelete = true if none exists
                var newInitializer = SyntaxFactory.InitializerExpression(
                    SyntaxKind.ObjectInitializerExpression,
                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("durableDelete"),
                            SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))));

                var updatedObjectCreation = objectCreation.WithInitializer(newInitializer);
                editor.ReplaceNode(objectCreation, updatedObjectCreation);
            }

            return editor.GetChangedDocument();
        }
    }
}
