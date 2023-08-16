namespace DocumentFlow.Models.DB.DTO
{
    public class SearchResultsDTO
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
