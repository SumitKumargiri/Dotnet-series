namespace JobSeekingApplication.Model
{
    public class ResultModel<T>
    {
        public bool Success { get; set; }
        public int MsgCode { get; set; }
        public string Message { get; set; }
        public string Token {  get; set; }
        public int UserId {  get; set; }
        public T Data { get; set; }
        public ResultModel()
        {
            Success = true;
            Message = "Success";
            MsgCode = 1;
        }
    }
}
