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
            return _context.Invoices
                .Where(i => i.Id == id)
                .Include(invoice => invoice.Resident)
                .FirstOrDefault();
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

        public IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomNotPaidYet(string RoomCode)
        {
            return _invoices.Where(invoice => 
                                    invoice.Room.Code == RoomCode
                                && 
                                    (
                                        invoice.Status == InvoiceStatus.NOT_PAID_YET ||
                                        invoice.Status == InvoiceStatus.LATE
                                    )
                                && invoice.EndDate != null
                                    )
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

        public InvoiceDTO GetInvoiceDetailById(long id, long userId, long managerId)
        {
            var result = _context.Invoices
                .Include(invoice => invoice.WaterCost)
                .Include(invoice => invoice.ElectricityCost)
                .Include(invoice => invoice.Room.MotelChain)
                .Include(invoice => invoice.Room.MotelChain.Manager)
                .Where(x => x.Id == id &&
                (userId != -1 ? x.ResidentId == userId : true)
                && 
                (managerId != -1 ? x.Room.MotelChain.ManagerId == managerId : true)
                )
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
                        RentFee = invoice.Room.RentFee,
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

        public IEnumerable<InvoiceDTO> GetAllInvoice(string roomCode, int status, DateTime? paidDate, long userId, long managerId, ref Pagination pagination)
        {
            pagination = pagination ?? new Pagination();

            var listAllInvoice = _context.Invoices
                .Include(x => x.Room)
                .Include(x => x.Room.MotelChain)
                .Include(x => x.Resident)
                .Where(invoice =>
                (status == -1 ? true : invoice.Status == (InvoiceStatus)status)
                &&
                (roomCode == null ? true : invoice.Room.Code.Contains(roomCode))
                && 
                (userId != -1 ? invoice.Resident.Id == userId : true)
                &&
                (invoice.EndDate != null)
                &&
                (managerId != -1 ? invoice.Room.MotelChain.ManagerId == managerId : true)
                && 
                (invoice.PaidDate.HasValue ? (paidDate.HasValue ? invoice.PaidDate.Value.Date == paidDate.Value.Date : true) : true)
                ).Select(invoice => new InvoiceDTO
                {
                    Id = invoice.Id,
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
                });

            pagination.Total = listAllInvoice.LongCount();

            return listAllInvoice.OrderByDescending(i => i.StartDate).Skip((pagination.CurrentPage - 1) * pagination.PageSize)
                .Take(pagination.PageSize);

        }

        public InvoiceDTO UpdateRoomIdfOfInvoice(long newRoomId, long oldRoomId)
        {
            var invoice = _context.Invoices
                .Where(invoice => invoice.RoomId == oldRoomId)
                .OrderByDescending(invoice => invoice.Id)
                .FirstOrDefault();

            if(invoice == null)
            {
                return null;
            }
            
            invoice.RoomId = newRoomId;
            _context.Entry(invoice).State = EntityState.Modified;
            if(_context.SaveChanges() <= 0)
            {
                return null;
            }
            
            return new InvoiceDTO
            {
                Id = invoice.Id,
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
            };
        }

        public List<InvoiceDTO> GetClosedSampleInvoicesThatNotPayYetByResidentIdAndRoomId(long residentId, long roomId, DateTime checkoutDate)
        {
            List<InvoiceDTO> invoiceDTOs = new List<InvoiceDTO>(); 
            Invoice oldInvoice = _context.Invoices
                .Include(i => i.WaterCost)
                .Include(i => i.ElectricityCost)
                .Include(i => i.Resident)
                .Include(i => i.Room)
                .Include(i => i.Room.MotelChain)
                .Include(i => i.Room.MotelChain.Manager)
                .Where(
                i => i.ResidentId == residentId && i.RoomId == roomId &&
                i.PaidDate == null && i.EndDate != null).FirstOrDefault();
            Invoice newInvoice = _context.Invoices
                .Include(i => i.WaterCost)
                .Include(i => i.ElectricityCost)
                .Include(i => i.Resident)
                .Include(i => i.Room)
                .Include(i => i.Room.MotelChain)
                .Include(i => i.Room.MotelChain.Manager).Where(
                i => i.ResidentId == residentId && i.RoomId == roomId &&
                i.PaidDate == null && i.EndDate == null).FirstOrDefault();
            if (oldInvoice != null)
            {
                invoiceDTOs.Add(InvoiceToClosedInvoiceDTO(oldInvoice, checkoutDate));
            }
            invoiceDTOs.Add(InvoiceToClosedInvoiceDTO(newInvoice, checkoutDate));
            return invoiceDTOs;
        }

        public List<InvoiceDTO> UpdateCheckOutDateForResident(long residentId, long roomId, DateTime checkoutDate)
        {
            List<InvoiceDTO> invoiceDTOs = new List<InvoiceDTO>();
            Invoice oldInvoice = _context.Invoices
                .Include(i => i.WaterCost)
                .Include(i => i.ElectricityCost)
                .Include(i => i.Resident)
                .Include(i => i.Room)
                .Include(i => i.Room.MotelChain)
                .Include(i => i.Room.MotelChain.Manager).Where(
                i => i.ResidentId == residentId && i.RoomId == roomId &&
                i.PaidDate == null && i.EndDate != null).FirstOrDefault();
            Invoice newInvoice = _context.Invoices
                .Include(i => i.WaterCost)
                .Include(i => i.ElectricityCost)
                .Include(i => i.Resident)
                .Include(i => i.Room)
                .Include(i => i.Room.MotelChain)
                .Include(i => i.Room.MotelChain.Manager).Where(
                i => i.ResidentId == residentId && i.RoomId == roomId &&
                i.PaidDate == null && i.EndDate == null).FirstOrDefault();
            if (oldInvoice != null)
            {
                oldInvoice.PaidDate = checkoutDate;
                oldInvoice.Status = InvoiceStatus.PAID;
                _context.Entry(oldInvoice).State = EntityState.Modified;
                if (_context.SaveChanges() <= 0) throw new Exception("Đã có lỗi xảy ra ở phía máy chủ.");
                invoiceDTOs.Add(InvoiceToClosedInvoiceDTO(oldInvoice, null));
            }
            DateTime RoundedDate = new DateTime(checkoutDate.Year, checkoutDate.Month, checkoutDate.Day);
            newInvoice.PaidDate = checkoutDate;
            newInvoice.Status = InvoiceStatus.PAID;
            newInvoice.EndDate = RoundedDate;
            newInvoice.ExpiredDate = RoundedDate.AddDays(FixedData.DATE_TO_PAY);
            newInvoice.ElectricityConsumptionEnd = newInvoice.ElectricityConsumptionStart + 1;
            newInvoice.WaterConsumptionEnd = newInvoice.WaterConsumptionStart + 1;
            _context.Entry(newInvoice).State = EntityState.Modified;
            if (_context.SaveChanges() <= 0) throw new Exception("Đã có lỗi xảy ra ở phía máy chủ.");
            invoiceDTOs.Add(InvoiceToClosedInvoiceDTO(newInvoice, null));
            return invoiceDTOs;
        }

        private InvoiceDTO InvoiceToClosedInvoiceDTO(Invoice invoice, DateTime? dateTime)
        {
            DateTime RoundedDate = DateTime.Now;
            if (dateTime != null)
            {
                RoundedDate = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day);
            }

            InvoiceDTO invoiceDTO = new InvoiceDTO();
            invoiceDTO.Id = invoice.Id;
            invoiceDTO.StartDate = invoice.StartDate;
            invoiceDTO.EndDate = invoice.EndDate == null ? RoundedDate : invoice.EndDate; //////////////////
            invoiceDTO.CreatedDate = invoice.CreatedDate;
            invoiceDTO.ExpiredDate = invoice.ExpiredDate == null ? RoundedDate.AddDays(FixedData.DATE_TO_PAY) : invoice.ExpiredDate; /////////////
            invoiceDTO.PaidDate = invoice.PaidDate == null ? dateTime : invoice.PaidDate; ////////////////////
            invoiceDTO.Status = invoice.Status;
            invoiceDTO.ElectricityConsumptionEnd = invoice.ElectricityConsumptionEnd != null ? invoice.ElectricityConsumptionEnd : invoice.ElectricityConsumptionStart + 1;
            invoiceDTO.ElectricityConsumptionStart = invoice.ElectricityConsumptionStart;
            invoiceDTO.ElectricityCostId = invoice.ElectricityCostId;
            invoiceDTO.ElectricityCost = new ElectricityCostDTO()
            {
                Price = invoice.ElectricityCost.Price,
                Id = invoice.ElectricityCost.Id,
                AppliedDate = invoice.ElectricityCost.AppliedDate
            };
            invoiceDTO.WaterConsumptionEnd = invoice.WaterConsumptionEnd != null ? invoice.WaterConsumptionEnd : invoice.WaterConsumptionStart + 1;
            invoiceDTO.WaterConsumptionStart = invoice.WaterConsumptionStart;
            invoiceDTO.WaterCost = new WaterCostDTO()
            {
                Price = invoice.WaterCost.Price,
                Id = invoice.WaterCost.Id,
                AppliedDate = invoice.WaterCost.AppliedDate
            };
            invoiceDTO.Resident = new ResidentDTO()
            {
                Id = invoice.Resident.Id,
                FullName = invoice.Resident.FullName,
                IdentityCardNumber = invoice.Resident.IdentityCardNumber,
                Phone = invoice.Resident.Phone
            };

            RoomDTO roomDTO = new RoomDTO();
            roomDTO.Id = invoice.Room.Id;
            roomDTO.Code = invoice.Room.Code;
            roomDTO.FeeAppliedDate = invoice.Room.FeeAppliedDate;
            roomDTO.RentFee = invoice.Room.RentFee;

            roomDTO.MotelChain = new MotelChainDTO()
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
            };
            invoiceDTO.Room = roomDTO;
            return invoiceDTO;
        }
    }
}
