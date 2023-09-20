namespace DocumentFlow.Models.ViewModels
{
	public class PagingViewModel
	{

		/// <summary>
		/// 表示件数
		/// </summary>
		public int? limitNum { get; set; }
		//public int? limitNum { get; set; } = 5;
		/// <summary>
		/// 現在のページ番号
		/// </summary>
		public int? pageIndexNum { get; set; }
		//public int? pageIndexNum { get; set; } = 1;
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
		public int? maxPageNum { get; set; }
		//public int? maxPageNum { get; set; } = 6;
		/// <summary>
		/// maxPageNumを半分で割った値
		/// </summary>
		public int? maxPageNumHalf { get; set; }
	}
}
