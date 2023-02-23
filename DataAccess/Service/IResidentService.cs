using BussinessObject.DTO;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IResidentService
    {
        public Resident FindById(long id);
        public Resident FindByIdentityCardNumberToBookRoom(string identityCardNumber);
        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber);
        public ResidentDTOForDetail ViewResidentDetail(string identityCardNumber);
    }
}
