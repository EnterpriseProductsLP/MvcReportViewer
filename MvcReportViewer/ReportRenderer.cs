using Microsoft.Reporting.WebForms;

namespace MvcReportViewer
{
    public class ReportRenderer : IRenderReports
    {
        public byte[] Render(IProvideReportConfiguration reportConfiguration)
        {
            string mimeType;
            return Render(reportConfiguration, out mimeType);
        }

        public byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType)
        {
            string encoding;
            return Render(reportConfiguration, out mimeType, out encoding);
        }

        public byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding)
        {
            string fileNameExtension;
            return Render(reportConfiguration, out mimeType, out encoding, out fileNameExtension);
        }

        public byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding, out string fileNameExtension)
        {
            string[] streams;
            return Render(reportConfiguration, out mimeType, out encoding, out fileNameExtension, out streams);
        }

        public byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams)
        {
            Warning[] warnings;
            return Render(reportConfiguration, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }

        public byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings)
        {
            var reportRunner = new ReportRunner(reportConfiguration);
            return reportRunner.Render(out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }
    }
}