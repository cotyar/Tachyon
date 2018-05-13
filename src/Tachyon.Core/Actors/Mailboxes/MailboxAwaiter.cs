#region copyright
// -----------------------------------------------------------------------
//  <copyright file="MailboxAwaiter.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Core.Actors.Mailboxes
{
    public struct MailboxAwaiter
    {
        public bool IsCompleted { get; }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public void GetResult()
        {
            throw new NotImplementedException();
        }

        public MailboxAwaiter GetAwaiter() => this;
    }
}