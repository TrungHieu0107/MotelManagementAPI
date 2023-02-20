using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IResidentRepo 
    {
        Resident CreatResidentAccount(Resident resident);


        IEnumerable<Resident> GetResidentByIdentityCardNumberAndStatusAndUserName(string idCard,  string username,[Optional] AccountStatus a);
       


       Resident UpdateResidentAccount(Resident resident);

        Resident findById(long id);
     
    }
}
