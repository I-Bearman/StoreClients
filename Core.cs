using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace StoreClients
{
    class Core
    {
        public void Main()
        {
            SqlConnectionStringBuilder SQLConStr = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "CustomersMSSQLLocalDB",
                IntegratedSecurity = true,
                Pooling = true
            };

            OleDbConnectionStringBuilder AccessConStr = new OleDbConnectionStringBuilder()
            {
                DataSource = "ProductsMSAccess.accdb",
                Provider = "Microsoft.ACE.OLEDB.12.0",
                PersistSecurityInfo = true
            };

            SqlConnection sqlConnection = new SqlConnection(SQLConStr.ConnectionString);

            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {

            }
            finally
            {
                sqlConnection.Close();
            }


            OleDbConnection oleDbConnection = new OleDbConnection(AccessConStr.ConnectionString);
            try
            {
                oleDbConnection.Open();
            }
            catch (Exception e)
            {

            }
            finally
            {
                oleDbConnection.Close();
            }
        }
    }
}
