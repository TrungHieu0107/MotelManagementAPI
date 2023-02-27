using BussinessObject.DTO;
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

        public IEnumerable<ResidentDTO> getAllResident(int pageSize, int currentPage)
        {
            return _residentRepo.GetAllResident(pageSize, currentPage);
        }

        public IEnumerable<ResidentDTO> FillterResident(string idCardNumber, string phone, string Fullname,int status, int pageSize, int currentPage)
        {
            return _residentRepo.FillterResident(idCardNumber, phone, Fullname, status, pageSize, currentPage);
        }
    }
}
