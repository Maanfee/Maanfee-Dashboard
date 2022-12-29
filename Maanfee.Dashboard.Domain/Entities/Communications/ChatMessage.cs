using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Domain.Entities.Communications
{
	public class ChatMessage
	{
		[Key]
		public string Id { get; set; }

		// ******************************************************************

		public string IdFromUser { get; set; }

		[ForeignKey("IdFromUser")]
		public virtual ApplicationUser FromUser { get; set; }

		// ******************************************************************

		public string IdToUser { get; set; }

		[ForeignKey("IdToUser")]
		public virtual ApplicationUser ToUser { get; set; }

		// ******************************************************************

		public string Message { get; set; }

		public DateTime SendDate { get; set; }

		public DateTime ReadDate { get; set; }
	}
}
