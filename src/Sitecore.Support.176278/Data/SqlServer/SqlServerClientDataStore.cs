using Sitecore.Data.DataProviders.Sql;
using System;
using Sitecore.Eventing;
using Sitecore.Abstractions;

namespace Sitecore.Support.Data.SqlServer
{
  public class SqlServerClientDataStore : Sitecore.Data.SqlServer.SqlServerClientDataStore
  {
    public SqlServerClientDataStore(SqlDataApi api, string objectLifetime, IEventQueue queue, BaseEventManager eventManager) : base(api, objectLifetime, queue, eventManager)
    {
    }

    public SqlServerClientDataStore(string connectionString, string objectLifetime, IEventQueue queue, BaseEventManager eventManager) : base(connectionString, objectLifetime, queue, eventManager)
    {
    }

    protected override void CompactData()
    {
      var batchSize = Configuration.Settings.LinkDatabase.MaximumBatchSize;
      DateTime time = DateTime.UtcNow - ObjectLifetime;

      string sql = "DELETE TOP ({2}batchSize{3}) FROM {0}ClientData{1} WHERE {0}Accessed{1} < {2}maxDate{3} SELECT @@ROWCOUNT";
      object[] parameters = new object[] { "batchSize", batchSize, "maxDate", time };

      var affectedRows = 0;

      do
      {
        affectedRows = this._api.Execute(sql, parameters);
      }
      while (affectedRows > 0);      
    }
  }
}