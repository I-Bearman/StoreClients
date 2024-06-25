using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Windows;
using System.Data;

namespace StoreClients
{
    class Core
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
        public void Initial()
        {
           
            Thread SQLConnectThread = new Thread(() =>
            {
                ReadFromClientsSQLDB();
            });

           /* Thread OleConnectThread = new Thread(() =>
            {

                using (OleDbConnection oleDbConnection = new OleDbConnection(AccessConStr.ConnectionString))
                    try
                    {
                        oleDbConnection.Open();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Невозможно подключиться к базе данных товаров!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
            });*/

            SQLConnectThread.Start();
            //OleConnectThread.Start();
        }
        public DataTable ReadFromClientsSQLDB()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(SQLConStr.ConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Невозможно подключиться к базе данных клиентов!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                string query = "SELECT * FROM Customers";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                {                    
                    dataAdapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
        public void AddClientToClientsSQLDB()
        {

        }
    }
}
