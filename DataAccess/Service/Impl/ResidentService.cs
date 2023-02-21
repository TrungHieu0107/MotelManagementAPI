using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Service.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepo _residentRepo;
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly IAccountRepo _accountRepo;
        public ResidentService(IResidentRepo _residentRepo, IInvoiceRepo _invoiceRepo, IAccountRepo accountRepo)
        {
            this._residentRepo = _residentRepo;
            this._invoiceRepo = _invoiceRepo;
            _accountRepo = accountRepo;
        }



        public ResidentDTO GetResidentByIdentityCardNumber(string idCard)
        {
            var resident = _residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCard, null).FirstOrDefault<Resident>();
            var serialized = JsonConvert.SerializeObject(resident);
            var residentDTO = JsonConvert.DeserializeObject<ResidentDTO>(serialized);
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
                resident.Password = account.Password;
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
            if (idCardCheck == null)
            {
                throw new Exception("Indentity Number all read");
            }
            // check if user exsit 
            Resident resident = _residentRepo.findById(id);

            // user exist , idCardnumber input is unique
            if (resident != null && idCardCheck == null)
            {

                resident.IdentityCardNumber = account.IdentityCardNumber;


                resident.Password = account.Password;
                resident.FullName = account.FullName;
                resident.Phone = account.Phone;
              
                //Save change
                var updatedResident = _residentRepo.UpdateResidentAccount(resident);
                if (updatedResident != null)
                {
                    check = true;
                }
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


    }
}
