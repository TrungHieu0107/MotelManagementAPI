using BussinessObject.Models;
using BussinessObject.Status;
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

    }
}
