﻿using System.Data;
using System.Text.RegularExpressions;

namespace DocumentFlow.Models.DB.DAO
{
    public class DocSearchDAO
    {
		/// <summary>
		/// 検索画面に表示するドキュメント情報を取得する
		/// </summary>
		/// <param name="searchCondTitleList">タイトルTextBoxの内容</param>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <param name="searchCondApprovalStatus">承認状況区分</param>
		/// <returns>検索結果</returns>
		public static DataTable GetSearchResults(List<string> searchCondTitleList, string userId, int? searchCondApprovalStatus)
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

			//自身が承認フローに含まれているもの、
			//自身が作成したもの、
			//承認済みのものを取得する
			//sql +=
			//"and ( " +
			//"    base.m_document_approvalflow_id in ( " +
			//"		select " +
			//"			m_approval_flow_id " +
			//"		from m_approval_flow " +
			//"		where m_approval_flow_m_user_id = '" + userId + "' " +
			//"    ) " +
			//"    or " +
			//"    base.m_document_create_user_id = '" + userId + "' " +
			//"    or " +
			//"    base.m_document_completion_at is not null " +
			//") ";

			//検索条件に「タイトル」の内容を追加する
			sql += AddSearchCondTitle(searchCondTitleList);

			//検索条件に「承認状況」の内容を追加する
			sql += AddSearchCondApprovalStatus(searchCondApprovalStatus, userId);

			//デフォルトの並び順は更新日時の降順とする
			sql += "order by base.m_document_updatedata desc ";
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

		/// <summary>
		/// 検索条件に「承認状況」の内容を追加する
		/// </summary>
		/// <param name="searchCondApprovalStatus">承認状況区分</param>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <returns>指定の承認状況を基に作成された条件句</returns>
		private static string AddSearchCondApprovalStatus(int? searchCondApprovalStatus, string userId)
		{
			String sql = "";

			switch (searchCondApprovalStatus)
			{
				//全て
				case 0:
					//全ての場合SQLの生成は行わない
					break;

				//承認済み
				case 1:
					sql += "and base.m_document_completion_at is not null ";
					break;

				//あなたの承認待ち
				case 2:
					sql += "and approval_flow.m_approval_flow_m_user_id = '" + userId + "' ";
					break;
			}

			return sql;

		}

	}
}