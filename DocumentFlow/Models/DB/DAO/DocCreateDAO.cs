using Microsoft.CodeAnalysis;
using System.Collections;
using System.Data;

namespace DocumentFlow.Models.DB.DAO
{
	/// <summary>
	/// DocCreate機能のDAOクラス
	/// </summary>
	public class DocCreateDAO
	{
        ///// <summary>
        ///// 画面の情報を基にドキュメント作成時に必要なDBへ登録する
        ///// </summary>
        ///// <param name="docTitle">タイトル</param>
        ///// <param name="docContent">内容</param>
        ///// <param name="userId">ユーザID</param>
        ///// <param name="selectFlow">フローID</param>
        //public static void Create(string docTitle, string docContent, string userId, string selectFlow)
        //{
        //    //画面の情報を基にドキュメントマスタへデータを登録
        //    DocCreate(docTitle, docContent, userId, selectFlow);




        //}

        /// <summary>
        /// ドキュメントマスタへデータを登録する
        /// </summary>
        /// <param name="docTitle">タイトル</param>
        /// <param name="docContent">内容</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="selectFlow">フローID</param>
        public static void DocCreate(string docTitle, string docContent, string userId, string selectFlow)
        {
            String sql =
            "insert into m_document " +
                "values ( " +
                    "'null', " +
                    " '" + docTitle + "', " +
                    " '" + docContent + "', " +
                    " '" + userId + "', " +
                    " '" + selectFlow + "', " +
                    " '1', " +    //作成時のフロー順(order)は最初の人間の為、1とする
                    " DEFAULT, " +
                    " DEFAULT, " +
                    " DEFAULT, " +
                    " DEFAULT " +
                ");";
            DataTable sqlResult = DAO_Master.Execute(sql);
        }

        /// <summary>
        /// ユーザーが最後に作成したドキュメントのIDを取得する
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <returns>ドキュメントID</returns>
        public static string GetCreateDocumentId(string userId)
        {
            String sql = "select m_document_id from m_document " +
                         "where m_document_create_user_id = '" + userId + "' " +
                         "and m_document_createddata =  " +
                         "(select max(m_document_createddata) from m_document  " +
                         "where m_document_create_user_id = '" + userId + "'); ";

            DataTable sqlResult = DAO_Master.Execute(sql);
              
            return sqlResult.Rows[0][0].ToString();
        }

        /// <summary>
        /// 承認フロー情報を取得する
        /// </summary>
        /// <returns>取得内容</returns>
        public static DataTable GetApprovalFlow()
		{
			String sql =
			"select " +
			" temp_approval.m_approval_flow_id, " +
			" temp_approval.m_approval_flow_name, " +
			" temp_approval.m_approval_flow_order, " +
			" temp_approval.m_approval_flow_m_user_id, " +
			" temp_user.m_user_name " +
			"from m_approval_flow temp_approval  " +
			"left join m_user temp_user  " +
			"on temp_approval.m_approval_flow_m_user_id = temp_user.m_user_id " +
			"where temp_approval.m_approval_flow_delete_at is null " +
			" and temp_user.m_user_delete_at is null " +
			"order by temp_approval.m_approval_flow_id, temp_approval.m_approval_flow_order";
			DataTable sqlResult = DAO_Master.Execute(sql);
			List<ArrayList> dataTableToList = DAO_Master.DataTableToListType(sqlResult);

			return sqlResult;

		}

        /// <summary>
        /// 承認管理マスタへデータを登録する <br/>
        /// <br/>
        /// 取得したドキュメントIDとフローIDを基に <br/>
        /// フローIDに紐づくフロー順(order)数分レコードを作成する
        /// </summary>
        /// <param name="documentId">ドキュメントID</param>
        /// <param name="selectFlow">承認フローID</param>
        public static void ApprovalManagemenCreate(string documentId, string selectFlow)
        {
            String sql =
                "insert into m_approval_management ( " +
                "    m_approval_management_doc_id, " +
                "    m_approval_management_approvalflow_id, " +
                "    m_approval_management_order " +
                ")" +
                "    select " +
                "     '" + documentId + "' as docId, " +
                "     m_approval_flow_id, " +
                "     m_approval_flow_order " +
                "    from m_approval_flow where m_approval_flow_id = '" + selectFlow + "' " +
                ";";
            DataTable sqlResult = DAO_Master.Execute(sql);
        }
    }
}
