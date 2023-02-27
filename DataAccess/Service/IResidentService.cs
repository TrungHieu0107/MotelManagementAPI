using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IResidentService
    {
        bool CreatResidentAccount(AccountDTO accountDTO);
        ResidentDTO GetResidentByIdentityCardNumber(string idCard);
        bool DeActiveResident(string idCard);
        bool UpdateResidentAccount(long id, ResidentUpdateDTO account);
        bool ActiveResident(string idCard);
        public Resident FindById(long id);
        public Resident FindByIdentityCardNumberToBookRoom(string identityCardNumber);
        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber);
        public ResidentDTOForDetail ViewResidentDetail(string identityCardNumber);

        public IEnumerable<ResidentDTO> getAllResident(int pageSize, int currentPage);
        IEnumerable<ResidentDTO> FillterResident(string idCardNumber, string phone, string Fullname,int status, int pageSize, int currentPage);
        public CommonResponse FindByIdForDetail(long residentId, int pageSize, int currentPage, string roomStatus);
        public bool BookRoom(BookingRoomRequest bookingRoomRequest, long managerId);
    }
}
