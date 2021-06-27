using System;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{
    public static class MockDataTransactionAsyncExtension
    {
        // TIPS: customizzare classe dataTransaction
        //public static Task CommitAsync(this IDataTransaction dataTransaction)
        //{
        //    if (dataTransaction is IDataTransaction _dt)
        //    {
        //        return _dt.CommitAsync();
        //    }
        //    throw new InvalidCastException($"Unable to cast type of '{dataTransaction.GetType().FullName}' to " +
        //                                   $"interface '{typeof(IDataTransaction).FullName}'.");
        //}

        //public static Task RollBackAsync(this IDataTransaction dataTransaction)
        //{
        //    if (dataTransaction is IDataTransaction _dt)
        //    {
        //        return _dt.RollBackAsync();
        //    }
        //    throw new InvalidCastException($"Unable to cast type of '{dataTransaction.GetType().FullName}' to " +
        //                                   $"interface '{typeof(IDataTransaction).FullName}'.");
        //}

        public static Task CommitAsync(this IDataTransaction mockDataTransaction)
        {
            // Call base method
            mockDataTransaction.Commit();
            return Task.CompletedTask;
        }

        public static Task RollBackAsync(this IDataTransaction mockDataTransaction)
        {
            // Call base method
            mockDataTransaction.Rollback();
            return Task.CompletedTask;
        }
    }

  //  /// TIPS: customizzare classe dataTransaction
  //  /// <summary>
  //  /// Data transaction implementation for mockup
  //  /// </summary>
  //  public class MockDataTransactionAsync : MockDataTransaction, IDataTransactionAsync
  //  {
  //      /// <summary>
  //      /// Constructor
  //      /// </summary>
  //      public MockDataTransactionAsync(IMockDataSessionAsync dataSession): base(dataSession) {}

  //      /// <summary>
  //      /// Executes commit of transaction
  //      /// </summary>
  //      public Task CommitAsync()
  //      {
  //          // Call base method
  //          this.Commit();
  //          return Task.CompletedTask;
  //      }

  //      /// <summary>
  //      /// Executes rollback of transaction
  //      /// </summary>
  //      public Task RollBackAsync()
  //      {
  //          // Call base method
  //          this.Rollback();
  //          return Task.CompletedTask;
  //      }

  //      /// <summary>
  //      /// Finalizes that ensures the object is correctly disposed of.
		///// </summary>
  //      ~MockDataTransactionAsync()
  //      {
  //          //Implicit dispose
  //          Dispose(false);
  //      }

  //  }
}
