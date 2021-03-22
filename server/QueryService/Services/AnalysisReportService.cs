using System;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Queries;
using Queries.Enums;

namespace QueryService.Services
{
    public class AnalysisReportService : IReportService<AnalysisFile>
    {
        public byte[] GenerateReport(AnalysisFile obj, params object[] parameters)
        {
            var document = new Document();
            var ms = new MemoryStream();

            try
            {
                var pdfWriter = PdfWriter.GetInstance(document, ms);
                pdfWriter.CloseStream = false;

                document.Open();
                document.AddTitle("Analysis report");

                CreateDocumentHeader(document, obj);
                CreateDocumentBody(document, obj);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            finally
            {
                document.Close();
            }

            ms.Flush();
            return ms.ToArray();
        }

        private void CreateDocumentBody(IElementListener document, AnalysisFile analysisFile)
        {
            var table = new PdfPTable(2)
            {
                SpacingBefore = 25.0f
            };
            table.SetWidths(new[] {1, 2});

            AddRow(table, "File name", analysisFile.FileName);
            AddRow(table, "Performed by", analysisFile.UserId.ToString());
            AddRow(table, "Analysis status", Enum.GetName(analysisFile.Status));
            AddRow(table, "Status change date", analysisFile.Date.ToString(CultureInfo.InvariantCulture));
            if (analysisFile.Status == OperationStatus.Complete) AddHighlightedResultRow(table, analysisFile.Result);

            document.Add(table);
        }

        private void AddHighlightedResultRow(PdfPTable table, double result)
        {
            table.AddCell(new PdfPCell(new Phrase("Analysis result"))
            {
                BackgroundColor = BaseColor.LIGHT_GRAY,
                MinimumHeight = 25,
                VerticalAlignment = Element.ALIGN_CENTER,
                PaddingLeft = 10
            });

            table.AddCell(new PdfPCell(new Phrase($"{result.ToString(CultureInfo.InvariantCulture)}%"))
            {
                MinimumHeight = 25,
                VerticalAlignment = Element.ALIGN_CENTER,
                PaddingLeft = 10,
                BackgroundColor = result switch
                {
                    <= 30 => BaseColor.GREEN,
                    <= 50 => BaseColor.YELLOW,
                    _ => BaseColor.RED
                }
            });
        }

        private static void AddRow(PdfPTable table, string title, string value)
        {
            table.AddCell(new PdfPCell(new Phrase(title))
            {
                BackgroundColor = BaseColor.LIGHT_GRAY,
                MinimumHeight = 25,
                VerticalAlignment = Element.ALIGN_CENTER,
                PaddingLeft = 10
            });
            table.AddCell(new PdfPCell(new Phrase(value))
            {
                MinimumHeight = 25,
                VerticalAlignment = Element.ALIGN_CENTER,
                PaddingLeft = 10
            });
        }

        private void CreateDocumentHeader(IElementListener document, AnalysisFile file)
        {
            document.Add(new Paragraph(
                $"Print date: {DateTime.Now.ToString(CultureInfo.InvariantCulture)}",
                FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.GRAY))
            {
                Alignment = Element.ALIGN_RIGHT
            });
            document.Add(CreateTitle());
            document.Add(CreateSubtitle(file.Id));
        }

        private IElement CreateTitle()
        {
            var title = new Paragraph(
                "Plagiarism report",
                FontFactory.GetFont("Arial", 23)
            )
            {
                Alignment = Element.ALIGN_CENTER
            };
            return title;
        }

        private IElement CreateSubtitle(Guid id)
        {
            var title = new Paragraph($"Analysis id: {id}",
                FontFactory.GetFont("Arial", 12, Font.ITALIC, BaseColor.GRAY))
            {
                Alignment = Element.ALIGN_CENTER
            };
            return title;
        }
    }
}