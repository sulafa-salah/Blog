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
        //public BaseResponse(bool success, int statusCode, string responseMessage, List<ValidationError> validationErrors = null)
        //{
        //    IsSuccess = success;
        //    ResponseCode = statusCode;
        //    ResponseMessage = responseMessage;
        //    ValidationErrors = validationErrors;
        //}

    }
    //public class ValidationError
    //{
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //}
}

