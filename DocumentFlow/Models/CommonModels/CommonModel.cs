using AutoMapper;
using DocumentFlow.Profile;
using System.Collections;
using System.Data;

namespace DocumentFlow.Models.CommonModels
{
	/// <summary>
	/// 共通関数記載クラス
	/// </summary>
	public class CommonModel
	{
		/// <summary>
		/// DataTableをListに変形させる
		/// </summary>
		/// <param name="dT">DataTable</param>
		/// <returns>DataTableを変形させたList</returns>
		public static List<ArrayList> DataTableToListType(DataTable dT)
		{
			int rowCount = dT.Rows.Count;
			int columnCount = dT.Columns.Count;
			var resultList = new List<ArrayList>();

			for (int row = 0; row < rowCount; row++)
			{
				var rowList = new ArrayList();
				for (int column = 0; column < columnCount; column++)
				{
					rowList.Add(dT.Rows[row][column]);
				}
				resultList.Add(rowList);
			}

			return resultList;
		}

		/// <summary>
		/// AutoMapperを使ったマッピングを行うためのMapper作成
		/// </summary>
		/// <returns>Mapper</returns>
		public static IMapper CreateMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<AutoMapperConfig>();
			});
			IMapper mapper = config.CreateMapper();

			return mapper;

		}
	}
}
