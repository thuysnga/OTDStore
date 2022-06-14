using Microsoft.AspNetCore.Mvc;
using OTDStore.ViewModels.Sales;
using Microsoft.Extensions.Configuration;
using OTDStore.ApiIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OTDStore.Data.EF;
using OfficeOpenXml;
using System.Data;
using System.IO;
using OfficeOpenXml.Style;
using OTDStore.Data.Enum;

namespace OTDStore.AdminApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;
        private readonly IConfiguration _configuration;
        private readonly OTDDbContext _context;
        public OrderController(IOrderApiClient orderApiClient, IConfiguration configuration,
                            OTDDbContext context)
        {
            _orderApiClient = orderApiClient;
            _configuration = configuration;
            _context = context;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _orderApiClient.GetOrdersPagings(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _orderApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var result = await _orderApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new StatusUpdateRequest()
                {
                    Status = user.Status,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StatusUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _orderApiClient.UpdateOrder(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật hóa đơn thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public IActionResult RevenueStatistic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RevenueStatistic(TimeRequest request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = new DataTable();
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var dateht = DateTime.Now;
                var dayht = DateTime.Now.ToString("dd");
                var monthht = DateTime.Now.ToString("MM");
                var yearht = DateTime.Now.ToString("yyyy");

                var query = from od in _context.OrderDetails
                            join o in _context.Orders on od.OrderId equals o.Id
                            where o.OrderDate.Date >= request.beginT.Date && o.OrderDate.Date <= request.endT.Date
                            && o.Status != (OrderStatus)0
                            select new { od };

                int[] product = new int[100];

                foreach (var item in query)
                {
                    product[item.od.ProductId] += item.od.Quantity;
                }

                var sheet = package.Workbook.Worksheets.Add("Thong ke doanh thu");
                sheet.Cells["A1:M99"].Style.Font.Name = "Times New Roman";
                sheet.Cells["A1:E1"].Merge = true;
                sheet.Cells["B2:D2"].Merge = true;
                sheet.Cells["H1:N1"].Merge = true;
                sheet.Cells["H2:N2"].Merge = true;
                sheet.Cells["H3:J3"].Merge = true;
                sheet.Cells["K3:M3"].Merge = true;

                sheet.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A1:E1"].Style.Font.Bold = true;
                sheet.Cells["A1:E1"].Style.Font.Size = 14;
                sheet.Cells["A1:E1"].Value = "OTD STORE";

                sheet.Cells["B2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["B2:D2"].Style.Font.Italic = true;
                sheet.Cells["B2:D2"].Style.Font.Size = 13;
                sheet.Cells["B2:D2"].Value = "BỘ PHẬN KẾ TOÁN";

                sheet.Cells["H1:N1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["H1:N1"].Style.Font.Bold = true;
                sheet.Cells["H1:N1"].Style.Font.Size = 11;
                sheet.Cells["H1:N1"].Value = "THỐNG KÊ DOANH THU BÁN HÀNG";

                sheet.Cells["H2:N2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["H2:N2"].Style.Font.Bold = true;
                sheet.Cells["H2:N2"].Style.Font.Size = 11;
                sheet.Cells["H2:N2"].Value = $"Từ {request.beginT.Date} đến {request.endT.Date}";

                sheet.Cells["H3:J3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["H3:J3"].Style.Font.Italic = true;
                sheet.Cells["H3:J3"].Style.Font.Size = 11;
                sheet.Cells["H3:J3"].Value = "Ngày giờ hệ thống";

                sheet.Cells["K3:M3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["K3:M3"].Style.Font.Size = 11;
                sheet.Cells["K3:M3"].Value = $"{dateht}";

                int count = 0;
                sheet.Row(5).Height = 30;
                sheet.Cells[$"C5:F5"].Merge = true;
                sheet.Cells[$"G5:I5"].Merge = true;
                sheet.Cells[$"J5:K5"].Merge = true;
                sheet.Cells[$"M5:N5"].Merge = true;
                sheet.Cells["A5:N5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A5:N5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A5:N5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A5:N5"].Style.Fill.BackgroundColor.SetColor(0, 46, 219, 254);
                sheet.Cells["A5:N5"].Style.Font.Bold = true;
                sheet.Cells["A5:N5"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A5:N5"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A5:N5"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A5:N5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A5:A5"].Value = "STT";
                sheet.Cells["B5:B5"].Value = "Mã";
                sheet.Cells["C5:F5"].Value = "Tên sản phẩm";
                sheet.Cells["G5:I5"].Value = "Thông số";
                sheet.Cells["J5:K5"].Value = "Giá bán";
                sheet.Cells["L5:L5"].Value = "Số lượng";
                sheet.Cells["M5:N5"].Value = "Thành tiền";
                sheet.Name = "Xuất doanh thu bán hàng";
                for (int h = 0; h < 100; h++)
                {
                    if (product[h] > 0)
                    {
                        count += 1;
                    }
                }

                if (count < 1)
                {
                    return View(request);
                }
                int i = 1;
                int j = 6;
                count = count + 6 - 1;
                string chuoi = $"A6:N{count}";
                var total = 0;
                if (count >= 1)
                {
                    sheet.Cells[chuoi].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[chuoi].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    for (int k = 0; k < 100; k++)
                    {
                        if (product[k] > 0)
                        {
                            var query1 = from p in _context.Products
                                         where p.Id == k
                                         select new
                                         {
                                             masp = p.Id,
                                             tensp = p.Name,
                                             mausp = p.CPU,
                                             dungluong = p.Memory,
                                             ram = p.RAM,
                                             gia = p.Price
                                         };
                            foreach (var item in query1)
                            {
                                sheet.Cells[$"C{j}:F{j}"].Merge = true;
                                sheet.Cells[$"G{j}:I{j}"].Merge = true;
                                sheet.Cells[$"J{j}:K{j}"].Merge = true;
                                sheet.Cells[$"M{j}:N{j}"].Merge = true;
                                sheet.Row(j).Height = 17;
                                string stt = $"A{j}:A{j}";
                                string masp = $"B{j}:B{j}";
                                string tensp = $"C{j}:F{j}";
                                string thongso = $"G{j}:I{j}";
                                string giaban = $"J{j}:K{j}";
                                string soluong = $"L{j}:L{j}";
                                string thanhtien = $"M{j}:N{j}";
                                sheet.Cells[stt].Value = i;
                                sheet.Cells[masp].Value = k;
                                sheet.Cells[tensp].Value = item.tensp;
                                sheet.Cells[thongso].Value = item.mausp + "/" + item.dungluong + "/" + item.ram;
                                sheet.Cells[giaban].Value = item.gia.ToString("0,0.") + "đ";
                                sheet.Cells[soluong].Value = product[k];
                                sheet.Cells[thanhtien].Value = (product[k] * item.gia).ToString("0,0.") + "đ";
                                total += product[k] * (int)item.gia;
                                i++;
                                j++;
                            }

                        }
                    }
                    count = count + 1;
                    sheet.Row(count).Height = 21;
                    sheet.Cells[$"A{count}:K{count}"].Merge = true;
                    sheet.Cells[$"L{count}:N{count}"].Merge = true;
                    sheet.Cells[$"A{count}:N{count}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[$"A{count}:N{count}"].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 125);
                    sheet.Cells[$"A{count}:N{count}"].Style.Font.Size = 17;
                    sheet.Cells[$"A{count}:N{count}"].Style.Font.Bold = true;
                    sheet.Cells[$"A{count}:K{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet.Cells[$"L{count}:N{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A{count}:K{count}"].Value = "DOANH THU:";
                    sheet.Cells[$"L{count}:N{count}"].Value = total.ToString("0,0.") + "đ";

                    count = count + 3;
                    sheet.Cells[$"H{count}:M{count}"].Merge = true;
                    sheet.Cells[$"H{count}:I{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"H{count}:I{count}"].Style.Font.Italic = true;
                    sheet.Cells[$"H{count}:I{count}"].Style.Font.Size = 11;
                    sheet.Cells[$"H{count}:I{count}"].Value = $"Thành Phố Hồ Chí Minh, ngày {dayht}, tháng {monthht}, năm {yearht}";

                    sheet.Cells[$"I{count + 1}:L{count + 1}"].Merge = true;
                    sheet.Cells[$"I{count + 1}:L{count + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"I{count + 1}:L{count + 1}"].Style.Font.Size = 11;
                    sheet.Cells[$"I{count + 1}:L{count + 1}"].Style.Font.Bold = true;
                    sheet.Cells[$"I{count + 1}:L{count + 1}"].Value = $"CỬA HÀNG TRƯỞNG";

                    sheet.Cells[$"I{count + 5}:L{count + 5}"].Merge = true;
                    sheet.Cells[$"I{count + 5}:L{count + 5}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"I{count + 5}:L{count + 5}"].Style.Font.Size = 11;
                    sheet.Cells[$"I{count + 5}:L{count + 5}"].Style.Font.Bold = true;
                    sheet.Cells[$"I{count + 5}:L{count + 5}"].Value = $"Nguyễn Thị Thúy Nga";

                    package.Save();
                }
            }
            stream.Position = 0;

            var tenfile = $"Doanh thu ban hang_{DateTime.Now}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", tenfile);
        }

        [HttpGet]
        public IActionResult Print()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Print(PrintRequest request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = new DataTable();
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var result = await _orderApiClient.GetById(request.Id);
                if (!result.IsSuccessed)
                {
                    TempData["result"] = "Không tìm thấy hóa đơn";
                    return RedirectToAction("Print");
                }
                var order = result.ResultObj;
                var dateht = DateTime.Now;
                var dayht = DateTime.Now.ToString("dd");
                var monthht = DateTime.Now.ToString("MM");
                var yearht = DateTime.Now.ToString("yyyy");


                int[] product = new int[100];

                foreach (var item in order.OrderDetails)
                {
                    product[item.ProductId] += item.Quantity;
                }
                string thanhtoan;

                if (order.PaymentMethod == "1")
                {
                    thanhtoan = "Thanh toán khi nhận hàng";
                }
                else
                {
                    thanhtoan = "Đã thanh toán";
                }

                var sheet = package.Workbook.Worksheets.Add("Hoa don");
                sheet.Cells["A1:M99"].Style.Font.Name = "Times New Roman";
                sheet.Cells["F1:G1"].Merge = true;
                sheet.Cells["A2:D2"].Merge = true;
                sheet.Cells["A3:E3"].Merge = true;
                sheet.Cells["A4:D4"].Merge = true;
                sheet.Cells["I2:L2"].Merge = true;
                sheet.Cells["I3:L3"].Merge = true;
                sheet.Cells["I4:L4"].Merge = true;
                sheet.Cells["E6:H6"].Merge = true;
                sheet.Cells["J7:L7"].Merge = true;

                sheet.Cells["F1:G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["F1:G1"].Style.Font.Bold = true;
                sheet.Cells["F1:G1"].Style.Font.Size = 15;
                sheet.Cells["F1:G1"].Value = "OTD STORE";

                sheet.Cells["A2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                sheet.Cells["A2:D2"].Style.Font.Size = 12;
                sheet.Cells["A2:D2"].Value = $"Khách hàng: {order.ShipName}";

                sheet.Cells["A3:E3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                sheet.Cells["A3:E3"].Style.Font.Size = 12;
                sheet.Cells["A3:E3"].Value = $"Địa chỉ: {order.ShipAddress}";

                sheet.Cells["A4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                sheet.Cells["A4:D4"].Style.Font.Size = 12;
                sheet.Cells["A4:D4"].Value = $"SĐT: {order.ShipPhoneNumber}";

                sheet.Cells["I2:L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells["I2:L2"].Style.Font.Size = 12;
                sheet.Cells["I2:L2"].Value = $"Ngày đặt hàng: {order.OrderDate}";

                sheet.Cells["I4:L4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells["I4:L4"].Style.Font.Size = 12;
                sheet.Cells["I4:L4"].Value = $"Thanh toán: {thanhtoan}";

                sheet.Cells["I3:L3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells["I3:L3"].Style.Font.Size = 12;
                sheet.Cells["I3:L3"].Value = $"Email: {order.ShipEmail}";

                sheet.Cells["E6:H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["E6:H6"].Style.Font.Size = 12;
                sheet.Cells["E6:H6"].Value = "HÓA ĐƠN BÁN HÀNG";

                sheet.Cells["J7:L7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["J7:L7"].Style.Font.Size = 12;
                sheet.Cells["J7:L7"].Value = $"{dateht}";

                int count = 0;
                sheet.Row(8).Height = 25;
                sheet.Cells["A8:D8"].Merge = true;
                sheet.Cells["E8:G8"].Merge = true;
                sheet.Cells["I8:J8"].Merge = true;
                sheet.Cells["K8:L8"].Merge = true;
                sheet.Cells["A8:L8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A8:L8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A8:L8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A8:L8"].Style.Fill.BackgroundColor.SetColor(0, 46, 219, 254);
                sheet.Cells["A8:L8"].Style.Font.Bold = true;
                sheet.Cells["A8:L8"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A8:L8"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A8:L8"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A8:L8"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A8:D8"].Value = "Sản phẩm";
                sheet.Cells["E8:G8"].Value = "Thông tin";
                sheet.Cells["H8:H8"].Value = "SL";
                sheet.Cells["I8:J8"].Value = "Giá bán";               
                sheet.Cells["K8:L8"].Value = "Thành tiền";
                sheet.Name = "Xuất hóa đơn";
                for (int h = 0; h < 100; h++)
                {
                    if (product[h] > 0)
                    {
                        count += 1;
                    }
                }

                int i = 1;
                int j = 9;
                count = count + 9 - 1;
                string chuoi = $"A9:L{count}";
                var total = 0;
                if (count >= 1)
                {
                    sheet.Cells[chuoi].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[chuoi].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[chuoi].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    for (int k = 0; k < 100; k++)
                    {
                        if (product[k] > 0)
                        {
                            var query1 = from p in _context.Products
                                         where p.Id == k
                                         select new
                                         {
                                             masp = p.Id,
                                             tensp = p.Name,
                                             mausp = p.CPU,
                                             dungluong = p.Memory,
                                             ram = p.RAM,
                                             gia = p.Price
                                         };
                            foreach (var item in query1)
                            {
                                sheet.Cells[$"A{j}:D{j}"].Merge = true;
                                sheet.Cells[$"E{j}:G{j}"].Merge = true;
                                sheet.Cells[$"I{j}:J{j}"].Merge = true;
                                sheet.Cells[$"K{j}:L{j}"].Merge = true;
                                sheet.Row(j).Height = 17;
                                string tensp = $"A{j}:D{j}";
                                string thongso = $"E{j}:G{j}";
                                string giaban = $"I{j}:J{j}";
                                string soluong = $"H{j}:H{j}";
                                string thanhtien = $"K{j}:L{j}";
                                sheet.Cells[tensp].Value = item.tensp;
                                sheet.Cells[thongso].Value = item.mausp + "/" + item.dungluong + "/" + item.ram;
                                sheet.Cells[giaban].Value = item.gia.ToString("0,0.") + "đ";
                                sheet.Cells[soluong].Value = product[k];
                                sheet.Cells[thanhtien].Value = (product[k] * item.gia).ToString("0,0.") + "đ";
                                total += product[k] * (int)item.gia;
                                i++;
                                j++;
                            }

                        }
                    }
                    count = count + 1;
                    sheet.Row(count).Height = 20;
                    sheet.Cells[$"A{count}:L{count}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[$"A{count}:L{count}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[$"A{count}:L{count}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[$"A{count}:L{count}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[$"A{count}:J{count}"].Merge = true;
                    sheet.Cells[$"K{count}:L{count}"].Merge = true;
                    sheet.Cells[$"A{count}:L{count}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[$"A{count}:L{count}"].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 125);
                    sheet.Cells[$"A{count}:L{count}"].Style.Font.Size = 15;
                    sheet.Cells[$"A{count}:L{count}"].Style.Font.Bold = true;
                    sheet.Cells[$"A{count}:J{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"K{count}:L{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A{count}:J{count}"].Value = "TỔNG CỘNG:";
                    sheet.Cells[$"K{count}:L{count}"].Value = total.ToString("0,0.") + "đ";

                    count = count + 1;
                    sheet.Cells[$"A{count}:C{count}"].Merge = true;
                    sheet.Cells[$"A{count}:C{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A{count}:C{count}"].Style.Font.Size = 12;
                    sheet.Cells[$"A{count}:C{count}"].Value = $"Số hóa đơn: {order.Id}";

                    count = count + 2;
                    sheet.Cells[$"E{count}:H{count}"].Merge = true;
                    sheet.Cells[$"E{count}:H{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"E{count}:H{count}"].Style.Font.Size = 12;
                    sheet.Cells[$"E{count}:H{count}"].Value = $"OTDStore xin cảm ơn!!!";

                    package.Save();
                }
            }
            stream.Position = 0;

            var tenfile = $"Hoa_don_{DateTime.Now}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", tenfile);
        }
    }
}
