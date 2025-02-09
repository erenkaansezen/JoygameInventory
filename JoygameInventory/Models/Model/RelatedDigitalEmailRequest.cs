namespace JoygameInventory.Models.Model
{
    public class RelatedDigitalEmailRequest
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ReplyAddress { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string Charset { get; set; }
        public string ToName { get; set; }
        public string ToEmailAddress { get; set; }
        public string PostType { get; set; }
        public string KeyId { get; set; }
        public string CustomParams { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public string Name { get; set; }  =string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class RelatedDigitalEmailResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string DetailedMessage { get; set; }
        public string PostId { get; set; }
    }


}
