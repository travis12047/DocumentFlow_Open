using DocumentFlow.Models.DB.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DocumentFlow.Models.ViewModel
{
	public class DocCreateViewModel
    {
        public List<SelectListItem> selectListItems { get; set; }
        public List<List<string>> approvalUserNameCombo { get; set; } = null;
        public List<List<ApprovalFlowDTO>> approvalFlowCombo { get; set; }
		public string selectFlow { get; set; } = string.Empty;
		public string docTitle { get; set; }
		public string docContent { get; set; } = string.Empty;
	}
}
