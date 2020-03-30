﻿//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// this is a structured log item, with the additional restriction
    /// that it is intended for human consumption, so long messages
    /// can be truncated and implementation-related messages are discouraged
    /// </summary>
    public class StatusReportItem
    {
        /// <summary>
        /// optional time stamp or null
        /// </summary>
        public string TimeStamp { get; set; }

        public enum SeverityCode
        {
            Info,
            Warning,
            Error,
            None // no messages should be created at this level, in it used to filter out all messages
        }
        public SeverityCode Severity { get; set; } = SeverityCode.Info;
        
        /// <summary>
        /// if set, this indicates this message is too numerous for display in a short
        /// status display.  It will usually only be set on Info messages.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// the status message, which may be more than one line long
        /// but should generally be short
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// a recommendation to the user or null
        /// </summary>
        public string Recommendation { get; set; }

        /// <summary>
        /// log this result
        /// </summary>
        /// <param name="logManager"></param>
        public void Log(LogManager logManager)
        {
            switch (Severity)
            {
                case SeverityCode.None:
                    throw new System.Exception($"Severity 'None' must not be used for any status report instances; implementation error");
                case SeverityCode.Info:
                    logManager.LogInfo(Status);
                    if (Recommendation != null)
                    {
                        logManager.LogInfo(Recommendation);
                    }
                    break;
                case SeverityCode.Warning:
                    logManager.LogWarning(Status);
                    if (Recommendation != null)
                    {
                        logManager.LogWarning(Recommendation);
                    }
                    break;
                case SeverityCode.Error:
                    logManager.LogError(Status);
                    if (Recommendation != null)
                    {
                        logManager.LogError(Recommendation);
                    }
                    break;
            }
        }
    }
}