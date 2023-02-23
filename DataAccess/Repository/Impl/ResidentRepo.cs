using BussinessObject.Data;
using BussinessObject.Models;
using BussinessObject.Status;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DataAccess.Repository
{
    public class ResidentRepo : IResidentRepo
    {
        private readonly Context _context;
        public ResidentRepo(Context context)
        {
            this._context = context;
        }


        public Resident CreatResidentAccount(Resident resident)
        {
            
            _context.Residents.Add(resident);
            int check =  _context.SaveChanges();
            if(check > 0)
            {
                return resident;
            } else  throw new Exception("Can not update the resident");


        }

        public Resident findById(long id)
        {
            return _context.Residents.Find(id);
        }

        public IEnumerable<Resident> GetResidentByIdentityCardNumberAndStatusAndUserName(string idCard, string username, [Optional] AccountStatus a)
        {
            var query = _context.Residents.AsQueryable();
            if (a != AccountStatus.ACTIVE && a != AccountStatus.INACTIVE)
            {
                query = query.Where(p => p.Status == a);
            }
            if (!String.IsNullOrEmpty(idCard))
            {
                query = query.Where(p => p.IdentityCardNumber == idCard);
            }
            if (!String.IsNullOrEmpty(username))
            {
                query = query.Where(p => p.UserName == username);
            }
            return query.ToList();
        }

        public Resident UpdateResidentAccount(Resident resident)
        {
            _context.Entry(resident).State = EntityState.Modified;
            int check = _context.SaveChanges();
            if (check > 0)
            {
                return resident;
            }
            else throw new Exception("Can not update the resident");
        }


        public bool CheckLatePaymentAccountsByLateInvoices(List<Invoice> invoices)
        {
            foreach(Invoice invoice in invoices)
            {
                CheckLatePaymentAccount(invoice.ResidentId);
            }
            return true;
        }

        public Resident FindById(long id)
        {
            return _context.Residents.FirstOrDefault(x => x.Id == id);
        }

        public Resident FindByIdentityCardNumber(string identityCardNumber)
        {
            Resident resident = _context.Residents.FirstOrDefault(r => r.IdentityCardNumber == identityCardNumber);
            if (resident == null) throw new Exception("Resident with identity card number: " + identityCardNumber + " doesn't exist.");
            return resident;
        }

        public Resident FindByIdentityCardNumberToBookRoom(string identityCardNumber)
        {
            Resident resident = _context.Residents.FirstOrDefault(r => r.IdentityCardNumber == identityCardNumber);
            if (resident == null) throw new Exception("Resident with identity card number: " + identityCardNumber + " doesn't exist.");
            if (resident.Status != AccountStatus.LATE_PAYMENT) throw new Exception("This resident has an invoice that is not paid yet.");
            return resident;
        }

        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber)
        {
            Resident resident = FindByIdentityCardNumberToBookRoom(identityCardNumber);
            resident.Status = AccountStatus.ACTIVE;
            var tracker = _context.Attach(resident);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return resident;
        }

        private Resident CheckLatePaymentAccount(long id)
        {
            Resident resident = FindById(id);
            resident.Status = AccountStatus.LATE_PAYMENT;
            var tracker = _context.Attach(resident);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return resident;
        }
    }
}
