using DocumentFlow.Models.DB.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DocumentFlow.Models.ViewModels
{
	/// <summary>
	/// ドキュメント作成・閲覧画面のViewModel
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
		public List<List<ApprovalFlowDTO>>? approvalFlowCombo { get; set; }

		/// <summary>
		/// 画面で選択された承認フロー
		/// </summary>
		[Required(ErrorMessage = "{0}は必須です")]
		[DisplayName("承認フロー")]
		public string? selectFlow { get; set; }

        /// <summary>
        /// 画面で入力されたタイトル
        /// </summary>
        [Required(ErrorMessage = "{0}は必須です")]
        [StringLength(20)]
        [DisplayName("タイトル")]
        public string docTitle { get; set; }

        /// <summary>
        /// 画面で入力された内容
        /// </summary>
        [Required(ErrorMessage = "{0}は必須です")]
        [StringLength(400)]
        [DisplayName("内容")]
        public string docContent { get; set; } = string.Empty;

		/// <summary>
		/// 作成済みドキュメントフラグ
		/// </summary>
		public Boolean createdFlg { get; set; } = true;

	}
}
