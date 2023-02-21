using BussinessObject.DTO;

namespace DataAccess.Service
{
    public interface IResidentService
    {
        bool CreatResidentAccount(AccountDTO accountDTO);


        ResidentDTO GetResidentByIdentityCardNumber(string idCard);

        bool DeActiveResident(string idCard);

        bool UpdateResidentAccount(long id, ResidentUpdateDTO account);

        bool ActiveResident(string idCard);
    }
}
