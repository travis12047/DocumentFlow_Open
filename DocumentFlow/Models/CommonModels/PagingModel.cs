using DocumentFlow.Models.ViewModels;
using System.Data;

namespace DocumentFlow.Models.CommonModels
{
    /// <summary>
    /// 取得した一覧情報のページングを実施するためのModel
    /// </summary>
    public class PagingModel
    {
        /// <summary>
        /// 表示するページ番号の表示件数分だけ格納されたDataTableを返す
        /// </summary>
        /// <param name="originalDataTable">元のdataTable</param>
        /// <param name="limitNum">表示件数</param>
        /// <param name="pageIndexNum">インデックスページ番号</param>
        /// <returns>加工後のDataTable</returns>
        public static DataTable CreateDisplayDataTable
        (DataTable originalDataTable, int limitNum, int pageIndexNum)
        {
            /***********************************************
			 *表示するページ番号の件数分だけ取得
			 **********************************************/
            int itemLoopStart = limitNum * pageIndexNum - limitNum;
            if (itemLoopStart < 0)
            {
                itemLoopStart = 0;
            }

            DataTable displayDataTable = new DataTable();

            for (int ii = 0; ii < originalDataTable.Columns.Count; ii++)
            {
                displayDataTable.Columns.Add(originalDataTable.Columns[ii].ToString());
            }
            for (int i = 0; i < originalDataTable.Rows.Count; i++)
            {
                if (itemLoopStart <= i && i < itemLoopStart + limitNum)
                {
                    displayDataTable.Rows.Add(originalDataTable.Rows[i].ItemArray);
                }
            }

            return displayDataTable;

        }
        /// <summary>
        /// ページリンク系の作成
        /// </summary>
        /// <param name="viewModel">viewModel</param>
        /// <param name="searchResultsDt">ページ数算出に使用する検索結果</param>
        /// <returns>ページリンク設定後のviewModel</returns>
        public static PagingViewModel CreatePageLync(PagingViewModel viewModel, DataTable searchResultsDt)
        {
            viewModel.maxPageNumHalf = viewModel.maxPageNum / 2;
            viewModel.pageLastNum = PageCountCalculator(searchResultsDt, (int)viewModel.limitNum);
            int preMaxPageNum = PreMaxPageNumMaker((int)viewModel.maxPageNum, (int)viewModel.pageLastNum);
            viewModel.minPageNum = MinPageNumMaker((int)viewModel.pageIndexNum, (int)viewModel.maxPageNumHalf, (int)viewModel.pageLastNum, preMaxPageNum);
            viewModel.maxPageNum = MaxPageNumMaker((int)viewModel.minPageNum, (int)viewModel.maxPageNum, (int)viewModel.pageLastNum);

            return viewModel;
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
            else if (pageLastNum < minPageNum + maxPageNum)
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
            if (maxPageNum + minPageNum < pageLastNum)
            {
                returnNum = maxPageNum + minPageNum;
            }
            else
            {
                returnNum = pageLastNum;
            }

            return returnNum;
        }
    }
}
