using System.Data.Entity;
using System.Data.Entity.SqlServer;

/* This implements connection resiliency by adding
 * a new execution strategy, so we can retry errors
 * caused by temporary losses in connectivity and configure
 * how many times to retry.
 */

namespace ASC_Coil_Tracker_Production.Data_Access_Layer
{
    public class CoilConfiguration : DbConfiguration
    {
        public CoilConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}