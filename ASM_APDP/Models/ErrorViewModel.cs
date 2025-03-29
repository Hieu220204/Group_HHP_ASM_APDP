namespace ASM_APDP.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } = string.Empty;

        public bool ShowRequestId => !string.IsNullOrWhiteSpace(RequestId);

        public ErrorViewModel() { }

        public ErrorViewModel(string requestId)
        {
            RequestId = requestId;
        }
    }
}
