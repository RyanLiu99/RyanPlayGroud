using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Ringba.Devtools.AerospikeCodeAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WritePolicyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RAE004";

        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
                  id: DiagnosticId,
                  title: "Ensure Aerospike.Client.WritePolicy has durableDelete set to true",
                  messageFormat: "The 'durableDelete' property must be explicitly set to 'true' for Aerospike.Client.WritePolicy instances.",
                  category: "Ringba",
                  defaultSeverity: DiagnosticSeverity.Error,
                  isEnabledByDefault: true
              );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
        }

        private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
        {
            var objectCreation = (ObjectCreationExpressionSyntax)context.Node;

            // Ensure the type is Aerospike.Client.WritePolicy
            if (!(context.SemanticModel.GetSymbolInfo(objectCreation.Type).Symbol is INamedTypeSymbol typeSymbol) ||
                typeSymbol.Name != "WritePolicy" ||
                typeSymbol.ContainingNamespace.ToDisplayString() != "Aerospike.Client")
            {
                return;
            }

            // Check for property initializers in the object creation
            var initializer = objectCreation.Initializer;
            if (initializer != null)
            {
                // Look for an existing assignment to durableDelete
                var durableDeleteAssignment = initializer.Expressions
                    .OfType<AssignmentExpressionSyntax>()
                    .FirstOrDefault(assign => assign.Left.ToString() == "durableDelete");

                if (durableDeleteAssignment != null &&
                    durableDeleteAssignment.Right.ToString() == "true")
                {
                    // If durableDelete is explicitly set to true, no diagnostic needed
                    return;
                }
            }

            // If no initializer or durableDelete is not explicitly set to true, report a diagnostic
            context.ReportDiagnostic(Diagnostic.Create(Rule, objectCreation.GetLocation()));

        }
    }
}