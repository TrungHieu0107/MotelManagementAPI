using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepo _residentRepo;
        private readonly IHistoryRepo _historyRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly IMotelChainRepo _motelChainRepo;

        public ResidentService(IResidentRepo residentRepo, IHistoryRepo historyRepo, IRoomRepo roomRepo)
        {
            _residentRepo = residentRepo;
            _historyRepo = historyRepo;
            _roomRepo = roomRepo;
        }

        public Resident FindById(long id)
        {
            return _residentRepo.FindById(id);
        }

        public Resident FindByIdentityCardNumberToBookRoom(string identityCardNumber)
        {
            return _residentRepo.FindByIdentityCardNumberToBookRoom(identityCardNumber);
        }

        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber)
        {
            return _residentRepo.UpdateStatusWhenBookingByIdentityCardNumber(identityCardNumber);
        }

        public ResidentDTOForDetail ViewResidentDetail(string identityCardNumber)
        {
            Resident resident = _residentRepo.FindByIdentityCardNumber(identityCardNumber);
            ResidentDTOForDetail residentDTOForDetail = new ResidentDTOForDetail();
            residentDTOForDetail.IdentityCardNumber = resident.IdentityCardNumber;
            residentDTOForDetail.Phone = resident.Phone;
            residentDTOForDetail.FullName = resident.FullName;
            residentDTOForDetail.Status = nameof(resident.Status);
            List<History> histories = _historyRepo.GetNullEndDateHistoriesByResident(resident);
            List<RoomDTOForResidentDetail> roomDTOForResidentDetails = new List<RoomDTOForResidentDetail>();
            foreach(History history in histories)
            {
                RoomDTOForResidentDetail roomDTOForResidentDetail = new RoomDTOForResidentDetail();
                roomDTOForResidentDetail.Code = history.Room.Code;
                roomDTOForResidentDetail.Status = nameof(history.Room.Status);
                roomDTOForResidentDetail.StartDate = history.StartDate.ToString("dd/MM/yyyy");
                roomDTOForResidentDetail.MotelChainName = _motelChainRepo.FindById(history.Room.MotelId).Name;
                roomDTOForResidentDetails.Add(roomDTOForResidentDetail);
            }
            residentDTOForDetail.RoomDTOForResidentDetails = roomDTOForResidentDetails;
            return residentDTOForDetail;
        }
    }
}
