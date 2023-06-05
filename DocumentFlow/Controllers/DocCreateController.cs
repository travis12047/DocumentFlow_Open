using DocumentFlow.Models;
using DocumentFlow.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlow.Controllers
{
	/// <summary>
	/// ドキュメント作成・閲覧画面のコントローラー
	/// </summary>
	public class DocCreateController : Controller
	{
		// GET: DocCreateController
		public ActionResult Index()
		{
			return View();
		}

		// GET: DocCreateController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: DocCreateController/Create
		public async Task<IActionResult> Create()
		{
			DocCreateViewModel docCreateViewModel = new DocCreateViewModel();
			docCreateViewModel = DocCreateModel.CreateViewModel(docCreateViewModel);
			ViewData.Model = docCreateViewModel;
			return View();
		}

		// POST: DocCreateController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		//public ActionResult Create(IFormCollection collection, string title, string content)
		public ActionResult Create(IFormCollection collection, string docTitle, string docContent, string selectFlow)
		//public ActionResult Create(IFormCollection collection)
		{
			DocCreateViewModel docCreateViewModel = new DocCreateViewModel();
			try
			{
				//セッションに記録されているユーザID
				string userId = HttpContext.User.Claims.First().Value;
				DocCreateModel.DoCreate(docTitle, docContent, userId, selectFlow);
				//return RedirectToAction(nameof(Index));
			}
			catch
			{
			}
			docCreateViewModel = DocCreateModel.CreateViewModel(docCreateViewModel);
			ViewData.Model = docCreateViewModel;
			return View();
		}

		// GET: DocCreateController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: DocCreateController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: DocCreateController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: DocCreateController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
