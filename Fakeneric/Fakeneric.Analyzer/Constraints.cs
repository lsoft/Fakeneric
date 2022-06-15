using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Fakeneric.Analyzer
{
    internal interface IActualConstraint
    {
        INamedTypeSymbol TypeParameter
        {
            get;
        }

        string GetUniqueId();

        bool IsSuccess(out string errorMessage);
    }

    internal class HasConstructorActualConstraint : IActualConstraint
    {
        public INamedTypeSymbol TypeParameter
        {
            get;
        }
        public ImmutableArray<ITypeSymbol> ConstructorParameters
        {
            get;
        }

        public HasConstructorActualConstraint(
            INamedTypeSymbol typeParameter,
            ImmutableArray<ITypeSymbol> constructorParameters
            )
        {
            if (typeParameter is null)
            {
                throw new ArgumentNullException(nameof(typeParameter));
            }

            TypeParameter = typeParameter;
            ConstructorParameters = constructorParameters;
        }

        public bool IsSuccess(out string errorMessage)
        {
            var success = CheckForConstructor();

            if (success)
            {
                errorMessage = string.Empty;
            }
            else
            {
                errorMessage = GetErrorMessage(
                    GetConstructorSignature()
                    );
            }

            return success;
        }

        private bool CheckForConstructor()
        {
            for (var c = 0; c < TypeParameter.Constructors.Length; c++)
            {
                var constructor = TypeParameter.Constructors[c];
                var parameters = constructor.Parameters;

                if (parameters.Length != ConstructorParameters.Length)
                {
                    continue;
                }
                if (constructor.DeclaredAccessibility == Accessibility.Private)
                {
                    continue;
                }

                for (var p = 0; p < parameters.Length; p++)
                {
                    var parameter = constructor.Parameters[p];
                    var cParameter = ConstructorParameters[p];

                    if (!SymbolEqualityComparer.IncludeNullability.Equals(parameter.Type, cParameter))
                    {
                        goto nextConstructor;
                    }
                }

                //success!
                return true;

            nextConstructor:
                int g = 0; //noop
            }

            return false;
        }

        public static string GetErrorMessage(string constructorSignature)
        {
            return $"Non-private constructor {constructorSignature} does not found";
        }

        public string GetConstructorSignature()
        {
            var sb = new List<string>();

            for (var p = 0; p < ConstructorParameters.Length; p++)
            {
                var cParameter = ConstructorParameters[p];

                sb.Add(cParameter.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            }

            var sbs = string.Join(", ", sb);

            return $"{TypeParameter.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}({sbs})";
        }

        public string GetUniqueId()
        {
            var result = $"{nameof(HasConstructorActualConstraint)}-{GetConstructorSignature()}";

            return result;
        }
    }

    internal class ImplementsActualConstraint : IActualConstraint
    {
        public INamedTypeSymbol TypeParameter
        {
            get;
        }
        public INamedTypeSymbol ImplementType
        {
            get;
        }

        public ImplementsActualConstraint(
            INamedTypeSymbol typeParameter,
            INamedTypeSymbol implementType
            )
        {
            if (typeParameter is null)
            {
                throw new ArgumentNullException(nameof(typeParameter));
            }

            if (implementType is null)
            {
                throw new ArgumentNullException(nameof(implementType));
            }

            TypeParameter = typeParameter;
            ImplementType = implementType;
        }

        public bool IsSuccess(out string errorMessage)
        {
            var success = TypeParameter.CanBeCastedTo(ImplementType);

            if (success)
            {
                errorMessage = string.Empty;
            }
            else
            {
                errorMessage = GetErrorMessage(
                    TypeParameter.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    ImplementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    );
            }

            return success;
        }

        public static string GetErrorMessage(string from, string to)
        {
            return $"{from} cannot be casted to {to}";
        }

        public string GetUniqueId()
        {
            var result = $"{nameof(ImplementsActualConstraint)}-{TypeParameter.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}-{ImplementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}";

            return result;
        }
    }

    internal class NotImplementsActualConstraint : IActualConstraint
    {
        public INamedTypeSymbol TypeParameter
        {
            get;
        }
        public INamedTypeSymbol ImplementType
        {
            get;
        }

        public NotImplementsActualConstraint(
            INamedTypeSymbol typeParameter,
            INamedTypeSymbol implementType
            )
        {
            if (typeParameter is null)
            {
                throw new ArgumentNullException(nameof(typeParameter));
            }

            if (implementType is null)
            {
                throw new ArgumentNullException(nameof(implementType));
            }

            TypeParameter = typeParameter;
            ImplementType = implementType;
        }

        public bool IsSuccess(out string errorMessage)
        {
            var canCast = TypeParameter.CanBeCastedTo(ImplementType);

            if (canCast)
            {
                errorMessage = GetErrorMessage(
                    TypeParameter.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    ImplementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    );
            }
            else
            {
                errorMessage = string.Empty;
            }

            return !canCast;
        }

        public static string GetErrorMessage(string from, string to)
        {
            return $"{from} can be casted to {to}, but it is forbidden.";
        }

        public string GetUniqueId()
        {
            var result = $"{nameof(NotImplementsActualConstraint)}-{TypeParameter.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}-{ImplementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}";

            return result;
        }
    }
}
