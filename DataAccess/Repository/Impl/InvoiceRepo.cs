using BussinessObject.Data;
using BussinessObject.Models;
using BussinessObject.Status;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly Context _context;

        public InvoiceRepo(Context context)
        {
            _context = context;
        }

        public void Add(Invoice invoice)
        {
            _context.Add(invoice);
            _context.SaveChanges();
        }

        public List<Invoice> AutoCheckLateInvoices(DateTime dateTime)
        {
            List<Invoice> lateInvoices = GetLateInvoices(dateTime);

            foreach(Invoice invoice in lateInvoices)
            {
                CheckLateInvoice(invoice.Id);
            }
            return lateInvoices;
        }

        public List<Invoice> GetLateInvoices(DateTime dateTime)
        {
            return _context.Invoices.Where(i => i.ExpiredDate <= dateTime.AddMinutes(5) && i.Status == InvoiceStatus.NOT_PAID_YET).ToList();
        }

        public bool AutoCloseInvoices(DateTime dateTime)
        {
            List<Invoice> closedInvoices = GetInvoicesToClose(dateTime);
            foreach (Invoice invoice in closedInvoices)
            {
                UpdateClosedInvoice(invoice, dateTime);
            }
            return true;
        }

        private List<Invoice> GetInvoicesToClose(DateTime dateTime)
        {
            return _context.Invoices.
                Where(i =>
                i.EndDate == null &&
                i.StartDate <= dateTime.AddMinutes(5).AddMonths(-1)
                ).ToList();
        }

        public Invoice FindById(long id)
        {
            return _context.Invoices.Where(i => i.Id == id).FirstOrDefault();
        }

        public Invoice GetPreviousInvoiceByRoomId(long roomId)
        {
            return _context.Invoices
                .Where(i => i.RoomId == roomId && i.EndDate != null)
                .OrderBy(i => i.EndDate).LastOrDefault();
        }

        private Invoice CheckLateInvoice(long invoiceId)
        {
            Invoice invoice = FindById(invoiceId);
            invoice.Status = InvoiceStatus.LATE;
            var tracker = _context.Attach(invoice);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return invoice;
        }

        private Invoice UpdateClosedInvoice(Invoice invoice, DateTime endDate)
        {
            invoice.ElectricityConsumptionEnd = invoice.ElectricityConsumptionStart + 1;
            invoice.WaterConsumptionEnd = invoice.WaterConsumptionStart + 1;
            invoice.EndDate = endDate;
            invoice.ExpiredDate = endDate.AddDays(FixedData.DATE_TO_PAY);
            var tracker = _context.Attach(invoice);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return invoice;
        }
    }
}
