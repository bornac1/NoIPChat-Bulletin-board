namespace MessageFormat
{
    public class Message
    {
        /// <summary>
        /// Title of the message.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Text content of the message.
        /// </summary>
        public string? TxtContent { get; set; }
        /// <summary>
        /// Binary content of the message.
        /// </summary>
        public byte[]? Content { get; set; }
    }
}