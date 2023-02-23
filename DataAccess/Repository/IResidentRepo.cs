using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IResidentRepo
    {
        public bool CheckLatePaymentAccountsByLateInvoices(List<Invoice> invoices);
        public Resident FindById(long id);
        public Resident FindByIdentityCardNumberToBookRoom(string identityCardNumber);
        public Resident FindByIdentityCardNumber(string identityCardNumber);
        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber);
    }
}
