using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskList.Models
{
	public class TaskItem
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime? DateCreated { get; set; } = DateTime.Now;
		public DateTime? DateCompleted { get; set; }


		public string UserId { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
	}
}