using Fakeneric.Infrastructure;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Fakeneric
{
    internal static class DiagnosticDescriptors
    {
        private const string Category = "Generic";
        public const string DiagnosticId = "Fakeneric";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization

        private static readonly LocalizableString ConstraintViolationTitle = new LocalizableResourceString(nameof(Resources.ConstraintViolationTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ConstraintViolationMessageFormat = new LocalizableResourceString(nameof(Resources.ConstraintViolationMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ConstraintViolationDescription = new LocalizableResourceString(nameof(Resources.ConstraintViolationDescription), Resources.ResourceManager, typeof(Resources));
        public static readonly DiagnosticDescriptor ConstraintViolationDescriptor = new DiagnosticDescriptor(DiagnosticId, ConstraintViolationTitle, ConstraintViolationMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: ConstraintViolationDescription);

        private static readonly LocalizableString ConstraintDeclarationTitle = new LocalizableResourceString(nameof(Resources.ConstraintDeclarationTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ConstraintDeclarationMessageFormat = new LocalizableResourceString(nameof(Resources.ConstraintDeclarationMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ConstraintDeclarationDescription = new LocalizableResourceString(nameof(Resources.ConstraintDeclarationDescription), Resources.ResourceManager, typeof(Resources));
        public static readonly DiagnosticDescriptor ConstraintDeclarationDescriptor = new DiagnosticDescriptor(DiagnosticId, ConstraintDeclarationTitle, ConstraintDeclarationMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: ConstraintDeclarationDescription);

        private static readonly LocalizableString InternalErrorTitle = new LocalizableResourceString(nameof(Resources.InternalErrorTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString InternalErrorMessageFormat = new LocalizableResourceString(nameof(Resources.InternalErrorMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString InternalErrorDescription = new LocalizableResourceString(nameof(Resources.InternalErrorDescription), Resources.ResourceManager, typeof(Resources));
        public static readonly DiagnosticDescriptor InternalErrorDescriptor = new DiagnosticDescriptor(DiagnosticId, InternalErrorTitle, InternalErrorMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: InternalErrorDescription);
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FakenericAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.ConstraintViolationDescriptor,
                    DiagnosticDescriptors.ConstraintDeclarationDescriptor,
                    DiagnosticDescriptors.InternalErrorDescriptor
                    );
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var type = (INamedTypeSymbol)context.Symbol;

            try
            {
                //if (!namedTypeSymbol.IsGenericType)
                //{
                //    return;
                //}

                //CheckForConstraintDeclaration(context, type);

                CheckForConstraintViolation(context, type);

                //for (var i = 0; i < baseType.TypeArguments.Length; i++)
                //{
                //    var isSubstituted = baseType.TypeArguments[i].TypeKind != TypeKind.TypeParameter;
                //    if (isSubstituted)
                //    {
                //        //дженерик параметр замещен конкретным типом
                //        //надо проанализировать, позволяют ли это констрейнты

                //        ITypeParameterSymbol aaaa = baseType.TypeParameters[1];

                //        int f = 0;
                //    }
                //}

                //int g = 0;

                ////// Find just those named type symbols with names containing lowercase letters.
                ////if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
                ////{
                ////    // For all such symbols, produce a diagnostic.
                ////    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                ////    context.ReportDiagnostic(diagnostic);
                ////}
            }
            catch (Exception excp)
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.InternalErrorDescriptor, type.Locations[0], excp.Message);
                context.ReportDiagnostic(diagnostic);
            }
        }
        private static void CheckForConstraintViolation(SymbolAnalysisContext context, INamedTypeSymbol type)
        {
            if (type.AllInterfaces.Length == 0)
            {
                return;
            }

            var baseType = type.BaseType;

            if (baseType == null)
            {
                return;
            }
            if (!baseType.IsGenericType)
            {
                return;
            }
            if (SymbolEqualityComparer.IncludeNullability.Equals(baseType.ConstructUnboundGenericType(), baseType))
            {
                return;
            }

            //var s = namedTypeSymbol.Name;

            var constraints = AnalyzerHelper.ParseConstraints(baseType.AllInterfaces);

            foreach (var constraint in constraints)
            {
                var success = constraint.IsSuccess(out var errorMessage);
                if (!success)
                {
                    // For all such symbols, produce a diagnostic.
                    var diagnostic = Diagnostic.Create(DiagnosticDescriptors.ConstraintViolationDescriptor, type.Locations[0], errorMessage);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static void CheckForConstraintDeclaration(SymbolAnalysisContext context, INamedTypeSymbol type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsGenericType)
            {
                return;
            }
            if (type.AllInterfaces.Length == 0)
            {
                return;
            }

            var constraints = AnalyzerHelper.ParseConstraints(type.Interfaces);

            var hashSet = new HashSet<string>();
            for (var c = 0; c < constraints.Count; c++)
            {
                var constraint = constraints[c];
                var uid = constraint.GetUniqueId();
                if (hashSet.Contains(uid))
                {
                    // For all such situations, produce a diagnostic.
                    var diagnostic = Diagnostic.Create(DiagnosticDescriptors.ConstraintDeclarationDescriptor, type.Locations[0], type.Name);
                    context.ReportDiagnostic(diagnostic);
                }
                else
                {
                    hashSet.Add(uid);
                }
            }
        }
    }
}
