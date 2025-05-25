using HotelBooking.Application.CQRS.Statistic.Dtos;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HotelBooking.Application.Services.Report
{
    public class ReportExcelDocument
    {
        private readonly BookingStatisticsDto _booking;
        private readonly RevenueStatisticsDto _revenue;
        private readonly RoomStatisticsDto _room;

        public ReportExcelDocument(BookingStatisticsDto booking, RevenueStatisticsDto revenue, RoomStatisticsDto room)
        {
            _booking = booking;
            _revenue = revenue;
            _room = room;
        }

        public byte[] Generate()
        {
            // Create Excel package
            using var package = new ExcelPackage();

            var ws = package.Workbook.Worksheets.Add("Statistics Report");

            int row = 1;

            void SectionTitle(string title)
            {
                ws.Cells[row, 1].Value = title;
                ws.Cells[row, 1, row, 5].Merge = true;
                ws.Cells[row, 1].Style.Font.Bold = true;
                ws.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                ws.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                row++;
            }

            void LabelValue(string label, object? value)
            {
                ws.Cells[row, 1].Value = label;
                ws.Cells[row, 2].Value = value;
                row++;
            }

            // Booking Statistics
            SectionTitle("Booking Statistics");
            LabelValue("Total Bookings", _booking?.TotalBookings);
            LabelValue("Completed Bookings", _booking?.CompletedBookings);
            LabelValue("Pending Bookings", _booking?.PendingBookings);
            LabelValue("Cancelled Bookings", _booking?.CancelledBookings);
            row++;

            // Revenue Statistics
            SectionTitle("Revenue Statistics");
            LabelValue("Total Revenue", _revenue?.TotalRevenue);
            LabelValue("Monthly Revenue", _revenue?.TotalMonthlyRevenue);
            LabelValue("Weekly Revenue", _revenue?.TotalWeeklyRevenue);
            row++;

            // Room Statistics
            SectionTitle("Room Statistics");
            LabelValue("Total Rooms", _room?.TotalRooms);
            LabelValue("Available Rooms", _room?.AvailableRooms);
            LabelValue("Booked Rooms", _room?.BookedRooms);
            LabelValue("Cleaning Up Rooms", _room?.CleaningUpRooms);
            LabelValue("Maintenance Rooms", _room?.MaintenanceRooms);
            row++;

            // Daily Bookings Table
            SectionTitle("Daily Bookings");
            ws.Cells[row, 1].Value = "Date";
            ws.Cells[row, 2].Value = "Count";
            ws.Row(row).Style.Font.Bold = true;
            row++;

            foreach (var b in _booking?.DailyBookings ?? [])
            {
                ws.Cells[row, 1].Value = b.Date.ToString("dd/MM/yyyy");
                ws.Cells[row, 2].Value = b.Count;
                row++;
            }
            row++;

            // Monthly Revenue Table
            SectionTitle("Monthly Revenue");
            ws.Cells[row, 1].Value = "Month";
            ws.Cells[row, 2].Value = "Revenue";
            ws.Row(row).Style.Font.Bold = true;
            row++;

            foreach (var r in _revenue?.MonthlyRevenue ?? [])
            {
                ws.Cells[row, 1].Value = r.Month;
                ws.Cells[row, 2].Value = r.Revenue;
                ws.Cells[row, 2].Style.Numberformat.Format = "#,##0.00 ₫";
                row++;
            }

            ws.Cells.AutoFitColumns();
            package.Workbook.Calculate();

            return package.GetAsByteArray();
        }
    }
}
