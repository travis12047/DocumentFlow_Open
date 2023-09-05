using DocumentFlow.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentFlow.Models.ViewModels;

namespace DocumentFlow.Controllers
{
    /// <summary>
    /// ドキュメント検索画面のコントローラー
    /// </summary>
    public class DocSearchController : Controller
    {
        //public ActionResult Search()
        //{
        //    DocSearchViewModel docSearchViewModel = new DocSearchViewModel();
        //    docSearchViewModel = DocSearchModel.CreateViewModel(docSearchViewModel, null, null, null, null, null, null);
        //    ViewData.Model = docSearchViewModel;
        //    ViewBag.limitNumList = docSearchViewModel.limitNumList;
        //    return View();
        //}
        /// <summary>
        /// 検索画面の表示
        /// </summary>
        /// <param name="limitNumList">表示件数プルダウンの値</param>
        /// <param name="searchCondApprovalStatusList">承認状況ラジオボタンの値</param>
        /// <param name="pageNum">画面で表示中のページ番号</param>
        /// <param name="limitNum">表示件数</param>
        /// <param name="searchCondApprovalStatus">承認状況</param>
        /// <param name="searchCondTitle">タイトルTextBoxの内容</param>
        /// <returns>検索画面</returns>
		//[HttpPost]
        public ActionResult Search(
		//初期表示、検索処理実行時に使用される引数
		string limitNumList, string searchCondApprovalStatusList,
		//ページリンク押下時に使用される引数
		string pageNum, string limitNum, string searchCondApprovalStatus,
		//全ての処理で使用される引数
		string searchCondTitle)
		{
			DocSearchViewModel docSearchViewModel = new DocSearchViewModel();

			//セッションに記録されているユーザID
			string userId = HttpContext.User.Claims.First().Value;

			docSearchViewModel = DocSearchModel.CreateViewModel(docSearchViewModel, userId, limitNumList, searchCondApprovalStatusList, pageNum, limitNum, searchCondApprovalStatus, searchCondTitle);
			ViewData.Model = docSearchViewModel;
			ViewBag.limitNumList = docSearchViewModel.limitNumList;
			return View();
		}
	}
}
