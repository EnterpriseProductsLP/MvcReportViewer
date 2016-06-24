using System;

namespace MvcReportViewer
{
    public static class ReportViewerEventsHandlerExtensions
    {
        internal static IReportViewerEventsHandler CreateEventHandlers(this ReportViewerParameters parameters)
        {
            return CreateEventHandlers(parameters.EventsHandlerType);
        }

        internal static IReportViewerEventsHandler CreateEventHandlers(string eventsHandlerType)
        {
            if (String.IsNullOrEmpty(eventsHandlerType))
            {
                return null;
            }

            var handlersType = Type.GetType(eventsHandlerType);
            if (handlersType == null)
            {
                throw new MvcReportViewerException($"Type {eventsHandlerType} is not found");
            }

            var handlers = Activator.CreateInstance(handlersType) as IReportViewerEventsHandler;
            if (handlers == null)
            {
                throw new MvcReportViewerException(
                    $"Type {eventsHandlerType} has not implemented IReportViewerEventsHandler interface or cannot be instantiated.");
            }

            return handlers;
        }
    }
}