namespace Administrator.ViewModel
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
    public class ChatHistoryRequest
    {
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
    }
}
