namespace SFLang.Lexicon
{
    /// <summary>
    ///     Delegate for all function invocations evaluated by SFLang in its lambda delegate.
    /// </summary>
    [Serializable]
    public delegate object Function<TContext>(TContext ctx, ContextBinder<TContext> binder, Parameters args);

    /// <summary>
    ///     Delegate for a lambda object created by SFLang.
    /// </summary>
    [Serializable]
    public delegate object Lambda<TContext>(TContext ctx, ContextBinder<TContext> binder);
}