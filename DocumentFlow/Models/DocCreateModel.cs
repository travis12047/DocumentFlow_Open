using AutoMapper;
using DocumentFlow.Models.CommonModels;
using DocumentFlow.Models.DB.DAO;
using DocumentFlow.Models.DB.DTO;
using DocumentFlow.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Data;

namespace DocumentFlow.Models
{
	/// <summary>
	/// ドキュメント作成・閲覧画面のモデル
	/// </summary>
	public class DocCreateModel
	{
		/// <summary>
		/// 画面の情報を基にドキュメントを作成する
		/// </summary>
		/// <param name="docTitle">タイトル</param>
		/// <param name="docContent">内容</param>
		/// <param name="userId">ユーザID</param>
		/// <param name="selectFlow">選択されたフロー</param>
		public static void DoCreate(string docTitle, string docContent, string userId, string selectFlow)
        {
            //画面の情報を基にドキュメントマスタへ登録
            DocCreateDAO.DocCreate(docTitle, docContent, userId, selectFlow);
            //登録したデータのドキュメントIDを取得
            string createDocumentId = DocCreateDAO.GetCreateDocumentId(userId);
            //取得したドキュメントIDとフローIDを基に
            //承認管理マスタへデータを登録
            DocCreateDAO.ApprovalManagemenCreate(createDocumentId, selectFlow);

        }

		/// <summary>
		/// 画面表示に必要なViewModelの作成
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <returns>項目設定済みのviewModel</returns>
		public static DocCreateViewModel CreateViewModel(DocCreateViewModel viewModel)
		{
			//承認フロー選択に関する画面項目の作成
			DataTable approvalFlowDt = DocCreateDAO.GetApprovalFlow();
			viewModel.selectListItems = CreateSelectBox(approvalFlowDt);
			viewModel.approvalUserNameCombo = CreateApprovalUserNameCombo(approvalFlowDt);
			viewModel.approvalFlowCombo = CreateApprovalFlowComb(approvalFlowDt);
			//viewModel = CreateApprovalFlowComb(approvalFlowDt);

			return viewModel;
		}

		/// <summary>
		/// ドキュメントIDに紐づく文書情報を取得・設定する
		/// </summary>
		/// <param name="viewModel">viewModel</param>
		/// <param name="documentId">ドキュメントID</param>
		/// <returns>文書情報をセット済みのviewModel</returns>
		public static DocCreateViewModel GetCreatedDoc(DocCreateViewModel viewModel, string documentId)
		{
			if (!string.IsNullOrEmpty(documentId))
			{
				DataTable documentDataDt = DocCreateDAO.GetCreatedDoc(documentId);
				List<ArrayList> documentDataList = CommonModel.DataTableToListType(documentDataDt);

				IMapper mapper = CommonModel.CreateMapper();
				var mapperList = mapper.Map<List<ArrayList>, List<CreatedDocDTO>>(documentDataList);

				//文書情報を設定してください
				viewModel.docTitle = mapperList[0].m_document_title;
				viewModel.docContent = mapperList[0].m_document_content;
				//作成済みドキュメントフラグをアクティブにする
				viewModel.createdFlg = false;
			}

			return viewModel;
		}

		/// <summary>
		/// フローを選択するSelectBoxを作成
		/// </summary>
		/// <param name="approvalFlowDt">承認フロー情報が入っているdatatable</param>
		/// <returns>SelectBoxの表示内容</returns>
		public static List<SelectListItem> CreateSelectBox(DataTable approvalFlowDt)
		{

			//受け取ったdatatableからselectBoxの作成に邪魔なカラムを除去
			DataView dtview = dtview = new DataView(approvalFlowDt);
			DataTable flowIdNameDt = dtview.ToTable(false, "m_approval_flow_id", "m_approval_flow_name");

			//detatable内の重複するレコード情報をマージ
			flowIdNameDt = flowIdNameDt.DefaultView.ToTable(true, "m_approval_flow_id", "m_approval_flow_name");

			//datatableをListに変形
			List<ArrayList> flowIdNameList = CommonModel.DataTableToListType(flowIdNameDt);

			List<SelectListItem> selectListItems = new List<SelectListItem>();

			//一つ目の項目に空のアイテムを登録する
			SelectListItem blank = new SelectListItem();
			blank.Value = "";
			blank.Text = "";
			selectListItems.Add(blank);

			//List内の情報をSelectBoxの表示に使う変数へ追加
			for (int i = 0; i < flowIdNameList.Count; i++)
			{
				SelectListItem newItem = new SelectListItem();

				newItem.Value = flowIdNameList[i][0].ToString();
				newItem.Text = flowIdNameList[i][1].ToString();
				
				selectListItems.Add(newItem);
			}

			return selectListItems;

		}

