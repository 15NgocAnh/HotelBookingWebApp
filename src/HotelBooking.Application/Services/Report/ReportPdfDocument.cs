using HotelBooking.Application.CQRS.Statistic.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

namespace HotelBooking.Application.Services.Report;

public class ReportPdfDocument : IDocument
{
    private readonly BookingStatisticsDto _booking;
    private readonly RevenueStatisticsDto _revenue;
    private readonly RoomStatisticsDto _room;

    public ReportPdfDocument(
        BookingStatisticsDto booking,
        RevenueStatisticsDto revenue,
        RoomStatisticsDto room)
    {
        _booking = booking;
        _revenue = revenue;
        _room = room;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer doc)
    {
        byte[]? revenueChart = null;
        try
        {
            revenueChart = BuildRevenueChart();   // PNG bytes
        }
        catch (Exception ex)
        {
            // Log error nếu cần
            Console.WriteLine($"Chart generation failed: {ex.Message}");
        }

        doc.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(TextStyle.Default.FontSize(11));
            page.Content().Column(column =>
            {
                column.Item().Text("Hotel Statistics Report")
                             .FontSize(20).SemiBold().FontColor(Colors.Blue.Darken4)
                             .AlignCenter();

                column.Item().PaddingVertical(4)
                             .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                /* ---------- SUMMARY GRID ---------- */
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Column(SummaryCard("Total Bookings", _booking?.TotalBookings));
                    grid.Item().Column(SummaryCard("Completed Bookings", _booking?.CompletedBookings));

                    grid.Item().Column(SummaryCard("Total Revenue", _revenue?.TotalRevenue, "C0"));
                    grid.Item().Column(SummaryCard("Monthly Revenue", _revenue?.TotalMonthlyRevenue, "C0"));

                    grid.Item().Column(SummaryCard("Total Rooms", _room?.TotalRooms));
                    grid.Item().Column(SummaryCard("Available Rooms", _room?.AvailableRooms));
                });

                /* ---------- SECTION: BOOKING DETAILS ---------- */
                column.Item().Element(SectionHeading("Booking Statistics"));
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn();           // label
                        c.ConstantColumn(60);         // value
                    });

                    Row("Pending", _booking?.PendingBookings);
                    Row("Cancelled", _booking?.CancelledBookings);

                    void Row(string label, object? value)
                    {
                        table.Cell().Element(CellLabel(label));
                        table.Cell().AlignRight().Text(value?.ToString() ?? "—");
                    }
                    Action<IContainer> CellLabel(string label)
                        => c => c.Text(label);
                });

                /* ---------- SECTION: REVENUE CHART ---------- */
                if (revenueChart != null && revenueChart.Length > 0)
                {
                    column.Item().Element(SectionHeading("Monthly Revenue (VNĐ)"));
                    column.Item().AlignCenter().Height(220).Image(revenueChart);
                }

                /* ---------- FOOTER ---------- */
                column.Item().PaddingTop(10)
                             .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                column.Item().AlignCenter().Text(t =>
                {
                    t.Span("Generated: ");
                    t.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                     .SemiBold();
                });
            });
        });
    }

    /*──────────────────────────────────────────────────────────────*/
    /*  Helpers                                                    */
    /*──────────────────────────────────────────────────────────────*/

    private Action<ColumnDescriptor> SummaryCard(string title, object? value, string? format = null)
        => col =>
        {
            col.Spacing(2);
            col.Item().Text(title).FontSize(10).FontColor(Colors.Grey.Darken2);
            col.Item().Text(format is null
                                ? value?.ToString() ?? "—"
                                : string.Format($"{{0:{format}}}", value ?? 0))
                     .FontSize(14).SemiBold();
        };

    private Action<IContainer> SectionHeading(string text)
        => container => container
            .PaddingTop(8)
            .Text(text).FontSize(12).SemiBold().FontColor(Colors.Blue.Darken3);

    private byte[]? BuildRevenueChart()
    {
        try
        {
            var months = _revenue?.MonthlyRevenue?.Select(m => m.Month).ToArray() ?? Array.Empty<string>();
            var values = _revenue?.MonthlyRevenue?.Select(m => (double)m.Revenue).ToArray() ?? Array.Empty<double>();

            // Kiểm tra dữ liệu
            if (months.Length == 0 || values.Length == 0)
            {
                return null;
            }

            var plot = new PlotModel
            {
                PlotMargins = new OxyThickness(50, 10, 10, 40),
                Background = OxyColors.White
            };

            // --- Trục X ---
            var catAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                GapWidth = 0.2,
                Angle = 45,
                FontSize = 9
            };
            catAxis.Labels.AddRange(months);
            plot.Axes.Add(catAxis);

            // --- Trục Y ---
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                StringFormat = "###,###",
                FontSize = 9,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            // --- Series ---
            var series = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                StrokeThickness = 2,
                Color = OxyColors.Blue
            };

            // Sử dụng LineSeries thay vì BarSeries để tránh lỗi
            for (int i = 0; i < values.Length; i++)
            {
                series.Points.Add(new DataPoint(i, values[i]));
            }

            plot.Series.Add(series);

            // --- Xuất PNG ---
            using var stream = new MemoryStream();
            var exporter = new PngExporter
            {
                Width = 600,
                Height = 220,
            };
            exporter.Export(plot, stream);

            var result = stream.ToArray();
            return result.Length > 0 ? result : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chart error: {ex.Message}");
            return null;
        }
    }
}