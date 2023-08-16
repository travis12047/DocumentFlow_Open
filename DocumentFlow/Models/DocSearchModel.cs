using AutoMapper;
using DocumentFlow.Models.DB;
using DocumentFlow.Models.DB.DAO;
using DocumentFlow.Models.DB.DTO;
using DocumentFlow.Models.ViewModels;
using DocumentFlow.Profile;
using System.Collections;
using System.Data;

namespace DocumentFlow.Models
{
    public class DocSearchModel
    {
		/// <summary>
		/// 画面表示に必要なViewModelの作成
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <param name="limitNumList">表示件数プルダウンの値</param>
		/// <param name="searchCondApprovalStatusList">承認状況ラジオボタンの値</param>
		/// <param name="pageNum">画面で表示中のページ番号</param>
		/// <param name="limitNum">表示件数</param>
		/// <param name="searchCondApprovalStatus">承認状況</param>
		/// <param name="searchCondTitle">タイトルTextBoxの内容</param>
		/// <returns>項目設定済みのviewModel</returns>
		public static DocSearchViewModel CreateViewModel(DocSearchViewModel viewModel,
		//初期表示、検索処理実行時に使用される引数
		string limitNumList, string searchCondApprovalStatusList,
		//ページリンク押下時に使用される引数
		string pageNum, string limitNum, string searchCondApprovalStatus,
		//全ての処理で使用される引数
		string searchCondTitle)
		{
			/***********************************************
			 *検索処理の実行
			 **********************************************/
			DataTable searchResultsDt = GetSearchResults(searchCondTitle);

			/***********************************************
			 *画面上で設定した検索条件を保存
			 **********************************************/
			//タイトル
			viewModel.searchCondTitle = searchCondTitle;
			//ページ番号
			if (!string.IsNullOrEmpty(pageNum))
			{
				//初期表示または検索ボタン押下時
				viewModel.pageIndexNum = int.Parse(pageNum);
			}
			//表示件数系の設定
			viewModel = LimitNumSetter(viewModel, limitNumList, limitNum);
			//承認状況系の設定
			viewModel = ApprovalStatusSetter(viewModel, searchCondApprovalStatusList, searchCondApprovalStatus);
			//ページングリンク作成
			viewModel = CreatePageLync(viewModel, searchResultsDt);

			/***********************************************
			 *取得した検索結果を画面表示用に加工し
			 *viewModelに設定
			 **********************************************/
			viewModel.searchResults = CreateSearchResults(searchResultsDt, viewModel.limitNum, viewModel.pageIndexNum);

			return viewModel;
		}

		/// <summary>
		/// 検索処理の実行
		/// </summary>
		/// <param name="searchCondTitle">タイトルTextBoxの内容</param>
		/// <returns>検索結果を格納したDataTable</returns>
		private static DataTable GetSearchResults(string searchCondTitle)
		{
			//「タイトル」に文字が入力されている場合、
			//ブランクでスプリットし、Listとしてまとめる
			List<string> searchCondTitleList = new List<string>();
			if (!string.IsNullOrEmpty(searchCondTitle))
			{
				searchCondTitleList = StrignSplitter(searchCondTitle);
			}
			//検索処理の実行
			DataTable searchResultsDt = DocSearchDAO.GetSearchResults(searchCondTitleList);
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
				viewModel.limitNum = int.Parse(limitNumList);
			}
			else if (!string.IsNullOrEmpty(limitNum))
			{
				//ページリンク押下時
				viewModel.limitNum = int.Parse(limitNum);
			}

			//表示件数選択プルダウンの選択状態設定
			int limitNumListLoopCount = 0;
			foreach (var i in viewModel.limitNumList)
			{
				if (int.Parse(i.Value) == viewModel.limitNum)
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
		/// ページリンク系の作成
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <param name="searchResultsDt">ページ数算出に使用する検索結果</param>
		/// <returns>ページリンク設定後のviewModel</returns>
		private static DocSearchViewModel CreatePageLync(DocSearchViewModel viewModel, DataTable searchResultsDt)
		{
			viewModel.maxPageNumHalf = viewModel.maxPageNum / 2;
			viewModel.pageLastNum = PageCountCalculator(searchResultsDt, viewModel.limitNum);
			int preMaxPageNum = PreMaxPageNumMaker(viewModel.maxPageNum, viewModel.pageLastNum);
			viewModel.minPageNum = MinPageNumMaker(viewModel.pageIndexNum, viewModel.maxPageNumHalf, viewModel.pageLastNum, preMaxPageNum);
			viewModel.maxPageNum = MaxPageNumMaker(viewModel.minPageNum, viewModel.maxPageNum, viewModel.pageLastNum);

			return viewModel;
		}
		/// <summary>
		/// 検索結果をList化し、表示するページ番号の件数分だけ返す
		/// </summary>
		/// <param name="searchResultsDt">検索結果datatable</param>
		/// <param name="limitNum">表示件数</param>
		/// <param name="pageIndexNum">インデックスページ数</param>
		/// <returns></returns>
		private static List<SearchResultsDTO> CreateSearchResults
		(DataTable searchResultsDt, int limitNum, int pageIndexNum)
		{
			/***********************************************
			 *引数のdatatableから、
			 *DTOクラスの形にマッピングされたListを作成
			 **********************************************/
			//datatableをListに変形
			List<ArrayList> searchResultsList = DAO_Master.DataTableToListType(searchResultsDt);

			//Listを指定のDTOクラスの形にマッピング
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<AutoMapperConfig>();
			});
			var mapper = config.CreateMapper();
			var mapperList = mapper.Map<List<ArrayList>, List<SearchResultsDTO>>(searchResultsList);



