using Sitecore.Data.DataProviders.Sql;
using System;

namespace Sitecore.Support.Data.SqlServer
{
  public class SqlServerClientDataStore : Sitecore.Data.SqlServer.SqlServerClientDataStore
  {
    public SqlServerClientDataStore(SqlDataApi api, string objectLifetime) : base(api, objectLifetime)
    {
    }

    public SqlServerClientDataStore(string connectionString, string objectLifetime) : base(connectionString, objectLifetime)
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