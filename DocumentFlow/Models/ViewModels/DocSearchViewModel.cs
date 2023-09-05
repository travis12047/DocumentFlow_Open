using DocumentFlow.Models.DB.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace DocumentFlow.Models.ViewModels
{
	/// <summary>
	/// DocSearch機能のViewModel
	/// </summary>
	public class DocSearchViewModel
	{
		/// <summary>
		/// 検索結果を格納するList
		/// </summary>
		public List<SearchResultsDTO>? searchResults { get; set; }

		/// <summary>
		/// 表示件数
		/// </summary>
		public int? limitNum { get; set; } = 5;
		/// <summary>
		/// 表示件数プルダウン用のList
		/// </summary>
		public List<SelectListItem> limitNumList = new List<SelectListItem>
		{
			new SelectListItem()
			{
				Text = "5 件",
				Value = "5"
			},
			new SelectListItem()
			{
				Text = "10 件",
				Value = "10"
			},
			new SelectListItem()
			{
				Text = "15 件",
				Value = "15"
			},
			new SelectListItem()
			{
				Text = "20 件",
				Value = "20"
			}
		};
		/// <summary>
		/// 承認状況
		/// </summary>
		public int? searchCondApprovalStatus { get; set; }
		/// <summary>
		/// 承認状況ラジオボタン用のClass
		/// </summary>
		public class ApprovalStatusList
		{
			public int id;
			public string caption;
			public bool Selected;
		}
		/// <summary>
		/// 承認状況ラジオボタン用のList
		/// </summary>
		public List<ApprovalStatusList> searchCondApprovalStatusList = new List<ApprovalStatusList>
		{
			new ApprovalStatusList()
			{
				id = 0,
				caption = "全て",
				Selected = true
			},
			new ApprovalStatusList()
			{
				id = 1,
				caption = "承認済み",
				Selected = false
			},
			new ApprovalStatusList()
			{
				id = 2,
				caption = "あなたの承認待ち",
				Selected = false
			}
		};

		/// <summary>
		/// 検索条件：タイトル
		/// </summary>
		[DisplayName("タイトル")]
		public string? searchCondTitle { get; set; }

		/// <summary>
		/// 現在のページ番号
		/// </summary>
		public int? pageIndexNum { get; set; } = 1;
		/// <summary>
		/// 表示件数と検索結果から算出した最後のページ番号
		/// </summary>
		public int? pageLastNum { get; set; }
		/// <summary>
		/// 画面に表示する最小のページ番号
		/// </summary>
		public int? minPageNum { get; set; }
		/// <summary>
		/// 画面に表示する最大のページ番号
		/// </summary>
		public int? maxPageNum { get; set; } = 6;
		/// <summary>
		/// maxPageNumを半分で割った値
		/// </summary>
		public int? maxPageNumHalf { get; set; }
	}
}
