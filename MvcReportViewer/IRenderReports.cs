namespace MvcReportViewer
{
    public interface IRenderReports
    {
        byte[] Render(IProvideReportConfiguration reportConfiguration);

        byte[] Render(IProvideReportConfiguration reportConfiguration, out string mimeType);
    }
}