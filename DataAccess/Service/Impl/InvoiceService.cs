using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IElectricityCostRepo _electricityCostRepo;
        private readonly IWaterCostRepo _waterCostRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly IHistoryRepo _historyRepo;
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly IResidentRepo _residentRepo;

        public InvoiceService(IElectricityCostRepo electricityCostRepo, IWaterCostRepo waterCostRepo, IRoomRepo roomRepo, IHistoryRepo historyRepo, IInvoiceRepo invoiceRepo, IResidentRepo residentRepo)
        {
            _electricityCostRepo = electricityCostRepo;
            _waterCostRepo = waterCostRepo;
            _roomRepo = roomRepo;
            _historyRepo = historyRepo;
            _invoiceRepo = invoiceRepo;
            _residentRepo = residentRepo;
        }

        public Invoice AddInitialInvoice(long residentId, long roomId, DateTime startDate)
        {
            ElectricityCost electricityCost = _electricityCostRepo.GetElectricityCostForCreatingInvoicesByDate(startDate);
            WaterCost waterCost = _waterCostRepo.GetWaterCostForCreatingInvoicesByDate(startDate);
            Invoice previousInvoice = GetPreviousInvoiceByRoomId(roomId);

            Invoice invoice = new()
            {
                ElectricityConsumptionStart = previousInvoice.ElectricityConsumptionEnd.GetValueOrDefault(0),
                ElectricityConsumptionEnd = null,
                WaterConsumptionStart = previousInvoice.WaterConsumptionEnd.GetValueOrDefault(0),
                WaterConsumptionEnd = null,
                CreatedDate = startDate,
                StartDate = startDate,
                EndDate = null,
                PaidDate = null,
                ExpiredDate = null,
                Status = InvoiceStatus.NOT_PAID_YET,
                ElectricityCostId = electricityCost.Id,
                WaterCostId = waterCost.Id,
                RoomId = roomId,
                ResidentId = residentId
            };

            _invoiceRepo.Add(invoice);
            return invoice;
        }

        public bool AutoCheckLateInvoices(DateTime dateTime)
        {
            List<Invoice> lateInvoices = _invoiceRepo.AutoCheckLateInvoices(dateTime);
            return _residentRepo.CheckLatePaymentAccountsByLateInvoices(lateInvoices);
        }

        public bool AutoCloseInvoices(DateTime dateTime)
        {
            return _invoiceRepo.AutoCloseInvoices(dateTime);
        }

        public bool AutoCreateInvoices(DateTime dateTime)
        {
            List<History> histories = _historyRepo.GetNullEndDateHistories(dateTime);
            List<Room> rooms = _roomRepo.GetRoomsForCreatingInvoicesByHistoriesAndDate(histories, dateTime);
            ElectricityCost electricityCost = _electricityCostRepo.GetElectricityCostForCreatingInvoicesByDate(dateTime);
            WaterCost waterCost = _waterCostRepo.GetWaterCostForCreatingInvoicesByDate(dateTime);
            int totalInvoices = histories.Count;

            for (int i = 0; i < totalInvoices; i++)
            {
                Invoice previousInvoice = GetPreviousInvoiceByRoomId(histories.ElementAt(i).RoomId);

                Invoice invoice = new()
                {
                    ElectricityConsumptionStart = previousInvoice.ElectricityConsumptionEnd.GetValueOrDefault(0),
                    ElectricityConsumptionEnd = null,
                    WaterConsumptionStart = previousInvoice.WaterConsumptionEnd.GetValueOrDefault(0),
                    WaterConsumptionEnd = null,
                    CreatedDate = DateTime.Now,
                    StartDate = dateTime,
                    EndDate = null,
                    PaidDate = null,
                    ExpiredDate = null,
                    Status = InvoiceStatus.NOT_PAID_YET,
                    ElectricityCostId = electricityCost.Id,
                    WaterCostId = waterCost.Id,
                    RoomId = rooms.ElementAt(i).Id,
                    ResidentId = histories.ElementAt(i).ResidentId
                };

                _invoiceRepo.Add(invoice);
            }

            return true;
        }

        public Invoice FindById(long id)
        {
            return _invoiceRepo.FindById(id);
        }

        public Invoice GetPreviousInvoiceByRoomId(long roomId)
        {
            Invoice previousInvoice = _invoiceRepo.GetPreviousInvoiceByRoomId(roomId);
            if (previousInvoice == null)
            {
                previousInvoice = new Invoice();
                previousInvoice.ElectricityConsumptionStart = 0;
                previousInvoice.WaterConsumptionStart = 0;
                return previousInvoice;
            }
            else return previousInvoice;
        }
    }
}
