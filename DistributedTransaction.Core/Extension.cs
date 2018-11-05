﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTransaction.Core
{
    internal class Type<T>
        where T : IData
    {
        public static IDataSource<T> DataSource =
            (IDataSource<T>)Activator.CreateInstance(
                typeof(T)
                    .GetInterfaces()
                    .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IData<,>))
                    .GetGenericArguments()[0]);

        public static string FullName = typeof(T).FullName;

        public static string Key(string id) => $"{FullName}:{id}";

        public static string LockKey(string id) => $"Lock:{FullName}:{id}";
    }

    internal static class TaskExtension
    {
        public static async Task<TResult> Then<TResult>(this Task self, Func<Task<TResult>> thenProc, bool continueOnCapturedContext = false)
        {
            await self.ConfigureAwait(continueOnCapturedContext);

            return await thenProc().ConfigureAwait(continueOnCapturedContext);
        }
    }

}
