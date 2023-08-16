using DocumentFlow.Models.DB.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DocumentFlow.Models.ViewModel
{
	/// <summary>
	/// DocCreate機能のViewModel
	/// </summary>
	public class DocCreateViewModel
    {
		/// <summary>
		/// ユーザListを格納したフローIDのList
		/// </summary>
		public List<SelectListItem> selectListItems { get; set; }

		/// <summary>
		/// フローIDに紐づくユーザ名List
		/// </summary>
		public List<List<string>> approvalUserNameCombo { get; set; } = null;

		/// <summary>
		/// フローIDに紐づくユーザList
		/// </summary>
		public List<List<ApprovalFlowDTO>> approvalFlowCombo { get; set; }

		/// <summary>
		/// 画面で選択された承認フロー
		/// </summary>
		public string selectFlow { get; set; } = string.Empty;

		/// <summary>
		/// 画面で入力されたタイトル
		/// </summary>
		public string docTitle { get; set; }

		/// <summary>
		/// 画面で入力された内容
		/// </summary>
		public string docContent { get; set; } = string.Empty;

		/// <summary>
		/// 作成済みドキュメントフラグ
		/// </summary>
		public Boolean createdFlg { get; set; } = false;

	}
}
