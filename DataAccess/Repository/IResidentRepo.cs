using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using BussinessObject.Status;
using System;
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
        public bool CheckLatePaymentAccountsByLateInvoices(List<Invoice> invoices);
        public Resident FindById(long id);
        public Resident FindByIdentityCardNumberToBookRoom(string identityCardNumber);
        public Resident FindByIdentityCardNumber(string identityCardNumber);
        public Resident UpdateStatusWhenBookingByIdentityCardNumber(string identityCardNumber);
        public bool UpdateStatusOfResident(long residentId, AccountStatus status);

        public IEnumerable<ResidentDTO> GetAllResident(int pageSize, int currentPage);

        IEnumerable<ResidentDTO>  FillterResident(string idCardNumber, string phone, string Fullname,int status, int pageSize, int currentPage);
        public CommonResponse FillterResidentWithPagination(string idCardNumber, string phone, string Fullname, int status, int pageSize, int currentPage);
        Resident findByPhone(String phone);
    }
}
