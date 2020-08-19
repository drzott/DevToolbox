using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSG.KPI.Report.Util
{
    public static class DateUtil
    {
        /// <summary>
        /// Gibt einen Datumswert zurück der direkt im Report verwendet werden kann
        /// </summary>
        public static string GetReportDate(string dateRange)
        {
            if (string.IsNullOrEmpty(dateRange))
                return string.Empty;

            if (dateRange.ToLower() == "|aktuelles datum|")
                return DateTime.Now.ToString("dd.MM.yyyy");

            if (dateRange.ToLower() == "|aktuelles jahr|")
                return DateTime.Now.Year.ToString();

            if (dateRange.ToLower() == "|aktueller monat|")
                return DateTime.Now.Month.ToString();

            if (dateRange.ToLower() == "|aktuelles quartal|")
                return ((int)((DateTime.Now.Month + 2) / 3)).ToString();

            if (dateRange.ToLower() == "|vorjahr|")
                return (DateTime.Now.AddYears(-1)).Year.ToString();

            if (dateRange.ToLower() == "|letzter abgeschlossener monat|")
                return (DateTime.Now.AddMonths(-1)).Month.ToString();

            if (dateRange.ToLower() == "|vorletzter abgeschlossener monat|")
                return (DateTime.Now.AddMonths(-2)).Month.ToString();

            if (dateRange.ToLower() == "|letztes abgeschlossenes quartal|")
            {
                if (DateTime.Now.Month <= 3)
                    return "4";

                if (DateTime.Now.Month <= 6)
                    return "1";

                if (DateTime.Now.Month <= 9)
                    return "2";

                return "3";
            }

            if (dateRange.ToLower() == "|vorletztes abgeschlossenes quartal|")
            {
                if (DateTime.Now.Month <= 3)
                    return "3";

                if (DateTime.Now.Month <= 6)
                    return "4";

                if (DateTime.Now.Month <= 9)
                    return "1";

                return "2";
            }

            return string.Empty;
        }

        /// <summary>
        /// Gibt den auswertungszeitraum eines Report-Elements des KPI Designers zurück
        /// </summary>
        public static ReportRange GetReportRange(string reportFrom, string reportTo, string reportFromRange, int reportFromRange_int, DateTime reportDateFrom, DateTime reportDateTo)
        {
            if (string.IsNullOrEmpty(reportFrom) && string.IsNullOrEmpty(reportTo))
                return null;

            // Tag heute

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "tag heute" && reportTo.ToLower() == "tag heute")
                return new ReportRange(DateTime.Today, DateTime.Today);

            // festes Datum ist gesetzt

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "festes datum" && (!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "festes datum" && reportDateFrom != DateTime.MinValue && reportDateTo != DateTime.MinValue)
            {
                // unsinnig eingabe
                if (reportDateFrom > reportDateTo)
                    return null;

                return new ReportRange(reportDateFrom, reportDateTo);
            }

            // Kombination aus Tag heute und festes Datum

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "festes datum" && (!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "tag heute" && reportDateFrom != DateTime.MinValue)
            {
                if (reportDateFrom > DateTime.Today)
                    return null;

                return new ReportRange(reportDateFrom, DateTime.Today);
            }

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "tag heute" && (!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "festes datum" && reportDateTo != DateTime.MinValue)
            {
                if (DateTime.Today > reportDateTo)
                    return null;

                return new ReportRange(DateTime.Today, reportDateTo);
            }

            // abgeschlossene Zeiträume

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letzter abgeschlossener monat")
            {
                DateTime from = FirstDayOfPreviousMonth(DateTime.Today);
                DateTime to = LastDayOfPreviousMonth(DateTime.Today);

                return new ReportRange(from, to);
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletzter abgeschlossener monat")
            {
                DateTime from = FirstDayOfPrePreviousMonth(DateTime.Today);
                DateTime to = LastDayOfPrePreviousMonth(DateTime.Today);

                return new ReportRange(from, to);
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letztes abgeschlossenes quartal")
            {
                DateTime dto = LastDayOfPreviousMonth(DateTime.Today);
                dto.AddDays(1);
                if (dto.Month > 9)
                {
                    DateTime from = new DateTime(dto.Year, 7, 1);
                    DateTime to = new DateTime(dto.Year, 9, 30);

                    return new ReportRange(from, to);
                }

                if (dto.Month > 6)
                {
                    DateTime from = new DateTime(dto.Year, 4, 1);
                    DateTime to = new DateTime(dto.Year, 6, 30);

                    return new ReportRange(from, to);
                }

                if (dto.Month > 3)
                {
                    DateTime from = new DateTime(dto.Year, 1, 1);
                    DateTime to = new DateTime(dto.Year, 3, 31);

                    return new ReportRange(from, to);
                }

                if (dto.Month <= 3)
                {
                    DateTime from = new DateTime(dto.Year - 1, 10, 1);
                    DateTime to = new DateTime(dto.Year - 1, 12, 31);

                    return new ReportRange(from, to);
                }

                return null;
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletztes abgeschlossenes quartal")
            {
                DateTime dto = LastDayOfPreviousMonth(DateTime.Today);

                if (dto.Month > 9)
                {
                    DateTime from = new DateTime(dto.Year, 4, 1);
                    DateTime to = new DateTime(dto.Year, 6, 30);

                    return new ReportRange(from, to);
                }

                if (dto.Month > 6)
                {
                    DateTime from = new DateTime(dto.Year, 1, 1);
                    DateTime to = new DateTime(dto.Year, 3, 31);

                    return new ReportRange(from, to);
                }

                if (dto.Month > 3)
                {
                    DateTime from = new DateTime(dto.Year -1 , 10, 1);
                    DateTime to = new DateTime(dto.Year -1, 12, 31);

                    return new ReportRange(from, to);
                }

                if (dto.Month <= 3)
                {
                    DateTime from = new DateTime(dto.Year - 1, 7, 1);
                    DateTime to = new DateTime(dto.Year - 1, 9, 30);

                    return new ReportRange(from, to);
                }

                return null;
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letztes abgeschlossenes halbjahr")
            {
                DateTime dto = LastDayOfPreviousMonth(DateTime.Today);

                if (dto.Month > 6)
                {
                    DateTime from = new DateTime(dto.Year, 1, 1);
                    DateTime to = new DateTime(dto.Year, 6, 30);

                    return new ReportRange(from, to);
                }
                else
                {
                    DateTime from = new DateTime(dto.Year - 1, 7, 1);
                    DateTime to = new DateTime(dto.Year - 1, 12, 31);

                    return new ReportRange(from, to);
                }
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletztes abgeschlossenes halbjahr")
            {
                DateTime dto = LastDayOfPreviousMonth(DateTime.Today);

                if (dto.Month > 6)
                {
                    DateTime from = new DateTime(dto.Year -1, 7, 1);
                    DateTime to = new DateTime(dto.Year -1, 12, 31);

                    return new ReportRange(from, to);
                }
                else
                {
                    DateTime from = new DateTime(dto.Year - 1, 1, 1);
                    DateTime to = new DateTime(dto.Year - 1, 6, 30);

                    return new ReportRange(from, to);
                }
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letztes abgeschlossenes jahr")
            {
                DateTime from = new DateTime(DateTime.Today.Year - 1, 1, 1);
                DateTime to = new DateTime(DateTime.Today.Year - 1, 12, 31);

                return new ReportRange(from, to);
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletztes abgeschlossenes jahr")
            {
                DateTime from = new DateTime(DateTime.Today.Year - 2, 1, 1);
                DateTime to = new DateTime(DateTime.Today.Year - 2, 12, 31);

                return new ReportRange(from, to);
            }

            // Zeitraum

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "zeitraum"
                    && (!string.IsNullOrEmpty(reportFromRange))
                    && (reportFromRange.ToLower() == "jahr(e)" || reportFromRange.ToLower() == "monat(e)" || reportFromRange.ToLower() == "woche(n)" || reportFromRange.ToLower() == "tag(e)")
                    && (!string.IsNullOrEmpty(reportTo))
                    && (reportTo.ToLower() == "tag heute" || reportTo.ToLower() == "festes datum")
               )
            {
                DateTime to = DateTime.Today;
                DateTime from = DateTime.Today;

                if (reportTo.ToLower() == "festes datum")
                    to = reportDateTo;

                if (reportFromRange.ToLower() == "jahr(e)")
                    from = from.AddYears(-reportFromRange_int);

                if (reportFromRange.ToLower() == "monat(e)")
                    from = from.AddMonths(-reportFromRange_int);

                if (reportFromRange.ToLower() == "woche(n)")
                    from = from.AddDays(-(reportFromRange_int * 7));

                if (reportFromRange.ToLower() == "tag(n)")
                    from = from.AddDays(-(reportFromRange_int));

                return new ReportRange(from, to);
            }

            return null;
        }

        public static string GetReportRangeText(string reportFrom, string reportTo, string reportFromRange, int reportFromRange_int, DateTime reportDateFrom, DateTime reportDateTo)
        {
            if (string.IsNullOrEmpty(reportFrom) && string.IsNullOrEmpty(reportTo))
                return string.Empty;

            // Tag heute

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "tag heute" && reportTo.ToLower() == "tag heute")
                return "Tag heute - Tag heute";

            // festes Datum ist gesetzt

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "festes datum" && (!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "festes datum" && reportDateFrom != DateTime.MinValue && reportDateTo != DateTime.MinValue)
            {
                // unsinnig eingabe
                if (reportDateFrom > reportDateTo)
                    return string.Empty;

                return reportDateFrom.ToShortDateString() + "-" + reportDateTo.ToShortDateString();
            }

            // Kombination aus Tag heute und festes Datum

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "festes datum" && (!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "tag heute" && reportDateFrom != DateTime.MinValue)
            {
                if (reportDateFrom > DateTime.Today)
                    return null;

                return reportDateFrom.ToShortDateString() + "- Tag heute";
            }

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "tag heute" && (!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "festes datum" && reportDateTo != DateTime.MinValue)
            {
                if (DateTime.Today > reportDateTo)
                    return null;

                return "Tag heute - " + reportDateTo.ToShortDateString();
            }

            // abgeschlossene Zeiträume

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letzter abgeschlossener monat")
            {
                return "letzter abgeschlossener Monat";
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letztes abgeschlossenes quartal")
            {
                return "letztes abgeschlossenes Quartal";
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letztes abgeschlossenes halbjahr")
            {
                return "letztes abgeschlossenes Halbjahr";
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "letztes abgeschlossenes jahr")
            {
                return "letztes abgeschlossenes Jahr";
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletzter abgeschlossener monat")
            {
                return "vorletzter abgeschlossener monat";
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletztes abgeschlossenes quartal")
            {
                return "vorletztes abgeschlossenes quartal";
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletztes abgeschlossenes halbjahr")
            {
                return "vorletztes abgeschlossenes halbjahr";
            }

            if ((!string.IsNullOrEmpty(reportTo)) && reportTo.ToLower() == "vorletztes abgeschlossenes jahr")
            {
                return "vorletztes abgeschlossenes jahr";
            }

            // Zeitraum

            if ((!string.IsNullOrEmpty(reportFrom)) && reportFrom.ToLower() == "zeitraum"
                    && (!string.IsNullOrEmpty(reportFromRange))
                    && (reportFromRange.ToLower() == "jahr(e)" || reportFromRange.ToLower() == "monat(e)" || reportFromRange.ToLower() == "woche(n)" || reportFromRange.ToLower() == "tag(e)")
                    && (!string.IsNullOrEmpty(reportTo))
                    && (reportTo.ToLower() == "tag heute" || reportTo.ToLower() == "festes datum")
               )
            {

                string r = "Tag heute";

                if (reportTo.ToLower() == "festes datum")
                    r = reportDateTo.ToShortDateString();

                if (reportFromRange.ToLower() == "jahr(e)")
                    r += " - " + reportFromRange_int + " Jahre(e)";

                if (reportFromRange.ToLower() == "monat(e)")
                    r += " - " + reportFromRange_int + " Monat(e)";

                if (reportFromRange.ToLower() == "woche(n)")
                    r += " - " + reportFromRange_int + " Woche(n)";

                if (reportFromRange.ToLower() == "tag(n)")
                    r += " - " + reportFromRange_int + " Tag(e)";

                return r;
            }

            return string.Empty;
        }

        public static DateTime LastDayOfPreviousMonth(DateTime dto)
        {
            return dto.AddDays(-dto.Day);
        }

        public static DateTime LastDayOfPrePreviousMonth(DateTime dto)
        {
            return ((FirstDayOfPrePreviousMonth(dto)).AddMonths(1)).AddDays(-1);
        }

        public static DateTime FirstDayOfPrePreviousMonth(DateTime dto)
        {
            return (FirstDayOfPreviousMonth(dto)).AddMonths(-1);
        }

        public static DateTime FirstDayOfPreviousMonth(DateTime dto)
        {
            DateTime lastDayPrevMonth = dto.AddDays(-dto.Day);

            return new DateTime(lastDayPrevMonth.Year, lastDayPrevMonth.Month, 1);
        }
    }
    
    public class ReportRange
    {
        public DateTime From = DateTime.MinValue;
        public DateTime To = DateTime.MinValue;

        public ReportRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
