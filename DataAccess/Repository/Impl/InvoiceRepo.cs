using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
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
        IWaterCostRepo _waterCostRepo;
        IElectricityCostRepo _electricityCostRepo;
        public InvoiceRepo(Context context, IWaterCostRepo waterCostRepo, IElectricityCostRepo electricityRepo)
        {
            this._context = context;
            _invoices = new Context().Set<Invoice>();
            _waterCostRepo = waterCostRepo;
            _electricityCostRepo = electricityRepo;
        }
        public void Add(Invoice invoice)
        {
            _context.Add(invoice);
            _context.SaveChanges();
        }

        public List<Invoice> AutoCheckLateInvoices(DateTime dateTime)
        {
            List<Invoice> lateInvoices = GetLateInvoices(dateTime);

            foreach (Invoice invoice in lateInvoices)
            {
                CheckLateInvoice(invoice.Id);
            }
            return lateInvoices;
        }

        public List<Invoice> GetLateInvoices(DateTime dateTime)
        {
            return _context.Invoices.Where(i => i.ExpiredDate <= dateTime.AddMinutes(5) && i.Status == InvoiceStatus.NOT_PAID_YET).ToList();
        }

        public bool AutoCloseInvoices(DateTime dateTime)
        {
            List<Invoice> closedInvoices = GetInvoicesToClose(dateTime);
            foreach (Invoice invoice in closedInvoices)
            {
                UpdateClosedInvoice(invoice, dateTime);
            }
            return true;
        }

        private List<Invoice> GetInvoicesToClose(DateTime dateTime)
        {
            return _context.Invoices.
                Where(i =>
                i.EndDate == null &&
                i.StartDate <= dateTime.AddMinutes(5).AddMonths(-1)
                ).ToList();
        }

        public Invoice FindById(long id)
        {
            return _context.Invoices.Where(i => i.Id == id).FirstOrDefault();
        }

        public Invoice GetPreviousInvoiceByRoomId(long roomId)
        {
            return _context.Invoices
                .Where(i => i.RoomId == roomId && i.EndDate != null)
                .OrderBy(i => i.EndDate).LastOrDefault();
        }

        private Invoice CheckLateInvoice(long invoiceId)
        {
            Invoice invoice = FindById(invoiceId);
            invoice.Status = InvoiceStatus.LATE;
            var tracker = _context.Attach(invoice);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return invoice;
        }

        private Invoice UpdateClosedInvoice(Invoice invoice, DateTime endDate)
        {
            invoice.ElectricityConsumptionEnd = invoice.ElectricityConsumptionStart + 1;
            invoice.WaterConsumptionEnd = invoice.WaterConsumptionStart + 1;
            invoice.EndDate = endDate;
            invoice.ExpiredDate = endDate.AddDays(FixedData.DATE_TO_PAY);
            var tracker = _context.Attach(invoice);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return invoice;
        }

        public List<Invoice> checkLateInvoice(string idCard)
        {
            return _context.Invoices.Where(p =>
                            p.Resident.IdentityCardNumber == idCard
                        &&
                            p.Status == InvoiceStatus.LATE)
                        .ToList<Invoice>();
        }

        public long CountInvocieHistoryHasRoomId(long roomId)
        {

            return _invoices.Where(invoice =>
                            invoice.RoomId == roomId
                        &&
                            invoice.ElectricityConsumptionEnd != null
                        &&
                            invoice.WaterConsumptionEnd != null
                        &&
                            invoice.PaidDate != null
                        &&
                            invoice.Status == InvoiceStatus.PAID
                        ).LongCount();
        }


        public IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomWithPaging(long roomId, Pagination pagination)
        {
            return _invoices.Where(invoice =>
                                invoice.RoomId == roomId
                            &&
                                invoice.ElectricityConsumptionEnd != null
                            &&
                                invoice.WaterConsumptionEnd != null
                            &&
                                invoice.PaidDate != null
                            &&
                                invoice.Status == InvoiceStatus.PAID
                            )
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

        public IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomWithUnPayStatus(string RoomCode)
        {
            return (from invoice in _invoices
                    where invoice.Room.Code == RoomCode
                    && invoice.Status == InvoiceStatus.NOT_PAID_YET
                    select invoice)
                          .Include(x => x.WaterCost)
                          .Include(x => x.ElectricityCost)

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
                          });

        }

        public int UpdateInvoiceStatus(Invoice invoice)
        {


            _context.Entry(invoice).State = EntityState.Modified;
            return _context.SaveChanges();


        }

        public InvoiceDTO GetInvoiceDetailById(long id)
        {
            var result = _context.Invoices
                .Where(x => x.Id == id)
                .Include(invoice => invoice.WaterCost)
                .Include(invoice => invoice.ElectricityCost)
                .Include(invoice => invoice.Room.MotelChain)
                .Include(invoice => invoice.Room.MotelChain.Manager)
                .Select(invoice => new InvoiceDTO
                {
                    Id = id,
                    StartDate = invoice.StartDate,
                    EndDate = invoice.EndDate,
                    CreatedDate = invoice.CreatedDate,
                    ExpiredDate = invoice.ExpiredDate,
                    PaidDate = invoice.PaidDate,
                    Status = invoice.Status,
                    ElectricityConsumptionEnd = invoice.ElectricityConsumptionEnd,
                    ElectricityConsumptionStart = invoice.ElectricityConsumptionStart,
                    ElectricityCostId = invoice.ElectricityCostId,
                    ElectricityCost = new ElectricityCostDTO()
                    {
                        Price = invoice.ElectricityCost.Price,
                        Id = invoice.ElectricityCost.Id,
                        AppliedDate = invoice.ElectricityCost.AppliedDate
                    },
                    WaterConsumptionEnd = invoice.WaterConsumptionEnd,
                    WaterConsumptionStart = invoice.WaterConsumptionStart,
                    WaterCost = new WaterCostDTO()
                    {
                        Price = invoice.WaterCost.Price,
                        Id = invoice.WaterCost.Id,
                        AppliedDate = invoice.WaterCost.AppliedDate
                    },
                    Resident = new ResidentDTO()
                    {
                        Id = invoice.Resident.Id,
                        FullName = invoice.Resident.FullName,
                        IdentityCardNumber = invoice.Resident.IdentityCardNumber,
                        Phone = invoice.Resident.Phone
                    },
                    Room = new RoomDTO()
                    {
                        Id = invoice.Room.Id,
                        Code = invoice.Room.Code,
                        FeeAppliedDate = invoice.Room.FeeAppliedDate,
                        MotelChain = new MotelChainDTO()
                        {
                            Id = invoice.Room.MotelChain.Id,
                            Address = invoice.Room.MotelChain.Address,
                            Name = invoice.Room.MotelChain.Name,
                            Manager = new ManagerDTO()
                            {
                                FullName = invoice.Room.MotelChain.Manager.FullName,
                                IdentityCardNumber = invoice.Room.MotelChain.Manager.IdentityCardNumber,
                                Phone = invoice.Room.MotelChain.Manager.Phone
                            },
                        },
                    }
                }).FirstOrDefault();

            return result;
        }
    }
}
