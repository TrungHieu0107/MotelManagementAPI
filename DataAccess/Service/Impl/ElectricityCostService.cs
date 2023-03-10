using BussinessObject.DTO;
using BussinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Service.Impl
{
    public class ElectricityCostService : IElectricityCostService
    {

        private readonly IElectricityCostRepo _electricityCostRepo;

        public ElectricityCostService(IElectricityCostRepo _electricityCostRepo)
        {
            this._electricityCostRepo = _electricityCostRepo;
        }
        public async Task<IEnumerable<ElectricityCostDTO>> GetElectrictyCost(int year, int month, int curentPage, int pageSize)
        {
            var electricityCosts = await _electricityCostRepo.GetElectricityCostByMonthAndYear(year, month, curentPage, pageSize);
            var serialized = JsonConvert.SerializeObject(electricityCosts);
            var electricityCostsDTO = JsonConvert.DeserializeObject<List<ElectricityCostDTO>>(serialized);
            return electricityCostsDTO;


        }

        public async Task<ElectricityCostDTO> GetCurrentElectricityCost()
        {
            var electricityCost = await _electricityCostRepo.GetCurrentElectrictyCost();
            var serialized = JsonConvert.SerializeObject(electricityCost);
            var electricityCostDTO = JsonConvert.DeserializeObject<ElectricityCostDTO>(serialized);
            return electricityCostDTO;
        }


        /// <summary>
        /// chỉ cho update với giá tiền lớn hơn ngày hiện tại
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private ElectricityCost GetElectricitAfterDate(DateTime date)
        {
            return _electricityCostRepo.GetElectricitAfterDate(date);
        }

        public ElectricityCostDTO UpdateElectricity(ElectricityCostRequestDTO obj)
        {
            DateTime appliedDate = new DateTime(obj.AppliedYear, obj.AppliedMonth, 5);
            ElectricityCost electricityCost = new ElectricityCost();
            electricityCost.Price = obj.Price;
            electricityCost.AppliedDate = appliedDate;
            int check = 0;


            ElectricityCost updateData = GetElectricitAfterDate(DateTime.Today);
            // Kiểm tra có tồn tại một đối tượng ElectricityCost có appliedDate sau ngày hôm nay không hay không
            // Nếu có thì tiến hành update nếu không thì sẽ tạo mới
            if (updateData != null)
            {
                updateData.Price = obj.Price;
                updateData.AppliedDate = appliedDate;
               check =  _electricityCostRepo.UpdateElectricityCost(updateData);
            }
            else /// nếu không có đối tượng ElectricityCost nào áp dụng trong tương lai thì sẽ tiến hành tạo mới
            {

                check = _electricityCostRepo.AddElectricityCost(electricityCost);
            }
            if (check > 0)
            {
                
                var serialized = JsonConvert.SerializeObject(electricityCost);
                var electricityCostDTO = JsonConvert.DeserializeObject<ElectricityCostDTO>(serialized);
                return electricityCostDTO;

            }
            else throw new Exception("Cập nhật giá điện không thành công");
           
        }


    }
}
