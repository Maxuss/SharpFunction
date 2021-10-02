namespace SFLang.Lexicon
{
    /// <summary>
    /// Delegate for all function invocations evaluated by Lizzie in its lambda delegate.
    /// </summary>
    public delegate object Function<TContext>(TContext ctx, ContextBinder<TContext> binder, Parameters arguments);

    /// <summary>
    /// Delegate for a lambda object created by Lizzie.
    /// </summary>
    public delegate object Lambda<TContext>(TContext ctx, ContextBinder<TContext> binder);
}