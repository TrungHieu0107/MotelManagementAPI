using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service.Impl
{
    [Authorize]
    public class ElectricityCostService : IElectricityCostService
    {

        private readonly IElectricityRepo _electricityRepo;

        public ElectricityCostService(IElectricityRepo _electricityRepo)
        {
            this._electricityRepo = _electricityRepo;
        }
        public async Task<IEnumerable<ElectricityCostDTO>> GetElectrictyCost( int year, int month)
        {
            var electricityCosts = await _electricityRepo.GetElectricityCostByMonthAndYear(year, month);
            var serialized = JsonConvert.SerializeObject(electricityCosts);
            var electricityCostsDTO = JsonConvert.DeserializeObject<List<ElectricityCostDTO>>(serialized);
            return electricityCostsDTO;


        }

        public async Task<ElectricityCostDTO> GetCurrentElectricityCost()
        {
            var electricityCost = await _electricityRepo.GetCurrentElectrictyCost();
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
            return _electricityRepo.GetElectricitAfterDate(date);
        }

        public ElectricityCostDTO UpdateElectricity(ElectricityCostRequestDTO obj)
        {
            DateTime appliedDate = new DateTime(obj.AppliedYear, obj.AppliedMonth, 5);
            ElectricityCost electricityCost = new ElectricityCost();
            electricityCost.Price = obj.Price;
            electricityCost.AppliedDate = appliedDate;
            
                    
                ElectricityCost updateData = GetElectricitAfterDate(DateTime.Today);
            // Kiểm tra có tồn tại một đối tượng ElectricityCost có appliedDate sau ngày hôm nay không hay không
            // Nếu có thì tiến hành update nếu không thì sẽ tạo mới
            if (updateData != null)
            {
                updateData.Price = obj.Price;
                updateData.AppliedDate = appliedDate;
                _electricityRepo.UpdateElectricityCost(updateData);
            }
            else /// nếu không có đối tượng ElectricityCost nào áp dụng trong tương lai thì sẽ tiến hành tạo mới
            {
               
                _electricityRepo.AddElectricityCost(electricityCost);
            }        
            var serialized = JsonConvert.SerializeObject(electricityCost);
            var electricityCostDTO = JsonConvert.DeserializeObject<ElectricityCostDTO>(serialized);
            return electricityCostDTO;
        }

      
    }
}
