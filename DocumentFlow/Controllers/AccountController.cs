using DocumentFlow.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DocumentFlow.Controllers
{
	/// <summary>
	/// ログイン画面のコントローラ
	/// </summary>
	[AllowAnonymous]
	public class AccountController : Controller
	{
		/// <summary>
		/// ログイン画面の表示
		/// </summary>
		/// <returns>ログイン画面</returns>
		public IActionResult Login()
		{
			return View();
		}

		/// <summary>
		/// ログイン処理の実施
		/// </summary>
		/// <param name="name">ユーザーネーム</param>
		/// <param name="pass">パスワード</param>
		/// <returns></returns>
		//public IActionResult LoginAction(string name, string pass)
		// POST: DocCreateController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(string name, string pass)
		{

			//ユーザーネームとパスワードの組み合わせ情報の確認
			String userId = AccountModel.GetUserId(name, pass);
			if (userId == null || userId.Equals(""))
			{
				//確認できない場合
				ViewData["name"] = name;
				ViewData["pass"] = pass;
				return View("LoginFail", "Account");
			}
			else
			{
				//return RedirectToAction("Index", "Home");
				//return RedirectToAction("Create", "DocCreate");
				//確認できた場合、nameとUserIDを私用してサインインを行う
				SignInExecute(name, userId);
				return RedirectToAction("Create", "DocCreate");
				//return RedirectToAction(nameof(DocCreateController.Create), "DocCreate");
			}
		}

		/// <summary>
		/// Authorizeが有効になっているエリア(画面等)へのアクセスを可能にするため、
		/// ユーザ情報を使ってサインインを行う
		/// </summary>
		/// <param name="name"></param>
		/// <param name="userId"></param>
		public async void SignInExecute(string name, string userId)
		{

			// ★以下ログイン処理
			// 名前、電子メール アドレス、年齢、Sales ロールのメンバーシップなど、id 情報の一部
			Claim[] claims = {
				new Claim(ClaimTypes.NameIdentifier, userId), // ユニークID
				new Claim(ClaimTypes.Name, name)
			  };

			// 一意の ID 情報
			var claimsIdentity = new ClaimsIdentity(
			  claims, CookieAuthenticationDefaults.AuthenticationScheme);

			// ログイン
			await HttpContext.SignInAsync(
			  CookieAuthenticationDefaults.AuthenticationScheme,
			  new ClaimsPrincipal(claimsIdentity),
			  new AuthenticationProperties
			  {
				  AllowRefresh = true,
				  ExpiresUtc = DateTimeOffset.Now.AddDays(1),
				  // Cookie をブラウザー セッション間で永続化するか？（ブラウザを閉じてもログアウトしないかどうか）
				  IsPersistent = true
			  });
		}

		/// <summary>
		/// ログイン失敗画面の表示
		/// </summary>
		/// <returns>ログイン失敗画面</returns>
		public IActionResult LoginFail()
		{
			return View();
		}
	}
}
