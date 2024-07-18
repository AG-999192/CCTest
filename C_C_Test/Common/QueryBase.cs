namespace C_C_Test.Common
{
    using MediatR;

    /// <summary>
    /// Abstract class for query for use with MediatR.
    /// </summary>
    /// <typeparam name="TResult">the returned type.</typeparam>
    public abstract class QueryBase<TResult> : IRequest<TResult>
        where TResult : class
    {
    }
}
