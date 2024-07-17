namespace Blog.Contracts.Common.Response
{
    public record ResponseDto
    {
        public bool IsSuccess { get; set; } = false;
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
     

        public object? Data { get; set; }
        public ResponseDto()
        {

        }
        public ResponseDto(bool success, int statusCode, string responseMessage,object? data =null)
        {
            IsSuccess = success;
            ResponseCode = statusCode;
            ResponseMessage = responseMessage;
            Data=data;

        }
      

    }
    public record ErrorDetail
    {
        public string FieldName { get; init; }  
        public string Message { get; init; }    // Using init-only property
    }

}

