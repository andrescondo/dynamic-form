using System;
namespace backend.Models.Request
{
	public class EditTableRequest
	{
		public int Id { get; set; }
        public List<InputsEdit>? inputs { get; set; }
        
    }

    public class InputsEdit
    {
        public int? ID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? IDForm { get; set; }
    }
}

