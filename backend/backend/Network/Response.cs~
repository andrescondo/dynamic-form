using System;
using backend.Models.Response;

namespace backend.Network
{
    public class Response
    {
        public ResponseGeneral ResponseSuccess(int status, List<dynamic> messages, List<dynamic> data, bool error)
        {
            ResponseGeneral response = new ResponseGeneral
            {
                Status = status,
                Messages = messages,
                Data = new List<dynamic>(data),
                Error = error
            };

            return response;
        }
    }
}

