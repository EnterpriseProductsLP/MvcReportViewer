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
            var reportRunner = new ReportRunner(reportConfiguration);
            return reportRunner.Render(out mimeType);
        }
    }
}