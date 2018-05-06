#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IObservableQuery.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Reactive.Streams;

namespace Tachyon.Core.Streams
{
    /// <summary>
    /// An observable query is used as a building block for reactive-streams compliant <see cref="IPublisher{T}"/>.
    /// 
    /// <see cref="IObservableQuery{O}"/> is not ready to be executed right away, as it's only descriptor of an
    /// operation. In order to make it <see cref="IRunnable{E}"/>, you need to compile it using <see cref="IStreamInterpreter"/>.
    /// </summary>
    /// <typeparam name="O"></typeparam>
    public interface IObservableQuery<O>
    {
        Expression Expression { get; }
        IStreamInterpreter Interpreter { get; }
    }

    public interface ISubjectQuery<I, O>
    {
        Expression Expression { get; }
        IStreamInterpreter Interpreter { get; }
    }

    public interface IObservatorQuery<I>
    {
        Expression Expression { get; }
        IStreamInterpreter Interpreter { get; }
    }

    public interface IRunnable<E>
    {
        Task<E> RunAsync(CancellationToken token = default(CancellationToken));
    }

    public interface IStreamInterpreter
    {
        IRunnable<E> Compile<E>(Expression expression);
    }
}