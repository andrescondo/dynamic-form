using System;
namespace backend.Models.Response
{
    public class ResponseGeneral
    {
        public required int Status { get; set; }
        public required List<dynamic> Messages { get; set; }
        public required List<dynamic> Data { get; set; }
        public required bool Error { get; set; }
    }

    public class Message
    {
        public string Text { get; set; }
        public bool Error { get; set; }

    }

}