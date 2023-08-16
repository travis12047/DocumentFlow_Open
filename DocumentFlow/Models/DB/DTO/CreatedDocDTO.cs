namespace DocumentFlow.Models.DB.DTO
{
    public class CreatedDocDTO
	{
		public string m_document_id { get; set; }
		public string m_document_title { get; set; }
		public string m_document_content { get; set; }
		public string m_document_create_user_id { get; set; }
		public string m_document_approvalflow_id { get; set; }
		public string m_document_approvalflow_current_order { get; set; }
		public string m_document_completion_at { get; set; }
		public string m_document_management_deleted_at { get; set; }
		public string m_document_createddata { get; set; }
		public string m_document_updatedata { get; set; }
	}
}
