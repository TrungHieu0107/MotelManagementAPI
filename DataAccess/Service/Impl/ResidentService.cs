using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using DataAccess.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Service.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepo _residentRepo;
        private readonly IHistoryRepo _historyRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly IMotelChainRepo _motelChainRepo;

        public ResidentService(IResidentRepo residentRepo, IHistoryRepo historyRepo, IRoomRepo roomRepo, IMotelChainRepo motelChainRepo, IAccountRepo accountRepo, IInvoiceRepo invoiceRepo)
        {
            _residentRepo = residentRepo;
            _historyRepo = historyRepo;
            _roomRepo = roomRepo;
            _motelChainRepo = motelChainRepo;
            _accountRepo = accountRepo;
            _invoiceRepo = invoiceRepo;
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

        private readonly IAccountRepo _accountRepo;
        private readonly IInvoiceRepo _invoiceRepo;
        public ResidentDTO GetResidentByIdentityCardNumber(string idCard)
        {
            var resident = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCard, null).FirstOrDefault<Resident>();
            var serialized = JsonConvert.SerializeObject(resident);
            var residentDTO = JsonConvert.DeserializeObject<ResidentDTO>(serialized);
            residentDTO.Password = "";
            residentDTO.UserName = "";
            return residentDTO;
        }

        public bool DeActiveResident(String idCard)
        {
            bool check = false;
            Resident resident = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCard, null, AccountStatus.ACTIVE).FirstOrDefault<Resident>(); ;
            if (resident != null)
            {
                resident.Status = AccountStatus.INACTIVE;
                _residentRepo.UpdateResidentAccount(resident);
                check = true;
            }
            return check;
        }

        public bool CreatResidentAccount(AccountDTO account)
        {
            bool check = false;
            Resident checkExistIdCardNumber = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(account.IdentityCardNumber, null).FirstOrDefault<Resident>();
            Account checkExistUserName = _accountRepo.FindAccountByUserName(account.UserName);
            if (checkExistIdCardNumber == null && checkExistUserName == null)
            {
                Resident resident = new Resident();
                resident.IdentityCardNumber = account.IdentityCardNumber;
                resident.Status = AccountStatus.ACTIVE;
                resident.UserName = account.UserName;
                resident.Password = PasswordHasher.Hash(account.Password);
                resident.FullName = account.FullName;
                resident.Phone = account.Phone;
                //Save change
                var newResident = _residentRepo.CreatResidentAccount(resident);
                if(newResident != null) {
                    check = true;
                }
               
            }else if(checkExistUserName != null)
            {
                throw new Exception("This username already exist");

            } else if(checkExistIdCardNumber != null) {
                throw new Exception("This resident already exist");
            }

            return check;
        }

        public bool UpdateResidentAccount(long id, ResidentUpdateDTO account)
        {
            bool check = false;
            //  check if id cardnumber is exsit
            Resident idCardCheck = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(account.IdentityCardNumber, null,AccountStatus.ACTIVE).FirstOrDefault<Resident>();
            if (idCardCheck != null && idCardCheck.Id != id)
            {
                return false;
            }
            else
            {
                // check if user exsit 
                Resident resident = _residentRepo.findById(id);

                // user exist , idCardnumber input is unique
                if (resident != null)
                {

                    resident.IdentityCardNumber = account.IdentityCardNumber;


                    resident.Password = PasswordHasher.Hash(account.Password);
                    resident.FullName = account.FullName;
                    resident.Phone = account.Phone;

                    //Save change
                    var updatedResident = _residentRepo.UpdateResidentAccount(resident);
                    if (updatedResident != null)
                    {
                        check = true;
                    }
                }
                else throw new Exception("Some thing went wrong");

            }
            return check;
        }


        private List<Invoice> checkLateInvoice(string idCard)
        {
            var invoices = _invoiceRepo.checkLateInvoice(idCard);
            return invoices;
        }

        public bool ActiveResident(string idCard)
        {
            bool check = false;

            // invoice không có trạng thái late, check xem có bị trễ invoice hay không
            List<Invoice> invoices = checkLateInvoice(idCard);

            if (invoices.Count == 0)
            {
                var resident = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCard, null, AccountStatus.INACTIVE).FirstOrDefault();
                if (resident != null)
                {
                    resident.Status = AccountStatus.ACTIVE;
                    _residentRepo.UpdateResidentAccount(resident);
                    check = true;
                }


            }
            return check;
        }

        public CommonResponse FindByIdForDetail(long residentId, int pageSize, int currentPage, string roomStatus)
        {
            Resident resident = FindById(residentId);

            if (resident == null) throw new Exception("Resident with ID: " + residentId + " doesn't exist.");
            ResidentDTOForDetail residentDTOForDetail = new ResidentDTOForDetail();
            residentDTOForDetail.FullName = resident.FullName;
            residentDTOForDetail.IdentityCardNumber = resident.IdentityCardNumber;
            residentDTOForDetail.Status = Enum.GetName(typeof(AccountStatus), resident.Status);
            residentDTOForDetail.Phone = resident.Phone;

            List<RoomDTOForDetail> roomDTOForDetails = new List<RoomDTOForDetail>();
            List<History> histories = _historyRepo.FindByResidentId(residentId);
            switch (roomStatus)
            {
                case "RENTING":
                    {
                        histories = histories.
                            Where(h => h.ResidentId == residentId && h.EndDate == null && h.StartDate < DateTime.Now).ToList();
                        break;
                    }
                case "USED_TO_RENT":
                    {
                        histories = histories.
                            Where(h => h.ResidentId == residentId && h.EndDate != null).ToList();
                        break;
                    }
                case "BOOKING":
                    {
                        DateTime dateTimePoint = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                        histories = histories.
                            Where(h => h.ResidentId == residentId && h.StartDate > dateTimePoint).ToList();
                        break;
                    }
                default:
                    {
                        throw new Exception("Status of renting must be RENTING, USED_TO_RENT or BOOKING");
                    }
            }

            long total = histories.Count;
            histories = histories.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            foreach (History history in histories)
            {
                Room room = history.Room;
                RoomDTOForDetail roomDTOForDetail = new RoomDTOForDetail();
                roomDTOForDetail.Code = room.Code;
                roomDTOForDetail.MotelChainName = _motelChainRepo.FindById(room.MotelId).Name;
                roomDTOForDetail.StartDate = history.StartDate.ToString("dd/MM/yyyy");
                roomDTOForDetail.EndDate = history.EndDate == null ? "-" : history.EndDate.Value.ToString("dd/MM/yyyy");

                roomDTOForDetails.Add(roomDTOForDetail);
            }
            
            residentDTOForDetail.RoomDTOForDetails = roomDTOForDetails;
            Pagination pagination = new Pagination();
            pagination.PageSize = pageSize;
            pagination.CurrentPage = currentPage;
            pagination.Total = total;
            CommonResponse response = new CommonResponse();
            response.Data = residentDTOForDetail;
            response.Pagination = pagination;
            response.Message = "Get resident successfully";
            return response;
        }

        public bool BookRoom(BookingRoomRequest bookingRoomRequest, long managerId)
        {
            Resident resident = _residentRepo.FindByIdentityCardNumberToBookRoom(bookingRoomRequest.IdentityCardNumber);
            if (resident == null) throw new Exception("Resident with identity card number: " + bookingRoomRequest.IdentityCardNumber + " doesn't exist.");
            if (resident.Status == AccountStatus.LATE_PAYMENT) throw new Exception("This resident has an invoice that is not paid yet.");

            Room room = _roomRepo.CheckAndGetBeforeBookingById(managerId, bookingRoomRequest.RoomId);
            if (room == null) throw new Exception("Room with ID: " + bookingRoomRequest.RoomId + " doesn't exist or isn't managed by the manager.");
            if (room.Status != RoomStatus.EMPTY) throw new Exception("Booked room's status must be EMPTY.");

            _residentRepo.UpdateStatusWhenBookingByIdentityCardNumber(bookingRoomRequest.IdentityCardNumber);
            _roomRepo.UpdateStatusWhenBookingById(managerId, bookingRoomRequest.RoomId, bookingRoomRequest.StartDate);
            History history = new History();
            history.RoomId = bookingRoomRequest.RoomId;
            history.StartDate = bookingRoomRequest.StartDate;
            history.ResidentId = resident.Id;
            history.EndDate = null;
            _historyRepo.Add(history);
            return true;
        }
        public IEnumerable<ResidentDTO> FillterResident(string idCardNumber, string phone, string Fullname,int status, int pageSize, int currentPage)
        {
            return _residentRepo.FillterResident(idCardNumber, phone, Fullname, status, pageSize, currentPage);
        }

        public IEnumerable<ResidentDTO> getAllResident(int pageSize, int currentPage)
        {
            return _residentRepo.GetAllResident(pageSize, currentPage);
        }
    }
}
