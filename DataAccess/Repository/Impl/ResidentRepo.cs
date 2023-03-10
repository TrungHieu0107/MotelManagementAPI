using BussinessObject.Data;
using BussinessObject.DTO;
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
            return resident;
        }

        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber)
        {
            Resident resident = _context.Residents.FirstOrDefault(r => r.IdentityCardNumber == identityCardNumber);
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

        public bool UpdateStatusOfResident(long residentId, AccountStatus status)
        {
            var resident = _context.Residents.FirstOrDefault(r => r.Id == residentId);

            if (resident == null)
                return false;

            resident.Status = status;
            _context.Entry(resident).State = EntityState.Modified;
           if (_context.SaveChanges() != 1)
           {
                return false;
           }

            return true;

        }

        public IEnumerable<ResidentDTO> GetAllResident(int pageSize, int currentPage)
        {

            return _context.Residents.ToList().Select(
                 r => new ResidentDTO()
                 {

                     IdentityCardNumber = r.IdentityCardNumber,
                     FullName = r.FullName,
                     Id = r.Id,
                     Phone = r.Phone,
                     Status = Enum.GetName(typeof(AccountStatus), r.Status).ToString(),
                     Password = "",
                     UserName = ""
                 }
              ).Skip((currentPage - 1) * pageSize)
                        .Take(pageSize);

        }

     

        public IEnumerable<ResidentDTO> FillterResident(string idCardNumber, string phone, string Fullname, int status ,int pageSize, int currentPage)
        {
            var query = _context.Residents.AsQueryable();
            if (!String.IsNullOrEmpty(idCardNumber))
            {
                query = query.Where(p => p.IdentityCardNumber.Contains(idCardNumber));

            }
            if (!String.IsNullOrEmpty(phone))
            {
                query = query.Where(p => p.Phone == phone);
            }
            if (!String.IsNullOrEmpty(Fullname))
            {
                query = query.Where(p => p.FullName.Contains(Fullname));
            }
            
            if (status >=0 && status <= Enum.GetNames(typeof(AccountStatus)).Length)
            {
                query = query.Where(p => p.Status == (AccountStatus)Enum.ToObject(typeof(AccountStatus), status));
            }
            var x = query.ToList();
            return query.ToList().Select(
                r => new ResidentDTO()
                {

                    IdentityCardNumber = r.IdentityCardNumber,
                    FullName = r.FullName,
                    Id = r.Id,
                    Phone = r.Phone,
                    Status = Enum.GetName(typeof(AccountStatus), r.Status).ToString(),
                    Password = "",
                    UserName = ""
                }
             ).Skip((currentPage - 1) * pageSize)
                       .Take(pageSize);


        }

        public Resident findByPhone(String phone)
        {
            return _context.Residents.FirstOrDefault(r => r.Phone == phone);
        }
     
    }
}
