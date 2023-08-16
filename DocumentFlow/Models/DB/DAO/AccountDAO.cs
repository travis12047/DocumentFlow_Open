using System.Collections;
using System.Data;

namespace DocumentFlow.Models.DB.DAO
{
	/// <summary>
	/// Account機能のDAOクラス
	/// </summary>
	public class AccountDAO
	{
		/// <summary>
		/// ユーザ名とPASSを用いてユーザマスタ(m_user)からデータを取得する
		/// </summary>
		/// <param name="name">ユーザ名</param>
		/// <param name="pass">パスワード</param>
		/// <returns>1件のレコード情報</returns>
		public static List<ArrayList> GetSingleUser(string name, string pass)
		{
			String sql =
				"select " +
				" * " +
				"from m_user " +
				"where m_user_name = '" + name + "' " +
				"and m_user_pass = '" + pass + "' ;";
			DataTable sqlResult = DAO_Master.Execute(sql);
			List<ArrayList> dataTableToList = DAO_Master.DataTableToListType(sqlResult);

			return dataTableToList;
		}
	}
}
