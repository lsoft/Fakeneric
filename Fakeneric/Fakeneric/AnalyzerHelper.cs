using Fakeneric.Infrastructure;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Fakeneric
{
    internal static class AnalyzerHelper
    {
        public static List<IActualConstraint> ParseConstraints(ImmutableArray<INamedTypeSymbol> interfaces)
        {
            var constraints = new List<IActualConstraint>();

            var whereInterfaces = interfaces
                .Where(i =>
                    i.IsGenericType
                    && i.ConstructUnboundGenericType().ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::Fakeneric.Infrastructure.Where<,>" //TODO: ускорить можно забив на проверку жденерик аргументов и не строя ConstructUnboundGenericType
                    )
                .ToList();

            if (whereInterfaces.Count == 0)
            {
                return constraints;
            }

            foreach (var whereInterface in whereInterfaces)
            {
                if (!(whereInterface.TypeArguments[0] is INamedTypeSymbol constrainedType))
                {
                    continue;
                }

                if (!(whereInterface.TypeArguments[1] is INamedTypeSymbol predicate))
                {
                    continue;
                }
                var predicateKind = predicate.BaseType;
                if (predicateKind == null)
                {
                    continue;
                }
                var predicateKindFullName = predicateKind.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                switch (predicateKindFullName)
                {
                    case "global::Fakeneric.Infrastructure.ImplementsConstraint":
                        {
                            constraints.Add(
                                new ImplementsActualConstraint(
                                    constrainedType,
                                    (INamedTypeSymbol)predicate.TypeArguments[0]
                                    )
                                );
                            break;
                        }
                    case "global::Fakeneric.Infrastructure.HasConstructorConstraint":
                        {
                            constraints.Add(
                                new HasConstructorActualConstraint(
                                    constrainedType,
                                    predicate.TypeArguments
                                    )
                                );
                            break;
                        }
                    default:
                        throw new NotImplementedException(predicateKindFullName);
                }
            }

            return constraints;
        }

    }
}
