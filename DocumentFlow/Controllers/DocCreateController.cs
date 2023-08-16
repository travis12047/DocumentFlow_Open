using DocumentFlow.Models;
using DocumentFlow.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlow.Controllers
{
	/// <summary>
	/// ドキュメント作成・閲覧画面のコントローラー
	/// </summary>
	public class DocCreateController : Controller
	{
		/// <summary>
		/// ドキュメント作成画面の初期表示
		/// </summary>
		/// <returns>ドキュメント作成画面</returns>
		public async Task<IActionResult> Create()
		{
			DocCreateViewModel docCreateViewModel = new DocCreateViewModel();
			docCreateViewModel = DocCreateModel.CreateViewModel(docCreateViewModel);
			ViewData.Model = docCreateViewModel;
			return View();
		}

		/// <summary>
		/// 作成済みドキュメント画面の表示
		/// </summary>
		/// <returns>作成済みドキュメント画面</returns>
		public async Task<IActionResult> ViewCreatedDoc(string documentId)
		{
			DocCreateViewModel docCreateViewModel = new DocCreateViewModel();
			docCreateViewModel = DocCreateModel.CreateViewModel(docCreateViewModel);
			docCreateViewModel = DocCreateModel.GetCreatedDoc(docCreateViewModel, documentId);
			ViewData.Model = docCreateViewModel;
			return View("Create");
		}

		/// <summary>
		/// ドキュメント作成の実施
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="docTitle">タイトル</param>
		/// <param name="docContent">内容</param>
		/// <param name="selectFlow">選択されたフロー</param>
		/// <returns>ドキュメント作成画面</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection, string docTitle, string docContent, string selectFlow)
		{
			DocCreateViewModel docCreateViewModel = new DocCreateViewModel();
			try
			{
				//セッションに記録されているユーザID
				string userId = HttpContext.User.Claims.First().Value;
				DocCreateModel.DoCreate(docTitle, docContent, userId, selectFlow);
			}
			catch
			{
			}
			//ドキュメント作成画面の表示
			docCreateViewModel = DocCreateModel.CreateViewModel(docCreateViewModel);
			ViewData.Model = docCreateViewModel;
			return View();
		}
	}
}
