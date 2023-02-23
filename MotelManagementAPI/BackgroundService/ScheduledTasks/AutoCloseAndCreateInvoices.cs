using DataAccess.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MotelManagementAPI.BackgroundService.ScheduledTasks
{
    public class AutoCloseAndCreateInvoices : ScheduledProcessor
    {
        private IInvoiceService _invoiceService;
        public AutoCloseAndCreateInvoices(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "0   0   5   *   * ";

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            DateTime current = DateTime.Now;
            DateTime dateTime = new DateTime(current.Year, current.Month, current.Day, 0, 0, 0);
            try
            {
                _invoiceService = serviceProvider.GetService<IInvoiceService>();
                _invoiceService.AutoCloseInvoices(dateTime);
                _invoiceService.AutoCreateInvoices(dateTime);
                Console.WriteLine("AutoCloseAndCreateInvoices fired at: " + current);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occured at AutoCloseAndCreateInvoices fired at: " + current + " with message: " + ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
