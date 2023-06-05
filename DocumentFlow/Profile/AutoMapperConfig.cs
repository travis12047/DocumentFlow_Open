namespace DocumentFlow.Profile
{
	using AutoMapper;
	using System.Collections;
	using DocumentFlow.Models.DB.DTO;

	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig() : base()
		{
			CreateMap<ArrayList, M_UserDTO>()
			.ForMember(dest => dest.m_user_id, opt => opt.MapFrom(s => s[0]))
			.ForMember(dest => dest.m_user_name, opt => opt.MapFrom(s => s[1]))
			.ForMember(dest => dest.m_user_pass, opt => opt.MapFrom(s => s[2]))
			.ForMember(dest => dest.m_user_mail, opt => opt.MapFrom(s => s[3]))
			.ForMember(dest => dest.m_user_delete_at, opt => opt.MapFrom(s => s == null ? String.Empty : s[4]))
			.ForMember(dest => dest.m_user_createddata, opt => opt.MapFrom(s => s[5]))
			.ForMember(dest => dest.m_user_updatedata, opt => opt.MapFrom(s => s[6]));

			CreateMap<ArrayList, ApprovalFlowDTO>()
			.ForMember(dest => dest.m_approval_flow_id, opt => opt.MapFrom(s => s == null ? String.Empty : s[0]))
			.ForMember(dest => dest.m_approval_flow_name, opt => opt.MapFrom(s => s == null ? String.Empty : s[1]))
			.ForMember(dest => dest.m_approval_flow_order, opt => opt.MapFrom(s => s == null ? String.Empty : s[2]))
			.ForMember(dest => dest.m_approval_flow_m_user_id, opt => opt.MapFrom(s => s == null ? String.Empty : s[3]))
			.ForMember(dest => dest.m_user_name, opt => opt.MapFrom(s => s == null ? String.Empty : s[4]));
		}
	}

}
