using Microsoft.Reporting.WebForms;

namespace MvcReportViewer
{
    public interface IRenderReports
    {
        byte[] Render(IProvideReportConfiguration reportConfiguration);

        byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType);

        byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding);

        byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding, out string fileNameExtension);

        byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams);

        byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);
    }
}