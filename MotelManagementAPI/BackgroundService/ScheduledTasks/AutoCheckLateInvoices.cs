using DataAccess.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MotelManagementAPI.BackgroundService.ScheduledTasks
{
    public class AutoCheckLateInvoices : ScheduledProcessor
    {
        private IInvoiceService _invoiceService;

        public AutoCheckLateInvoices(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "0   0   20   *   * ";

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            DateTime current = DateTime.Now;
            DateTime dateTime = new DateTime(current.Year, current.Month, current.Day, 0, 0, 0);
            try
            {
                _invoiceService = serviceProvider.GetService<IInvoiceService>();
                _invoiceService.AutoCheckLateInvoices(dateTime);
                Console.WriteLine("AutoCheckLateInvoices fired at: " + current);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occured at AutoCheckLateInvoices fired at: " + current + " with message: " + ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
