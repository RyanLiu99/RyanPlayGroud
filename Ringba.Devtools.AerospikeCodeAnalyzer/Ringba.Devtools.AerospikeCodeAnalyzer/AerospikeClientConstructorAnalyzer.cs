using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;

namespace Ringba.Devtools.AerospikeCodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AerospikeClientConstructorAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RAE001";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticId,
            title: "Ensure AerospikeClient constructor uses ClientPolicy",
            messageFormat: "AerospikeClient must be initialized with a ClientPolicy. Perfer Ringba.Framework.Aerospike. DefaultPolicies.DurableDeleteClientPolicy.",
            category: "AerospikeUsage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
        }

        private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
        {
            var objectCreation = (ObjectCreationExpressionSyntax)context.Node;

            if (!(context.SemanticModel.GetSymbolInfo(objectCreation.Type).Symbol is INamedTypeSymbol typeSymbol) || typeSymbol.ToString() != "Aerospike.Client.AerospikeClient")
            {
                return;
            }

            // Check if the first parameter is of type ClientPolicy
            if (objectCreation.ArgumentList == null || objectCreation.ArgumentList.Arguments.Count == 0)
            {
                var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
            else
            {
                var firstArg = objectCreation.ArgumentList.Arguments[0];
                var argType = context.SemanticModel.GetTypeInfo(firstArg.Expression).Type;

                if (argType == null || 
                    (argType.ToString() != "Aerospike.Client.ClientPolicy" && argType.ToString() != "Aerospike.Client.AsyncClientPolicy"))
                {
                    var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}