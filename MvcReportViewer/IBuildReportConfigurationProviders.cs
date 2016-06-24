namespace MvcReportViewer
{
    public interface IBuildReportConfigurationProviders
    {
        IProvideReportConfiguration BuildConfigurationProvider();
    }
}