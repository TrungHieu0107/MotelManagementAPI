using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using BussinessObject.Status;
using BussinessObject.Status;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly Context _context;
        private readonly DbSet<Invoice> _invoices;
        IWaterCostRepo _wareCostRepo;
        IElectricityRepo _electricityRepo;
        public InvoiceRepo(Context context, IWaterCostRepo waterCostRepo, IElectricityRepo electricityRepo)
        {
            this._context = context;
            _invoices = new Context().Set<Invoice>();
            _wareCostRepo = waterCostRepo;
            _electricityRepo = electricityRepo;
        }
        public List<Invoice> checkLateInvoice(string idCard)
        {
            return _context.Invoices.Where(
                   p => p.Resident.IdentityCardNumber == idCard && p.Status == InvoiceStatus.LATE).ToList<Invoice>();
        }

        public long CountInvocieHistoryHasRoomId(long roomId)
        {
            return (from invoice in _invoices
                    where invoice.RoomId == roomId
                    &&
                    invoice.ElectricityConsumptionEnd != null
                    &&
                    invoice.WaterConsumptionEnd != null
                    &&
                    invoice.PaidDate != null
                    && invoice.Status == InvoiceStatus.PAID
                    select invoice).LongCount();
        }

        public IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomWithPaging(long roomId, Pagination pagination)
        {
            return (from invoice in _invoices
                    where invoice.RoomId == roomId
                    && 
                    invoice.ElectricityConsumptionEnd != null
                    &&
                    invoice.WaterConsumptionEnd != null
                    && 
                    invoice.PaidDate != null
                    && invoice.Status == InvoiceStatus.PAID
                    select invoice)
                          .Include(x => x.WaterCost)
                          .Include(x => x.ElectricityCost)
                          .Skip((pagination.CurrentPage - 1) * pagination.PageSize)
                          .Select(x => new InvoiceDTO()
                          {
                              Id = x.Id,
                              CreatedDate = x.CreatedDate,
                              RoomId = x.RoomId,
                              ElectricityConsumptionEnd = x.ElectricityConsumptionEnd,
                              ElectricityConsumptionStart = x.ElectricityConsumptionStart,
                              WaterConsumptionEnd = x.WaterConsumptionEnd,
                              WaterConsumptionStart = x.WaterConsumptionStart,
                              PaidDate = x.PaidDate,
                              Status = x.Status,
                              ElectricityCost = new ElectricityCostDTO()
                              {
                                  Price = x.ElectricityCost.Price,
                              },
                              WaterCost = new WaterCostDTO()
                              {
                                  Price = x.WaterCost.Price,
                              },
                              EndDate = x.EndDate,
                              ExpiredDate = x.ExpiredDate,
                          })
                          .Take(pagination.PageSize);
        }

        public async Task<IEnumerable<Invoice>> GetInvoiceOfRoom(long roomId, int? pageNumber, int? pageSize)
        {
            int page = pageNumber ?? 1;
            int size = pageSize ?? 10;
            return await (from invoice in _invoices
                          where invoice.RoomId == roomId
                          select invoice)
                          .Include(x => x.WaterCost)
                          .Include(x => x.ElectricityCost)
                          .Skip((page - 1) * size)
                          .Select(x => new Invoice()
                          {
                              Id = x.Id,
                              CreatedDate = x.CreatedDate,
                              RoomId = x.RoomId,
                              ElectricityConsumptionEnd = x.ElectricityConsumptionEnd,
                              ElectricityConsumptionStart = x.ElectricityConsumptionStart,
                              WaterConsumptionEnd = x.WaterConsumptionEnd,
                              WaterConsumptionStart = x.WaterConsumptionStart,
                              PaidDate = x.PaidDate,
                              Status = x.Status,
                              ElectricityCost = new ElectricityCost()
                              {
                                  Price = x.ElectricityCost.Price,
                              },
                              WaterCost = new WaterCost()
                              {
                                  Price = x.WaterCost.Price,
                              },
                              EndDate = x.EndDate,
                              ExpiredDate = x.ExpiredDate,
                          })
                          .Take(size)
                          .ToListAsync();
        }
    }
}
