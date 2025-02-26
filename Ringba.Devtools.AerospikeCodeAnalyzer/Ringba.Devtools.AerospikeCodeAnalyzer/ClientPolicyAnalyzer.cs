﻿using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;
namespace Ringba.Devtools.AerospikeCodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClientPolicyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RAE003";
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
             id: DiagnosticId,
             title: "Ensure ClientPolicy has writePolicyDefault set",
             messageFormat: "ClientPolicy must be initialized with writePolicyDefault and cannot be null. Perfer use one of existing Ringba.Framework.Aerospike.DefaultPolicies",
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

            if (!(context.SemanticModel.GetSymbolInfo(objectCreation.Type).Symbol is INamedTypeSymbol typeSymbol) || (typeSymbol.ToString() != "Aerospike.Client.ClientPolicy" && typeSymbol.ToString() != "Aerospike.Client.AsyncClientPolicy"))
            {
                return;
            }

            var initializer = objectCreation.Initializer;

            if (initializer == null)
            {
                // If there is no initializer, report missing writePolicyDefault
                var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation());
                context.ReportDiagnostic(diagnostic);
                return;
            }

            // Look for writePolicyDefault assignment
            var writePolicyAssignment = initializer.Expressions
                .OfType<AssignmentExpressionSyntax>()
                .FirstOrDefault(assign => assign.Left.ToString() == "writePolicyDefault");

            if (writePolicyAssignment == null)
            {
                // If writePolicyDefault is missing, report a diagnostic
                var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
            else if (writePolicyAssignment.Right.IsKind(SyntaxKind.NullLiteralExpression))
            {
                // If writePolicyDefault is explicitly set to null, report a diagnostic
                var diagnostic = Diagnostic.Create(Rule, writePolicyAssignment.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}