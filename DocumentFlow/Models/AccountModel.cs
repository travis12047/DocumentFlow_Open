using AutoMapper;
using System.Collections;
using DocumentFlow.Profile;
using DocumentFlow.Models.DB.DTO;
using DocumentFlow.Models.DB.DAO;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlow.Models
{
	/// <summary>
	/// ログイン画面のモデル
	/// </summary>
	[AllowAnonymous]
	public class AccountModel
	{
		/// <summary>
		/// ユーザー名とパスワードの組み合わせが一意の場合、UserIDを返す
		/// </summary>
		/// <param name="name">ユーザーネーム</param>
		/// <param name="pass">パスワード</param>
		/// <returns>UserID</returns>
		public static String GetUserId(string name, string pass)
		{
			String userId = null;

			//ユーザ情報を取得
			List<ArrayList> dataTableToList = AccountDAO.GetSingleUser(name, pass);


			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<AutoMapperConfig>();
			});

			var mapper = config.CreateMapper();

			var mapperList = mapper.Map<List<ArrayList>, List<M_UserDTO>>(dataTableToList);

			//レコード件数が1件だった場合、UserIDをセット
			if (mapperList.Count == 1)
			{
				userId = mapperList[0].m_user_id;
			}


			return userId;
		}
	}

}
