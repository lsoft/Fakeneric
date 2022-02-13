using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakeneric.Analyzer
{
    public static class RoslynHelper
    {
        public static bool CanBeCastedTo(
            this ITypeSymbol source,
            ITypeSymbol target
            )
        {
            if (SymbolEqualityComparer.Default.Equals(source, target))
            {
                return true;
            }
            if (source is INamedTypeSymbol ntSource)
            {
                if (SymbolEqualityComparer.Default.Equals(ntSource.OriginalDefinition, target))
                {
                    return true;
                }
            }

            foreach (INamedTypeSymbol @interface in source.AllInterfaces)
            {
                if (SymbolEqualityComparer.Default.Equals(@interface, target))
                {
                    return true;
                }
            }

            if (source.BaseType != null && !SymbolEqualityComparer.Default.Equals(source.BaseType, source))
            {
                if (CanBeCastedTo(source.BaseType, target))
                {
                    return true;
                }
            }

            foreach (INamedTypeSymbol @interface in source.AllInterfaces)
            {
                var r = CanBeCastedTo(
                    @interface,
                    target
                    );

                if (r)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
