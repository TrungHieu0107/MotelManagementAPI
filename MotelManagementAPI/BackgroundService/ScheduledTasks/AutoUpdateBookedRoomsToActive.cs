using DataAccess.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MotelManagementAPI.BackgroundService.ScheduledTasks
{
    public class AutoUpdateBookedRoomsToActive : ScheduledProcessor
    {
        private IRoomService _roomService;
        public AutoUpdateBookedRoomsToActive(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "0   0   *   *   * ";

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            DateTime current = DateTime.Now;
            DateTime dateTime = new DateTime(current.Year, current.Month, current.Day, 0, 0, 0);
            try
            {
                _roomService = serviceProvider.GetService<IRoomService>();
                _roomService.AutoUpdateBookedRoomsToActive(dateTime);
                Console.WriteLine("AutoUpdateBookedRoomsToActive fired at: " + current);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occured at AutoUpdateBookedRoomsToActive fired at: " + current + " with message: " + ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
