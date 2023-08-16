using System.Data;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System.Collections;

namespace DocumentFlow.Models.DB
{
	/// <summary>
	/// DBサーバとのやり取りを実行するクラス
	/// </summary>
	public class DAO_Master
	{
		/// <summary>
		/// データベースにSQLを実行する
		/// </summary>
		/// <param name="sql">実行するSQL文</param>
		/// <returns>実行結果(空の場合もあり)</returns>
		public static DataTable Execute(String sql)
		{
			var pkFile = new PrivateKeyFile("documentflow-ec2.pem", "");
			DataTable tbl = new DataTable("tbl");


			MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder();
			connBuilder.AllowBatch = true;
			connBuilder.Server = "localhost";
			connBuilder.Port = 3306;
			connBuilder.UserID = "root";
			connBuilder.Password = "YFAvvB1HsKYiG4eBXYDX";
			connBuilder.Database = "documentflow";
			connBuilder.Pooling = true;
			connBuilder.ConnectionTimeout = 600;
			connBuilder.DefaultCommandTimeout = 600;
			connBuilder.CharacterSet = "utf8";

			using (var client = new SshClient("52.196.16.106", 22, "ec2-user", pkFile))
			{
				client.Connect();
				var forward = new ForwardedPortLocal("127.0.0.1", 3306, "documentflow-db.coxflyfeylll.ap-northeast-1.rds.amazonaws.com", 3306);
				client.AddForwardedPort(forward);
				forward.Start();
				using (var connection = new MySqlConnection(connBuilder.ToString()))
				{
					connection.Open();

					using (var com = new MySqlCommand(sql, connection))
					{
						com.CommandType = CommandType.Text;
						//var ds = new DataSet();
						using (var comExe = com.ExecuteReader())
						{
							tbl.Load(comExe);
						}
					}
					forward.Stop();
				}
			}
			return tbl;
		}

		/// <summary>
		/// SQL実行結果が格納されているDataTableをListに変形させる
		/// </summary>
		/// <param name="sqlResult">SQL実行結果が格納されているDataTable</param>
		/// <returns>SQL実行結果が格納されているList</returns>
		public static List<ArrayList> DataTableToListType(DataTable sqlResult)
		{
			int rowCount = sqlResult.Rows.Count;
			int columnCount = sqlResult.Columns.Count;
			var resultList = new List<ArrayList>();

			for (int row = 0; row < rowCount; row++)
			{
				var rowList = new ArrayList();
				for (int column = 0; column < columnCount; column++)
				{
					rowList.Add(sqlResult.Rows[row][column]);
				}
				resultList.Add(rowList);
			}

			return resultList;
		}
	}
}