namespace Dapper_pro.Models
{
    public class ChatMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
        public bool Read { get; set; }
        public string connectionId { get; set; }
    }

}
