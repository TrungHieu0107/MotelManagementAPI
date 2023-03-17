using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using DataAccess.Security;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Resident checkExistPhone = _residentRepo.findByPhone(account.Phone);



            if (checkExistIdCardNumber == null && checkExistUserName == null && checkExistPhone == null)
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
                throw new TaskCanceledException("Tên đăng nhập đã tồn tại");

            } else if(checkExistIdCardNumber != null) {
                throw new TaskCanceledException("Số chứng minh thư đã tồn tại");
            } else if(checkExistPhone != null)
            {
                throw new TaskCanceledException("Số điện thoại đã tồn tại");
            }

            return check;
        }

        public bool UpdateResidentAccount(long id, ResidentUpdateDTO account)
        {
            bool check = false;
            //  check if id cardnumber is exsit
            Resident idCardCheck = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(account.IdentityCardNumber, null,AccountStatus.ACTIVE).FirstOrDefault<Resident>();
            
            Resident phoneCheck = _residentRepo.findByPhone(account.Phone);          
            
            if ( phoneCheck != null && id != phoneCheck.Id   )
            {
                throw new TaskCanceledException("Số điện thoại đã tồn tại");
            }
            else if (idCardCheck != null && id != idCardCheck.Id)
            {
                throw new TaskCanceledException("Số chứng minh thư đã tồn tại");
            }
            else
            {
                // check if user exsit 
                Resident resident = _residentRepo.findById(id);

                // user exist , idCardnumber input is unique
                if (resident != null)
                {

                    resident.IdentityCardNumber = account.IdentityCardNumber;

                    if (!string.IsNullOrEmpty(account.Password))
                    {
                        resident.Password = PasswordHasher.Hash(account.Password);

                    }
                   
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


       
           

        public bool ActiveResident(string idCard)
        {
            bool check = false;

             //check xem có bị trễ invoice hay không
            var checkLatePayment = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCard, null, AccountStatus.LATE_PAYMENT).FirstOrDefault();


            if (checkLatePayment == null)
            {
                var resident = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCard, null, AccountStatus.INACTIVE).FirstOrDefault();
                if (resident != null)
                {
                    resident.Status = AccountStatus.ACTIVE;
                    _residentRepo.UpdateResidentAccount(resident);
                    check = true;
                }


            } else
            {
                throw new TaskCanceledException("Tài khoản có hóa đơn cần thanh toán");
            }
            
            return check;
        }

        public CommonResponse FindByIdForDetail(long residentId, int pageSize, int currentPage, string roomStatus)
        {
            Resident resident = _residentRepo.FindById(residentId);

            if (resident == null) throw new Exception("Người thuê với ID: " + residentId + " không tồn tại.");
            ResidentDTOForDetail residentDTOForDetail = new ResidentDTOForDetail();
            residentDTOForDetail.Id = residentId;
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
                        throw new Exception("Trạng thái thuê phải là \"Đang thuê\", \"Đã từng thuê\" hoặc \"Đang đặt\"");
                    }
            }

            long total = histories.Count;
            histories = histories.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            foreach (History history in histories)
            {
                Room room = history.Room;
                RoomDTOForDetail roomDTOForDetail = new RoomDTOForDetail();
                roomDTOForDetail.Id = room.Id;
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
            if (resident == null) throw new Exception("Người thuê với căn cước công dân: " + bookingRoomRequest.IdentityCardNumber + " không tồn tại.");
            if (resident.Status == AccountStatus.LATE_PAYMENT) throw new Exception("Người thuê đang có hóa đơn bị trễ.");

            Room room = _roomRepo.CheckAndGetBeforeBookingById(managerId, bookingRoomRequest.RoomId);
            if (room == null) throw new Exception("Phòng với ID: " + bookingRoomRequest.RoomId + " không tồn tại hoặc không thuộc kiểm soát của quản lý.");
            if (room.Status != RoomStatus.EMPTY) throw new Exception("Phòng được đặt hiện không trống");

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

        public CommonResponse FillterResidentWithPagination(string idCardNumber, string phone, string Fullname, int status, int pageSize, int currentPage)
        {
            return _residentRepo.FillterResidentWithPagination(idCardNumber, phone, Fullname, status, pageSize, currentPage);
        }
    }
}
