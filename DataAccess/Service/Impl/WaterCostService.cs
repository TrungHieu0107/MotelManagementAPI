﻿using BussinessObject.DTO;
using BussinessObject.Models;
using DataAccess.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<WaterCostDTO>> GetWaterCost(int year, int month)
        {
            var waterCosts = await _waterCostRepo.GetWaterCostByMonthAndYear(year, month);
            var serialized = JsonConvert.SerializeObject(waterCosts);
            var waterCostsDTO = JsonConvert.DeserializeObject<List<WaterCostDTO>>(serialized);
            return waterCostsDTO;
        }
        private WaterCost GetWaterCostByDate(DateTime date)
        {
            return _waterCostRepo.GetWaterCostAfterDate( date);
        }

        public WaterCostDTO UpdateWaterCost(WaterRequestDTO obj)
        {
            // Validate tất cả các filed
            var waterCost = new WaterCost();
            waterCost.Price = obj.Price;
            DateTime appliedDate = new DateTime(obj.AppliedYear, obj.AppliedMonth, 5);
            waterCost.AppliedDate = appliedDate;

            if (waterCost != null)
            {

                var updateData = GetWaterCostByDate(DateTime.Today);
                // Kiểm tra có tồn tại một đối tượng WaterCost có appliedDate sau ngày hôm nay không hay không
                // Nếu có thì tiến hành update nếu không thì sẽ tạo mới
                if (updateData != null)
                {
                    updateData.Price = waterCost.Price;
                    updateData.AppliedDate = waterCost.AppliedDate;
                    _waterCostRepo.UpdateWaterCost(updateData);
                }
                else /// nếu không có đối tượng ElectricityCost nào áp dụng trong tương lai thì sẽ tiến hành tạo mới
                {
                    _waterCostRepo.AddWaterCost(waterCost);
                }

            }

            var serialized = JsonConvert.SerializeObject(waterCost);
            var waterCostDTO = JsonConvert.DeserializeObject<WaterCostDTO>(serialized);
            return waterCostDTO;
        }

       
      
    }
}
