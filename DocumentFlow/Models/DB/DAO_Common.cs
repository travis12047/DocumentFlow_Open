namespace DocumentFlow.Models.DB
{
	/// <summary>
	/// DAOの共通クラス
	/// </summary>
	public class DAO_Common
	{
		/// <summary>
		/// ドキュメントマスタをベースに<br/>
		/// 画面表示用に関連マスタをLeft JoinしたSQL文を返す
		/// </summary>
		/// <returns>SQL文</returns>
		public static String M_DocumentDisplay()
		{
			String sql =
			"select	 " +
			" base.m_document_id document_id, " +
			" base.m_document_title document_title, " +
			" create_user.m_user_name create_user_name, " +
			//ドキュメントが完了済みの場合「完了」と表示
			//そうでない場合承認待ちユーザ名を表示
			" case " +
			"  when base.m_document_completion_at is not null then '完了済み' " +
			"  else approvalflow_current_user.m_user_name " +
			" end approval_status, " +
			" base.m_document_completion_at completion_at, " +
			" base.m_document_createddata createddata, " +
			" base.m_document_updatedata updatedata " +
			"from m_document base " +
			//ドキュメント作成者紐づけ
			"left join m_user create_user " +
			"on base.m_document_create_user_id = create_user.m_user_id " +
			//承認フローIDと現在の承認順を紐づけ
			"left join m_approval_flow approval_flow " +
			"on base.m_document_approvalflow_id = approval_flow.m_approval_flow_id " +
			"and base.m_document_approvalflow_current_order = approval_flow.m_approval_flow_order " +
			//上記で取得した承認情報を基に現在の承認待ちユーザ名を取得
			"left join m_user approvalflow_current_user " +
			"on approval_flow.m_approval_flow_m_user_id = approvalflow_current_user.m_user_id ";

			return sql;
		}
	}
}