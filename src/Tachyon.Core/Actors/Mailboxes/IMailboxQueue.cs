#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IMailboxQueue.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2017 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion
namespace Tachyon.Core.Actors.Mailboxes
{
    public interface IMailboxQueue<T>
    {
        bool HasMessages { get; }
        bool TryPush(T message);
        bool TryPop(out T message);
    }
}