namespace DocumentFlow.Models.DB.DTO
{
	public class M_UserDTO
	{
		public string	 m_user_id			{ get; set; }
		public string	 m_user_name		{ get; set; }
		public string	 m_user_pass		{ get; set; }
		public string	 m_user_mail		{ get; set; }
		public string m_user_delete_at	{ get; set; }
		public DateTime m_user_createddata	{ get; set; }
		public DateTime	 m_user_updatedata	{ get; set; }
	}
}
