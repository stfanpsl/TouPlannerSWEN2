using log4net;
using log4net.Config;

namespace TourPlannerSemesterProjekt.Logging
{
    public interface ILoggerWrapper
    {
        void Debug(string message);
        void Error(string message);
        void Fatal(string message);
        void Warn(string message);
    }
}
