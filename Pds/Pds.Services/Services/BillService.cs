﻿using System;
using System.Threading.Tasks;
using Pds.Core.Enums;
using Pds.Data;
using Pds.Services.Interfaces;
using Pds.Services.Models.Bill;

namespace Pds.Services.Services
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork unitOfWork;

        public BillService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task PayBillAsync(PayBillModel model)
        {
            var bill = await unitOfWork.Bills.GetFirstWhereAsync(b => b.Id == model.BillId);
            if (bill != null)
            {
                bill.Cost = model.Cost;
                bill.PaymentType = model.PaymentType;
                bill.Comment = model.Comment;
                bill.Status = BillStatus.Paid;
                bill.UpdatedAt = DateTime.UtcNow;

                await unitOfWork.Bills.UpdateAsync(bill);
            }
        }
    }
}