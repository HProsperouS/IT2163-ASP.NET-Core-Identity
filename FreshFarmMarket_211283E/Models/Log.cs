using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket_211283E.Models
{
	public class Log
	{
		[Key]
		public string Id { get; set; } = string.Empty;
		public string User { get; set; } = string.Empty;
		public Actions Action { get; set; } 
		public string Description { get; set; } = string.Empty;
		public DateTime Timestamp { get; set; } = DateTime.Now;
	}
	public enum Actions
	{
		Login,
		Logout
	}
}
