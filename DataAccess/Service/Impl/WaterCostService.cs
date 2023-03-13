using BussinessObject.DTO;
using BussinessObject.Models;
using DataAccess.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Service.Impl
{
    public class WaterCostService : IWaterCostService
    {
        private readonly IWaterCostRepo _waterCostRepo;

        public WaterCostService(IWaterCostRepo waterCostRepo)
        {
            this._waterCostRepo = waterCostRepo;
        }
        public async Task<WaterCostDTO> GetCurrentWaterCost()
        {
            var waterCost = await _waterCostRepo.GetCurrentWaterCost();
            var serialized = JsonConvert.SerializeObject(waterCost);
            var waterCostDTO = JsonConvert.DeserializeObject<WaterCostDTO>(serialized);
            return waterCostDTO;
        }

        public async Task<IEnumerable<WaterCostDTO>> GetWaterCost(int year, int month, int currentPage, int pageSize)
        {
            var waterCosts = await _waterCostRepo.GetWaterCostByMonthAndYear(year, month, currentPage, pageSize);
            var serialized = JsonConvert.SerializeObject(waterCosts);
            var waterCostsDTO = JsonConvert.DeserializeObject<List<WaterCostDTO>>(serialized);
            return waterCostsDTO;
        }
        private WaterCost GetWaterCostByDate(DateTime date)
        {
            return _waterCostRepo.GetWaterCostAfterDate(date);
        }

        public WaterCostDTO UpdateWaterCost(WaterRequestDTO obj)
        {
            // Validate tất cả các filed
            var waterCost = new WaterCost();
            waterCost.Price = obj.Price;
            DateTime appliedDate = new DateTime(obj.AppliedYear, obj.AppliedMonth, 5);
            waterCost.AppliedDate = appliedDate;
            int check = 0;
            var updateData = GetWaterCostByDate(DateTime.Today);
            // Kiểm tra có tồn tại một đối tượng WaterCost có appliedDate sau ngày hôm nay không hay không
            // Nếu có thì tiến hành update nếu không thì sẽ tạo mới
           
               
                if (updateData != null)
                {
                    updateData.Price = waterCost.Price;
                    updateData.AppliedDate = waterCost.AppliedDate;
                    check = _waterCostRepo.UpdateWaterCost(updateData);
                }
                else /// nếu không có đối tượng ElectricityCost nào áp dụng trong tương lai thì sẽ tiến hành tạo mới
                {
                    check =_waterCostRepo.AddWaterCost(waterCost);
                }

            
            if (check > 0)
            {
                var serialized = JsonConvert.SerializeObject(waterCost);
                var waterCostDTO = JsonConvert.DeserializeObject<WaterCostDTO>(serialized);
                return waterCostDTO;
            }
            else throw new Exception("Cập nhật giá nước không thành công");
         
        }



    }
}