        /// <summary>
        /// 承認フローのユーザ名をフローID単位でList化
        /// </summary>
        /// <param name="approvalFlowDt">承認フロー情報が入っているdatatable</param>
        /// <returns>フローID単位でList化されたユーザ名</returns>
        public static List<List<string>> CreateApprovalUserNameCombo(DataTable approvalFlowDt)
        {
            /***********************************************
			 *引数のdatatableから、
			 *フローIDのみを抽出しグループ化された
			 *datatableを作成する
			 **********************************************/
            //フローID以外の邪魔なカラムを除去
            DataView dtview = dtview = new DataView(approvalFlowDt);
            DataTable flowIdDt = dtview.ToTable(false, "m_approval_flow_id");

            //重複するレコード情報をマージ
            flowIdDt = flowIdDt.DefaultView.ToTable(true, "m_approval_flow_id");


            /***********************************************
			 *引数のdatatableから、
			 *DTOクラスの形にマッピングされたListを作成
			 **********************************************/
            //datatableをListに変形
            List<ArrayList> approvalFlowList = CommonModel.DataTableToListType(approvalFlowDt);

			//Listを指定のDTOクラスの形にマッピング
			IMapper mapper = CommonModel.CreateMapper();
			var mapperList = mapper.Map<List<ArrayList>, List<ApprovalFlowDTO>>(approvalFlowList);


            /***********************************************************
			 *フローIDが入ったdatatableと
			 *マッピング済みのListを使って
			 *フローID単位で詳細情報(ユーザ単位)が格納されたListを作成
			 **********************************************************/
            //フローID単位のList
            List<List<string>> combo = new List<List<string>>();
            //詳細情報(ユーザ単位)のList
            List<string> detail = new List<string>();

            //グループ化済みフローID格納datatableの行番号
            int idRow = 0;
            for (int i = 0; i < mapperList.Count; i++)
            {
                //マッピング済みList内のフローIDと、
                //グループ化済みフローID格納datatable内のフローIDを比較
                if (!int.Parse(mapperList[i].m_approval_flow_id).Equals(flowIdDt.Rows[idRow][0]))
                {
                    //詳細情報が格納されたListをフローID単位のListへ追加
                    combo.Add(detail);
                    //現在のフローIDの詳細情報が入っているListを初期化
                    detail = new List<string>();

                    idRow++;
                }
                //詳細情報Listへユーザ名を追加
                detail.Add(mapperList[i].m_user_name);
            }
            //最後のフローID単位の詳細情報Listをここで追加
            combo.Add(detail);

            return combo;
        }


        /// <summary>
        /// 承認フローの詳細情報(ユーザ単位の情報)をフローID単位でList化
        /// </summary>
        /// <param name="approvalFlowDt">承認フロー情報が入っているdatatable</param>
        /// <returns>フローID単位でList化された承認フロー情報</returns>
        public static List<List<ApprovalFlowDTO>> CreateApprovalFlowComb(DataTable approvalFlowDt)
        {
            /***********************************************
			 *引数のdatatableから、
			 *フローIDのみを抽出しグループ化された
			 *datatableを作成する
			 **********************************************/
            //フローID以外の邪魔なカラムを除去
            DataView dtview = dtview = new DataView(approvalFlowDt);
            DataTable flowIdDt = dtview.ToTable(false, "m_approval_flow_id");

            //重複するレコード情報をマージ
            flowIdDt = flowIdDt.DefaultView.ToTable(true, "m_approval_flow_id");


            /***********************************************
			 *引数のdatatableから、
			 *DTOクラスの形にマッピングされたListを作成
			 **********************************************/
            //datatableをListに変形
            List<ArrayList> approvalFlowList = CommonModel.DataTableToListType(approvalFlowDt);

			//Listを指定のDTOクラスの形にマッピング
			IMapper mapper = CommonModel.CreateMapper();
			var mapperList = mapper.Map<List<ArrayList>, List<ApprovalFlowDTO>>(approvalFlowList);


            /***********************************************************
			 *フローIDが入ったdatatableと
			 *マッピング済みのListを使って
			 *フローID単位で詳細情報(ユーザ単位)が格納されたListを作成
			 **********************************************************/
            //フローID単位のList
            List<List<ApprovalFlowDTO>> combo = new List<List<ApprovalFlowDTO>>();
            //詳細情報(ユーザ単位)のList
            List<ApprovalFlowDTO> detail = new List<ApprovalFlowDTO>();

            //グループ化済みフローID格納datatableの行番号
            int idRow = 0;
            for (int i = 0; i < mapperList.Count; i++)
            {
                //マッピング済みList内のフローIDと、
                //グループ化済みフローID格納datatable内のフローIDを比較
                if (!int.Parse(mapperList[i].m_approval_flow_id).Equals(flowIdDt.Rows[idRow][0]))
                {
                    //詳細情報が格納されたListをフローID単位のListへ追加
                    combo.Add(detail);
                    //現在のフローIDの詳細情報が入っているListを初期化
                    detail = new List<ApprovalFlowDTO>();

                    idRow++;
				}
				//詳細情報Listへユーザ名を追加
				detail.Add(mapperList[i]);
            }
            //最後のフローID単位の詳細情報Listをここで追加
            combo.Add(detail);

            return combo;
        }
    }
}
