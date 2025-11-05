namespace NoteTaking.ViewModel
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        ///saves
       
        public string FieldName { get; set; }
        public string ErrorMessageInner { get; set; }
        public string ErrorMessage { get; set; }
    }
}
