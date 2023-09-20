using System.Data;

namespace DocumentFlow.Models.DB.DAO
{
	/// <summary>
	/// ホーム画面のDAOクラス
	/// </summary>
	public class HomeDAO
	{
		/// <summary>
		/// 承認済みの未読のドキュメント情報を取得する
		/// </summary>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <returns>承認済みの未読情報</returns>
		public static DataTable GetApprovedUnreadDoc(string userId)
		{
			//DAOの共通クラスからドキュメントマスタ表示用SQLを呼び出し
			String sql = DAO_Common.M_DocumentDisplay();
			sql +=
			//ドキュメントが削除されていないものを取得する
			"where base.m_document_management_deleted_at is null " +
			//承認が完了しているものを取得する
			"and base.m_document_completion_at is not null " +
			//ドキュメント確認テーブルを参照し、
			//未読のドキュメントを取得する
			"and base.m_document_id in ( " +
			"	select " +
			"		t_doc_confirmation_doc_id " +
			"	from t_doc_confirmation " +
			"	where t_doc_confirmation_user_id = '" + userId + "' " +
			") ";


			//デフォルトの並び順は更新日時の降順とする
			sql += "order by base.m_document_updatedata desc ";
			sql += "; ";


			DataTable sqlResult = DAO_Master.Execute(sql);

			return sqlResult;

		}

		/// <summary>
		/// ユーザの承認待ちのドキュメント情報を取得する
		/// </summary>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <returns>承認待ち情報</returns>
		public static DataTable GetRequiringApprovalDoc(string userId)
		{
			//DAOの共通クラスからドキュメントマスタ表示用SQLを呼び出し
			String sql = DAO_Common.M_DocumentDisplay();
			sql +=
			//ドキュメントが削除されていないものを取得する
			"where base.m_document_management_deleted_at is null " +
			//承認が完了していないものを取得する
			"and base.m_document_completion_at is null " +
			//ユーザの承認待ちのものを取得する
			"and approval_flow.m_approval_flow_m_user_id = '" + userId + "' ";


			//デフォルトの並び順は更新日時の降順とする
			sql += "order by base.m_document_updatedata desc ";
			sql += "; ";


			DataTable sqlResult = DAO_Master.Execute(sql);

			return sqlResult;

		}
	}
}
