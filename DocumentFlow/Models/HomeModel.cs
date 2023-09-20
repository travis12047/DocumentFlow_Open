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
    /// ホーム画面のモデル
    /// </summary>
    public class HomeModel
	{
		/// <summary>
		/// 画面表示に必要なViewModelの作成
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <param name="approvedUnreadPageIndexNum">承認済み未読情報画面のページ番号</param>
		/// <param name="requiringApprovalPageIndexNum">承認必要情報画面のページ番号</param>
		/// <returns>項目設定済みのviewModel</returns>
		public static HomeViewModel CreateViewModel
		(HomeViewModel viewModel, string userId, string approvedUnreadPageIndexNum, string requiringApprovalPageIndexNum)
		{
			viewModel = CreateApprovedUnreadView(viewModel, userId, approvedUnreadPageIndexNum);
			viewModel = CreateRequiringApprovalView(viewModel, userId, requiringApprovalPageIndexNum);

			return viewModel;
		}

		/// <summary>
		/// 承認済み未読情報に関するViewModelを作成する
		/// </summary>
		/// <param name="viewModel">ViewModel</param>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <param name="approvedUnreadPageIndexNum">承認済み未読情報画面のページ番号</param>
		/// <returns>項目が設定されたViewModel</returns>
		private static HomeViewModel CreateApprovedUnreadView(HomeViewModel viewModel, string userId, string approvedUnreadPageIndexNum)
		{
			//ページ番号を設定
			if (!string.IsNullOrEmpty(approvedUnreadPageIndexNum))
			{
				viewModel.approvedUnreadPagingItem.pageIndexNum = int.Parse(approvedUnreadPageIndexNum);
			}

			DataTable approvedUnreadDocDt = new DataTable();
			approvedUnreadDocDt = HomeDAO.GetApprovedUnreadDoc(userId);

			//ページングアイテムの設定
			viewModel.approvedUnreadPagingItem = PagingModel.CreatePageLync(viewModel.approvedUnreadPagingItem, approvedUnreadDocDt);
			//取得した情報をページング画面用に加工・設定
			approvedUnreadDocDt = PagingModel.CreateDisplayDataTable(approvedUnreadDocDt, (int)viewModel.approvedUnreadPagingItem.limitNum, (int)viewModel.approvedUnreadPagingItem.pageIndexNum);
			viewModel.approvedUnreadList = CreateDisplayList(approvedUnreadDocDt);

			return viewModel;
		}

		/// <summary>
		/// 承認が必要な情報に関するViewModelを作成する
		/// </summary>
		/// <param name="viewModel">ViewModel</param>
		/// <param name="userId">セッションに記録されているユーザID</param>
		/// <param name="requiringApprovalPageIndexNum">承認必要情報画面のページ番号</param>
		/// <returns>項目が設定されたViewModel</returns>
		private static HomeViewModel CreateRequiringApprovalView(HomeViewModel viewModel, string userId, string requiringApprovalPageIndexNum)
		{
			//ページ番号を設定
			if (!string.IsNullOrEmpty(requiringApprovalPageIndexNum))
			{
				viewModel.requiringApprovalPagingItem.pageIndexNum = int.Parse(requiringApprovalPageIndexNum);
			}
			DataTable requiringApprovalDt = new DataTable();
			requiringApprovalDt = HomeDAO.GetRequiringApprovalDoc(userId);

			//ページングアイテムの設定
			viewModel.requiringApprovalPagingItem = PagingModel.CreatePageLync(viewModel.requiringApprovalPagingItem, requiringApprovalDt);
			//取得した情報をページング画面用に加工・設定
			requiringApprovalDt = PagingModel.CreateDisplayDataTable(requiringApprovalDt, (int)viewModel.requiringApprovalPagingItem.limitNum, (int)viewModel.requiringApprovalPagingItem.pageIndexNum);
			viewModel.requiringApprovalList = CreateDisplayList(requiringApprovalDt);

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
