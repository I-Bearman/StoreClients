using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoreClients
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection connectionSQL = new SqlConnection(sQLConStr.ConnectionString);
        SqlDataAdapter dataAdapterSQL = new SqlDataAdapter();
        DataTable dataTableSQL = new DataTable();

        OleDbConnection oleDbConnection = new OleDbConnection(accessConStr.ConnectionString);
        OleDbDataAdapter dataAdapterOle = new OleDbDataAdapter();
        DataTable dataTableOle = new DataTable();


        DataRowView dataRow;

        public MainWindow()
        {
            InitializeComponent();
            Preparing();
        }

        private void Preparing()
        {
            Thread SQLConnectThread = new Thread(() =>
            {
                #region Init

                SqlConnectionStringBuilder sQLConStr = new SqlConnectionStringBuilder()
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "CustomersMSSQLLocalDB",
                    IntegratedSecurity = true,
                };

                #endregion

                #region select

                string query = @"SELECT * FROM Customers";
                dataAdapterSQL.SelectCommand = new SqlCommand(query, connectionSQL);
                dataAdapterSQL.Fill(dataTableSQL);

                #endregion

                #region insert

                query = @"INSERT INTO Customers (LastName,  FirstName, FatherName, PhoneNumber,  Email) 
                                 VALUES (@LastName,  @FirstName, @FatherName, @PhoneNumber,  @Email); 
                     SET @id = @@IDENTITY;";

                dataAdapterSQL.InsertCommand = new SqlCommand(query, connectionSQL);

                dataAdapterSQL.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
                dataAdapterSQL.InsertCommand.Parameters.Add("@LastName", SqlDbType.NVarChar, 20, "LastName");
                dataAdapterSQL.InsertCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar, 10, "FirstName");
                dataAdapterSQL.InsertCommand.Parameters.Add("@FatherName", SqlDbType.NVarChar, 15, "FatherName");
                dataAdapterSQL.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 10, "PhoneNumber");
                dataAdapterSQL.InsertCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 20, "Email");

                #endregion

                #region update

                query = @"UPDATE Workers SET 
                           workerName = @workerName,
                           idBoss = @idBoss, 
                           idDescription = @idDescription 
                    WHERE id = @id";

                dataAdapterSQL.UpdateCommand = new SqlCommand(query, connectionSQL);
                dataAdapterSQL.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 4, "Id").SourceVersion = DataRowVersion.Original;
                dataAdapterSQL.UpdateCommand.Parameters.Add("@LastName", SqlDbType.NVarChar, 20, "LastName");
                dataAdapterSQL.UpdateCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar, 10, "FirstName");
                dataAdapterSQL.UpdateCommand.Parameters.Add("@FatherName", SqlDbType.NVarChar, 15, "FatherName");
                dataAdapterSQL.UpdateCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 10, "PhoneNumber");
                dataAdapterSQL.UpdateCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 20, "Email");

                #endregion

                #region delete

                query = "DELETE FROM Workers WHERE id = @id";

                dataAdapterSQL.DeleteCommand = new SqlCommand(query, connectionSQL);
                dataAdapterSQL.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "Id");

                #endregion

                dataAdapterSQL.Fill(dataTableSQL);

            });

            Thread OleConnectThread = new Thread(() =>
            {
                OleDbConnectionStringBuilder accessConStr = new OleDbConnectionStringBuilder()
                {
                    DataSource = @"ProductsMSAccess.accdb",
                    Provider = "Microsoft.ACE.OLEDB.12.0",
                };
            });

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clientListDG.DataContext = dataTable;
            clientListDG.Items.Refresh();
        }

        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dataRow = (DataRowView)clientListDG.SelectedItem;
            dataRow.BeginEdit();

        }

        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (dataRow == null) return;
            dataRow.EndEdit();
            dataAdapterSQL.Update(dataTable);

        }
    }
}