			/***********************************************
			 *表示するページ番号の件数分だけ取得
			 **********************************************/
			int itemLoopStart = (limitNum * pageIndexNum) - limitNum;
			if (itemLoopStart < 0)
			{
				itemLoopStart = 0;
			}

			List<SearchResultsDTO> searchResultsViewList = new List<SearchResultsDTO>();

			for (int i = 0; i < mapperList.Count; i++)
			{
				if (itemLoopStart <= i && i < (itemLoopStart + limitNum))
				{
					searchResultsViewList.Add(mapperList[i]);
				}
			}

			return searchResultsViewList;

		}
		/// <summary>
		/// 検索結果を画面表示件数で割り、最終ページ数を取得する
		/// </summary>
		/// <param name="searchResultsDt">検索結果datatable</param>
		/// <param name="limitNum">表示件数</param>
		/// <returns>最終ページ数</returns>
		private static int PageCountCalculator(DataTable searchResultsDt, int limitNum)
		{
			//検索結果のレコード件数を画面表示件数で割る
			int rowsCount = searchResultsDt.Rows.Count;
			int pageCount = rowsCount / limitNum;

			//上記除算にて余りが発生する場合、1プラスする
			int remainder = rowsCount % limitNum;
			if (remainder > 0)
			{
				pageCount += 1;
			}

			pageCount += 1;

			return pageCount;
		}
		/// <summary>
		/// 最大ページリンク数より最終ページ数が小さい場合、
		/// 最終ページ数で上書く
		/// </summary>
		/// <param name="maxPageNum">最大ページリンク数</param>
		/// <param name="pageLastNum">最終ページ数</param>
		/// <returns>最大ページリンク数</returns>
		private static int PreMaxPageNumMaker(int maxPageNum, int pageLastNum)
		{
			if (maxPageNum > pageLastNum)
			{
				maxPageNum = pageLastNum;
			}

			return maxPageNum;

		}
		/// <summary>
		/// 最小ページリンク数を設定する
		/// </summary>
		/// <param name="pageIndexNum">インデックスページ数</param>
		/// <param name="maxPageNumHalf">最大ページリンク数の半分の数</param>
		/// <param name="pageLastNum">最終ページ数</param>
		/// <param name="maxPageNum">最大ページリンク数</param>
		/// <returns>最小ページリンク数</returns>
		private static int MinPageNumMaker(int pageIndexNum, int maxPageNumHalf, int pageLastNum, int maxPageNum)
		{
			int minPageNum = pageIndexNum;
			//現在ページ数が最大ページリンク数の半分以下の場合、1を設定
			//そうでない場合、現在ページ数から最大ページリンク数の半分の数を引く
			if (minPageNum <= maxPageNumHalf)
			{
				minPageNum = 1;
			}
			else
			{
				minPageNum -= maxPageNumHalf;
			}

			//最大ページリンク数が最終ページ数以上の場合、1を設定
			if (pageLastNum <= maxPageNum)
			{
				minPageNum = 1;
			}
			//最終ページ数より、
			//最大ページリンク数と最小ページリンク数の合算値のほうが大きい場合
			else if (pageLastNum < (minPageNum + maxPageNum))
			{
				//最終ページ数から最大ページリンク数を引いた値を設定
				minPageNum = pageLastNum - maxPageNum;
			}

			return minPageNum;
		}

		/// <summary>
		/// 最大ページリンク数を設定する
		/// </summary>
		/// <param name="minPageNum">最小ページリンク数</param>
		/// <param name="maxPageNum">最大ページリンク数</param>
		/// <param name="pageLastNum">最終ページ数</param>
		/// <returns>最大ページリンク数</returns>
		private static int MaxPageNumMaker(int minPageNum, int maxPageNum, int pageLastNum)
		{
			int returnNum;
			if ((maxPageNum + minPageNum) < pageLastNum)
			{
				returnNum = maxPageNum + minPageNum;
			}else
			{
				returnNum = pageLastNum;
			}

			return returnNum;
		}

    }
}
