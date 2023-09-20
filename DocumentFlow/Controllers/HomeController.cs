using DocumentFlow.Models;
using DocumentFlow.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlow.Controllers
{
	/// <summary>
	/// Home画面のコントローラー
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// Home画面の表示
		/// </summary>
		/// <param name="approvedUnreadPageIndexNum">承認済み未読情報画面のページ番号</param>
		/// <param name="requiringApprovalPageIndexNum">承認必要情報画面のページ番号</param>
		/// <returns></returns>
		public IActionResult Home(string approvedUnreadPageIndexNum, string requiringApprovalPageIndexNum)
		{
			HomeViewModel homeViewModel = new HomeViewModel();

			//セッションに記録されているユーザID
			string userId = HttpContext.User.Claims.First().Value;

			homeViewModel = HomeModel.CreateViewModel(homeViewModel, userId, approvedUnreadPageIndexNum, requiringApprovalPageIndexNum);
			ViewData.Model = homeViewModel;

			return View();
		}

	}
}