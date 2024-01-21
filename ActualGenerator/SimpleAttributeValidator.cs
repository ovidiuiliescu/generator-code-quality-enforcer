using Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;

namespace ActualGenerator
{
    [Generator]
    public class SimpleAttributeValidator : IIncrementalGenerator
    {
        // You need the full name of the attribute we're scanning for, in order to match the nodes to which it was applied
        private static readonly string SimpleAttributeFullName = typeof(SimpleAttribute).FullName;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {

#if DEBUG
            // Needed in order to more easily debug the generator and see when it is called. Comment out or remove if not needed.
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif

            // Determine what syntax nodes to analyze
            var methods = context.SyntaxProvider.ForAttributeWithMetadataName<MethodDeclarationSyntax>(
                    fullyQualifiedMetadataName: SimpleAttributeFullName, // Only consider syntax nodes that have the SimpleAttribute applied
                    predicate: IsMethodDeclaration, // Out of those, only method declarations should be processed
                    transform: LeaveMethodDeclarationAsIs // Return matching declaration nodes as-is, no mapping to custom objects, no extra processing.
                ).Collect();

            // Register the analyzer to run on those nodes
            context.RegisterSourceOutput(methods, EnforceCodeQuality);
        }

        private void EnforceCodeQuality(SourceProductionContext context, ImmutableArray<MethodDeclarationSyntax> applicableMethodDeclarations)
        {
            // At this point, we will receive a list of qualifying syntax nodes, as defined in the Initialize() method
            foreach (var methodDeclaration in applicableMethodDeclarations)
            {
                // For all declarations that pass the validation, do nothing
                if (PassesValidation(methodDeclaration))
                {
                    continue;
                }

                // For all others, create a diagnostic entry and report it
                var diagnostic = CreateDiagnostic(methodDeclaration);
                context.ReportDiagnostic(diagnostic);
            }

        }

        private static bool PassesValidation(MethodDeclarationSyntax methodDeclaration)
        {
            // Any validation logic or code quality enforcement should be entered here
            // For now, this just checks that the given method has at most ONE for loop
            var totalForLoopCount = GetRecursiveCountNodesOfType<ForStatementSyntax>(methodDeclaration);
            return totalForLoopCount < 2;
        }

        #region Helpers

        private static bool IsMethodDeclaration(SyntaxNode node, CancellationToken ct)
            => node is MethodDeclarationSyntax;  // You can perform more advanced filtering here if needed

        private static MethodDeclarationSyntax LeaveMethodDeclarationAsIs(GeneratorAttributeSyntaxContext generatorContext, CancellationToken ct = default)
            => (MethodDeclarationSyntax) generatorContext.TargetNode; // Depending on the caller, you can/may map the source node to your own custom object 

        private static Diagnostic CreateDiagnostic(MethodDeclarationSyntax declaration)
        {
            var description = new DiagnosticDescriptor(
                        id: "CQ_SIMPLE", // 
                        title: "Complexity exceeded",
                        messageFormat: "Method {0} is more complex than allowed",
                        category: "complexity",
                        defaultSeverity: DiagnosticSeverity.Error,
                        isEnabledByDefault: true);

            return Diagnostic.Create(description, declaration.GetLocation(), declaration.Identifier.ToString());
        }

        private static int GetRecursiveCountNodesOfType<T>(SyntaxNode root)
        {
            int count = 0;
            foreach (var node in root.ChildNodes())
            {
                if (node is T)
                {
                    count++;
                }
                count += GetRecursiveCountNodesOfType<T>(node);
            }
            return count;
        }

        #endregion
    }
}
