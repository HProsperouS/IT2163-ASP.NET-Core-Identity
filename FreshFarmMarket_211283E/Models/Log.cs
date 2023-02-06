using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket_211283E.Models
{
	public class Log
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string User { get; set; } = string.Empty;
		public Actions Action { get; set; } 
		public string Description { get; set; } = string.Empty;
		public DateTime CreateTime { get; set; } = DateTime.Now;
	}
	public enum Actions
	{
		Login,
		Logout
	}
}
