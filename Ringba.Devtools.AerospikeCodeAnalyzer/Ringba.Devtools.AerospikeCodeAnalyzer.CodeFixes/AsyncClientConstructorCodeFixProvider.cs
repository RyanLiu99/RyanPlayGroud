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

namespace Ringba.Devtools.AerospikeCodeAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AerospikeClientConstructorCodeFixProvider)), Shared]
    public class AsyncClientConstructorCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AsyncClientConstructorAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            if (!(root.FindNode(diagnosticSpan) is ObjectCreationExpressionSyntax objectCreation)) return;

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Use constructor with AsyncClientPolicy",
                    createChangedDocument: c => FixAsync(context.Document, objectCreation, c),
                    equivalenceKey: "Use constructor with AsyncClientPolicy"),
                diagnostic);
        }


        private async Task<Document> FixAsync(Document document, ObjectCreationExpressionSyntax objectCreation, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            // Create new ClientPolicy with writePolicyDefault = new WritePolicy() { durableDelete = true }
            var newClientPolicy = SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.IdentifierName("AsyncClientPolicy"))
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

            // If argument list exists, check if the first argument is null
            if (objectCreation.ArgumentList?.Arguments.Count > 0)
            {
                var firstArgument = objectCreation.ArgumentList.Arguments[0];

                // If it is already right contructor overload taking 3 parameters, but the first argument is `null`, replace it
                if (objectCreation.ArgumentList.Arguments.Count == 3 && firstArgument.Expression.IsKind(SyntaxKind.NullLiteralExpression))
                {
                    var updatedArguments = objectCreation.ArgumentList.WithArguments(
                        objectCreation.ArgumentList.Arguments.Replace(firstArgument, newArgument));

                    var updatedObjectCreation = objectCreation.WithArgumentList(updatedArguments);
                    editor.ReplaceNode(objectCreation, updatedObjectCreation);
                }
                else
                {
                    // Otherwise, insert at the beginning if `ClientPolicy` is missing
                    var updatedArguments = objectCreation.ArgumentList.WithArguments(
                        objectCreation.ArgumentList.Arguments.Insert(0, newArgument));

                    var updatedObjectCreation = objectCreation.WithArgumentList(updatedArguments);
                    editor.ReplaceNode(objectCreation, updatedObjectCreation);
                }
            }
            else
            {
                // If no arguments exist, create a new argument list
                var updatedObjectCreation = objectCreation.WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(newArgument)));
                editor.ReplaceNode(objectCreation, updatedObjectCreation);
            }

            return editor.GetChangedDocument();
        }

    }

}