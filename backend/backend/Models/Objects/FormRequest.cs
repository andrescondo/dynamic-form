using System;
namespace backend.Models.Objects
{
    public class FormRquest
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string CI { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
