using System;
using Microsoft.EntityFrameworkCore;
using FreshFarmMarket_211283E.DataContext;
using FreshFarmMarket_211283E.Models;


namespace FreshFarmMarket_211283E.Services
{
	public class LogServices
	{
		private readonly AuthDbContext _context;
		public LogServices(AuthDbContext dataContext)
		{
			_context = dataContext;
		}
		public async Task<List<Log>> RetrieveAllLogs()
		{
			return await _context.Logs.ToListAsync();
		}
		public async Task RecordLogs(string action, string email)
		{
			if (action == "Login")
			{
				await _context.Logs.AddAsync(new Log()
				{
					Action = Actions.Login,
					Description = string.Format("User: {0} has logged in!", email),
					User = email,
				});
			}

			else if (action == "Logout")
			{
				await _context.Logs.AddAsync(new Log()
				{
					Action = Actions.Logout,
					Description = string.Format("User: {0} has logged out!", email),
					User = email,
				});
			}

			await _context.SaveChangesAsync();
		}
	}
}
