using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;

namespace DocumentFlow.Models.DB.DAO
{
    public class DocSearchDAO
    {
		/// <summary>
		/// 検索画面に表示するドキュメント情報を取得する
		/// </summary>
		/// <param name="searchCondTitleList">タイトルTextBoxの内容</param>
		/// <returns>検索結果</returns>
		public static DataTable GetSearchResults(List<string> searchCondTitleList)
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
            "on approval_flow.m_approval_flow_m_user_id = approvalflow_current_user.m_user_id " +
            //ドキュメントが削除されていないものを取得する
            "where base.m_document_management_deleted_at is null ";
			////表示件数を指定
			//"order by base.m_document_id " +
			//"LIMIT "+ limitStart + ", "+ limitEnd  +

			// 検索条件に「タイトル」の内容を追加する
			sql += AddSearchCondTitle(searchCondTitleList);

			sql += "; ";


			DataTable sqlResult = DAO_Master.Execute(sql);

            return sqlResult;

        }

        /// <summary>
        /// 検索条件に「タイトル」の内容を追加する
        /// </summary>
        /// <param name="searchCondTitleList">キーワードが格納されているList</param>
        /// <returns>キーワードを基に作成されたLike句</returns>
        private static string AddSearchCondTitle(List<string> searchCondTitleList)
		{
            String sql = "";
			bool firstLoop = true;
			foreach (var word in searchCondTitleList)
			{
				string escapeWord = StringEscape(word);
				if (firstLoop)
				{
					sql += "and (base.m_document_title like '%" + escapeWord + "%' ";
					firstLoop = false;
				}
				else
				{
					sql += "or base.m_document_title like '%" + escapeWord + "%' ";
				}
			}
            //Like句が一度でも作成されている場合、括弧で閉じる
            if(!firstLoop)
			{
				sql += ") ";
			}

            return sql;

		}
        /// <summary>
        /// 文字列に対してエスケープ処理を行う
        /// </summary>
        /// <param name="targetString">対象文字列</param>
        /// <returns>エスケープ処理を施した文字列</returns>
		private static string StringEscape(string targetString)
		{
			string returnString = Regex.Replace(targetString, @"[%_\[']", "\\$0");
			return returnString;
		}

	}
}