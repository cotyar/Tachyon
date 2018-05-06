#region copyright
// -----------------------------------------------------------------------
//  <copyright file="ObservableQueryExtensions.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Reactive.Streams;

namespace Tachyon.Core.Streams
{
    public static class ObservableQueryExtensions
    {
        public static IObservableQuery<O2> Select<O, O2>(this IObservableQuery<O> source, Expression<Func<O, O2>> selector)
        {
            throw new NotImplementedException();
        }

        public static IObservableQuery<O2> SelectAsync<O, O2>(this IObservableQuery<O> source, Expression<Func<O, Task<O2>>> selector)
        {
            throw new NotImplementedException();
        }

        public static IObservableQuery<O> Where<O>(this IObservableQuery<O> source, Expression<Func<O, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public static IObservableQuery<O> Deduplicate<O>(this IObservableQuery<O> source, Expression<Func<O, O, bool>> compare)
        {
            throw new NotImplementedException();
        }

        public static IAsyncEnumerator<O> ToAsyncEnumerator<O>(this IObservableQuery<O> source)
        {
            throw new NotImplementedException();
        }

        public static IObservable<O> ToObservable<O>(this IObservableQuery<O> source)
        {
            throw new NotImplementedException();
        }

        public static IPublisher<O> ToPublisher<O>(this IObservableQuery<O> source)
        {
            throw new NotImplementedException();
        }

        public static Task ForEachAsync<O>(this IObservableQuery<O> source, Action<O> block, CancellationToken token = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public static Task ForEachAsync<O>(this IObservableQuery<O> source, Func<O, Task> asyncBlock, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public static Task<S> Aggregate<S, O>(this IObservableQuery<O> source, S initialState, Func<S, O, S> aggregate, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public static Task<S> AggregateAsync<S, O>(this IObservableQuery<O> source, S initialState, Func<S, O, CancellationToken, Task<S>> aggregate, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}