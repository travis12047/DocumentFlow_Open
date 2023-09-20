using AutoMapper;
using DocumentFlow.Models.CommonModels;
using DocumentFlow.Models.DB.DAO;
using DocumentFlow.Models.DB.DTO;
using DocumentFlow.Models.ViewModels;
using System.Collections;
using System.Data;

namespace DocumentFlow.Models
{
    /// <summary>
    /// ドキュメント検索画面のモデル
    /// </summary>
    public class DocSearchModel
    {
		/// <summary>
		/// 画面表示に必要なViewModelの作成
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <param name="limitNumList">表示件数プルダウンの値</param>
		/// <param name="searchCondApprovalStatusList">承認状況ラジオボタンの値</param>
		/// <param name="pageNum">画面で表示中のページ番号</param>
		/// <param name="limitNum">表示件数</param>
		/// <param name="searchCondApprovalStatus">承認状況</param>
		/// <param name="searchCondTitle">タイトルTextBoxの内容</param>
		/// <returns>項目設定済みのviewModel</returns>
		public static DocSearchViewModel CreateViewModel(DocSearchViewModel viewModel, string userId,
		//初期表示、検索処理実行時に使用される引数
		string limitNumList, string searchCondApprovalStatusList,
		//ページリンク押下時に使用される引数
		string pageNum, string limitNum, string searchCondApprovalStatus,
		//全ての処理で使用される引数
		string searchCondTitle)
		{
			/***********************************************
			 *画面上で設定した検索条件を保存
			 **********************************************/
			//タイトル
			viewModel.searchCondTitle = searchCondTitle;
			//ページ番号
			if (!string.IsNullOrEmpty(pageNum))
			{
				//初期表示または検索ボタン押下時
				viewModel.pagingItem.pageIndexNum = int.Parse(pageNum);
			}
			//表示件数系の設定
			viewModel = LimitNumSetter(viewModel, limitNumList, limitNum);
			//承認状況系の設定
			viewModel = ApprovalStatusSetter(viewModel, searchCondApprovalStatusList, searchCondApprovalStatus);


			/***********************************************
			 *検索処理の実行
			 **********************************************/
			DataTable searchResultsDt = GetSearchResults(searchCondTitle, userId, viewModel.searchCondApprovalStatus);


			/***********************************************
			 *ページングリンク作成
			 **********************************************/
			viewModel.pagingItem = PagingModel.CreatePageLync(viewModel.pagingItem, searchResultsDt);


			/***********************************************
			 *取得した検索結果を画面表示用に加工し
			 *viewModelに設定
			 **********************************************/
			//viewModel.searchResults = CreateSearchResults(searchResultsDt, (int)viewModel.limitNum, (int)viewModel.pageIndexNum);
			searchResultsDt = PagingModel.CreateDisplayDataTable(searchResultsDt, (int)viewModel.pagingItem.limitNum, (int)viewModel.pagingItem.pageIndexNum);
			viewModel.searchResults = CreateDisplayList(searchResultsDt);

			return viewModel;
		}

		/// <summary>
		/// 検索処理の実行
		/// </summary>
		/// <param name="searchCondTitle">タイトルTextBoxの内容</param>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <param name="searchCondApprovalStatus">承認状況</param>
		/// <returns>検索結果を格納したDataTable</returns>
		private static DataTable GetSearchResults(string searchCondTitle, string userId, int? searchCondApprovalStatus)
		{
			//「タイトル」に文字が入力されている場合、
			//ブランクでスプリットし、Listとしてまとめる
			List<string> searchCondTitleList = new List<string>();
			if (!string.IsNullOrEmpty(searchCondTitle))
			{
				searchCondTitleList = StrignSplitter(searchCondTitle);
			}
			//検索処理の実行
			DataTable searchResultsDt = DocSearchDAO.GetSearchResults(searchCondTitleList, userId, searchCondApprovalStatus);
			return searchResultsDt;

		}
		/// <summary>
		/// 文字列をブランクでスプリットし、Listとして返す
		/// </summary>
		/// <param name="targetString">対象文字列</param>
		/// <returns>スプリットした文字列のList</returns>
		private static List<string> StrignSplitter(string targetString)
		{
			string[] words = targetString.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);

			List<string> splitWords = new List<string>();
			foreach (var word in words)
			{
				splitWords.Add(word);
			}

			return splitWords;
		}
		/// <summary>
		/// 表示件数情報の設定
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <param name="limitNumList">表示件数リストで選択された値</param>
		/// <param name="limitNum">ページリンク押下時に渡される値</param>
		/// <returns>表示件数情報が設定されたviewModel</returns>
		public static DocSearchViewModel LimitNumSetter(DocSearchViewModel viewModel, string limitNumList, string limitNum)
		{
			//表示件数設定
			if (!string.IsNullOrEmpty(limitNumList))
			{
				//初期表示または検索ボタン押下時
				viewModel.pagingItem.limitNum = int.Parse(limitNumList);
			}
			else if (!string.IsNullOrEmpty(limitNum))
			{
				//ページリンク押下時
				viewModel.pagingItem.limitNum = int.Parse(limitNum);
			}

			//表示件数選択プルダウンの選択状態設定
			int limitNumListLoopCount = 0;
			foreach (var i in viewModel.limitNumList)
			{
				if (int.Parse(i.Value) == viewModel.pagingItem.limitNum)
				{
					viewModel.limitNumList[limitNumListLoopCount].Selected = true;
				}
				limitNumListLoopCount++;
			}

			return viewModel;

		}
		/// <summary>
		/// 承認状況ラジオボタンの設定
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <param name="searchCondApprovalStatusList">初期表示または検索ボタン押下時に渡される値</param>
		/// <param name="searchCondApprovalStatus">ページリンク押下時に渡される値</param>
		/// <returns>承認状況情報が設定されたviewModel</returns>
		public static DocSearchViewModel ApprovalStatusSetter(DocSearchViewModel viewModel, string searchCondApprovalStatusList, string searchCondApprovalStatus)
		{
			//承認状況設定
			if (!string.IsNullOrEmpty(searchCondApprovalStatusList))
			{
				//初期表示または検索ボタン押下時
				viewModel.searchCondApprovalStatus = int.Parse(searchCondApprovalStatusList);
			}
			else if (!string.IsNullOrEmpty(searchCondApprovalStatus))
			{
				//ページリンク押下時
				viewModel.searchCondApprovalStatus = int.Parse(searchCondApprovalStatus);
			}

			foreach (var i in viewModel.searchCondApprovalStatusList)
			{
				if (i.id == viewModel.searchCondApprovalStatus)
				{
					viewModel.searchCondApprovalStatusList[i.id].Selected = true;
				}
			}

			return viewModel;

		}
		/// <summary>
		/// 取得したDataTableをM_DocumentDisplayDTO型のListとして返す
		/// </summary>
		/// <param name="originalDataTable">承認済みの未読情報が格納されたDataTable</param>
		/// <returns>M_DocumentDisplayDTO型のList</returns>
		private static List<M_DocumentDisplayDTO> CreateDisplayList(DataTable originalDataTable)
		{
			//datatableをListに変形
			List<ArrayList> originalList = CommonModel.DataTableToListType(originalDataTable);

			//Listを指定のDTOクラスの形にマッピング
			IMapper mapper = CommonModel.CreateMapper();
			var mapperList = mapper.Map<List<ArrayList>, List<M_DocumentDisplayDTO>>(originalList);

			return mapperList;

		}
	}
}
