using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataAccess.Repository
{
    public interface IResidentRepo
    {
        Resident CreatResidentAccount(Resident resident);
        IEnumerable<Resident> GetResidentByIdentityCardNumberAndStatusAndUserName(string idCard, string username, [Optional] AccountStatus a);
        Resident UpdateResidentAccount(Resident resident);
        Resident findById(long id);
        public bool CheckLatePaymentAccountsByLateInvoices(List<Invoice> invoices);
        public Resident FindById(long id);
        public Resident FindByIdentityCardNumberToBookRoom(string identityCardNumber);
        public Resident FindByIdentityCardNumber(string identityCardNumber);
        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber);
    }
}
