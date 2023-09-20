using DocumentFlow.Models.DB.DTO;

namespace DocumentFlow.Models.ViewModels
{
	/// <summary>
	/// ホーム画面のViewModel
	/// </summary>
	public class HomeViewModel
	{
		/// <summary>
		/// 承認済みの未読情報を格納するList
		/// </summary>
		public List<M_DocumentDisplayDTO>? approvedUnreadList { get; set; }

		/// <summary>
		/// 承認が必要な情報を格納するList
		/// </summary>
		public List<M_DocumentDisplayDTO>? requiringApprovalList { get; set; }

		/// <summary>
		/// 承認済み未読情報画面のページングアイテム群
		/// </summary>
		public PagingViewModel approvedUnreadPagingItem = new PagingViewModel()
		{
			/// <summary>
			/// 表示件数
			/// </summary>
			limitNum = 5,

			/// <summary>
			/// 現在のページ番号
			/// </summary>
			pageIndexNum = 1,

			/// <summary>
			/// 画面に表示する最大のページ番号
			/// </summary>
			maxPageNum = 9
		};

		/// <summary>
		/// 承認必要情報画面のページングアイテム群
		/// </summary>
		public PagingViewModel requiringApprovalPagingItem = new PagingViewModel()
		{
			/// <summary>
			/// 表示件数
			/// </summary>
			limitNum = 5,

			/// <summary>
			/// 現在のページ番号
			/// </summary>
			pageIndexNum = 1,

			/// <summary>
			/// 画面に表示する最大のページ番号
			/// </summary>
			maxPageNum = 9
		};
	}
}
