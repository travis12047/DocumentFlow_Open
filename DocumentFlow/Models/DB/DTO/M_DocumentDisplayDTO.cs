namespace DocumentFlow.Models.DB.DTO
{
	/// <summary>
	/// ドキュメントマスタをベースに<br/>
	/// 画面表示用に関連マスタをLeft Joinした<br/>
    /// Select文の実行結果を受け取るためのDTOクラス
	/// </summary>
	public class M_DocumentDisplayDTO
    {
        public string document_id { get; set; }
        public string document_title { get; set; }
        public string create_user_name { get; set; }
        public string approval_status { get; set; }
        public string completion_at { get; set; }
        public string createddata { get; set; }
        public string updatedata { get; set; }
    }
}
