namespace Fakeneric.Infrastructure
{
    public abstract class WhereConstraint
    {
    }

    #region Implements

    public abstract class ImplementsConstraint : WhereConstraint
    {
    }

    public sealed class Implements<T> : ImplementsConstraint
    {
    }

    #endregion

    #region HasConstructor

    public abstract class HasConstructorConstraint : WhereConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private parameterless constructor.
    /// </summary>
    public sealed class HasConstructorWithNoParameter : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with type
    /// <typeparamref name="T"/>.
    /// </summary>
    public sealed class HasConstructorWithParameter<T> : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with types
    /// (<typeparamref name="T1"/>, <typeparamref name="T2"/>).
    /// </summary>
    public sealed class HasConstructorWithParameter<T1, T2> : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with types
    /// (<typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>).
    /// </summary>
    public sealed class HasConstructorWithParameter<T1, T2, T3> : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with types
    /// (<typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>).
    /// </summary>
    public sealed class HasConstructorWithParameter<T1, T2, T3, T4> : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with types
    /// (<typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>).
    /// </summary>
    public sealed class HasConstructorWithParameter<T1, T2, T3, T4, T5> : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with types
    /// (<typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>).
    /// </summary>
    public sealed class HasConstructorWithParameter<T1, T2, T3, T4, T5, T6> : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with types
    /// (<typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, <typeparamref name="T7"/>).
    /// </summary>
    public sealed class HasConstructorWithParameter<T1, T2, T3, T4, T5, T6, T7> : HasConstructorConstraint
    {
    }

    /// <summary>
    /// Target type should have a non-private constructor with types
    /// (<typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, <typeparamref name="T7"/>, <typeparamref name="T8"/>).
    /// </summary>
    public sealed class HasConstructorWithParameter<T1, T2, T3, T4, T5, T6, T7, T8> : HasConstructorConstraint
    {
    }

    #endregion

#pragma warning disable IDE1006 // Naming Styles
    public interface Where<TTarget, TConstraint>
#pragma warning restore IDE1006 // Naming Styles
        where TConstraint : WhereConstraint
    {
        //keep this place empty, we do not want to force user's derived classes to have an additional method/property/sruff because of Fakenerics
    }

}
