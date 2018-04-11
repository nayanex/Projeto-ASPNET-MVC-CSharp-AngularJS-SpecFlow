using System;

namespace WexProject.BLL.Util
{
	public class Validation
	{
        public string Field { get; set; }
        public string Message { get; set; }
        public Validation(string Field, string Message)
        {
            this.Field = Field;
            this.Message = Message;
        }

	}
}
