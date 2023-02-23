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


    }
}
