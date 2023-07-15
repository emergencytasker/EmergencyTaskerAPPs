using System;
using System.Collections.Generic;
using System.Linq;

namespace EmergencyTask.ViewModel.Business
{
    public class CostCalculator
    {
        public List<Report> Calculate(DateTime start, DateTime end, double tarifa1, double tarifa2)
        {
            var nextday = start.AddDays(1);

            var schedules = new List<Schedule>
            {
                new Schedule { Type = 2, Id = 0, Cost = tarifa2, Start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0), End = new DateTime(start.Year, start.Month, start.Day, 7, 59, 0) },
                new Schedule { Type = 1, Id = 1, Cost = tarifa1, Start = new DateTime(start.Year, start.Month, start.Day, 8, 0, 0), End = new DateTime(start.Year, start.Month, start.Day, 19, 59, 0) },
                new Schedule { Type = 2, Id = 2, Cost = tarifa2, Start = new DateTime(start.Year, start.Month, start.Day, 20, 0, 0), End = new DateTime(start.Year, start.Month, start.Day, 23, 59, 0) },
                new Schedule { Type = 2, Id = 3, Cost = tarifa2, Start = new DateTime(nextday.Year, nextday.Month, nextday.Day, 0, 0, 0), End = new DateTime(nextday.Year, nextday.Month, nextday.Day, 7, 59, 0) },
                new Schedule { Type = 1, Id = 4, Cost = tarifa1, Start = new DateTime(nextday.Year, nextday.Month, nextday.Day, 8, 0, 0), End = new DateTime(nextday.Year, nextday.Month, nextday.Day, 19, 59, 0) },
                new Schedule { Type = 1, Id = 5, Cost = tarifa2, Start = new DateTime(nextday.Year, nextday.Month, nextday.Day, 20, 0, 0), End = new DateTime(nextday.Year, nextday.Month, nextday.Day, 23, 59, 0) }
            };

            List<Report> reports = new List<Report>();
            for (DateTime time = start; time < end; time = time.AddSeconds(1))
            {
                var schedule = schedules.FirstOrDefault(i => i.Start.Ticks <= time.Ticks && time.Ticks <= i.End.Ticks);
                if (schedule == null) continue;

                var report = reports.FirstOrDefault(r => r.IdSchedule == schedule.Id);
                if (report == null)
                {
                    reports.Add(new Report
                    {
                        IdSchedule = schedule.Id,
                        Cost = schedule.Cost,
                        Tarifa = schedule.Cost,
                        Seconds = 1,
                        Type = schedule.Type,
                        Start = time
                    });
                }
                else
                {
                    report.Seconds += 1;
                }
            }

            foreach (var report in reports)
                report.Cost = Calculate(report.Seconds, report.Tarifa);

            return reports;
        }

        /// <summary>
        /// Get Time From Cost
        /// </summary>
        /// <param name="costofinal"></param>
        /// <param name="costo"></param>
        /// <returns></returns>
        public TimeSpan Inverse(double costofinal, double tarifa)
        {
            var times = 0D;
            if(tarifa == 100D)
            {
                times = costofinal / 25D;
            }
            else if(tarifa == 200D)
            {
                times = costofinal / 50D;
            }
            return TimeSpan.FromMinutes(times * 15);
        }

        /// <summary>
        /// Get Cost From Time
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="tarifa"></param>
        /// <returns></returns>
        public double Calculate(double minutes, double tarifa)
        {
            var times = (minutes / 60) / 15D;
            var entero = (int)times;
            var restante = 1D - (times - entero);
            if(restante != 1) times += restante;
            var cost = 0D;
            if (tarifa == 100D)
            {
                cost = times * 25D;
            }
            else if (tarifa == 200D)
            {
                cost = times * 50D;
            }
            return cost;
        }
    }

    public class ScheduleSource
    {
        public int Id { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public double Cost { get; set; }

        public ScheduleSource(int id, TimeSpan start, TimeSpan end, double cost)
        {
            Id = id;
            Start = start;
            End = end;
            Cost = cost;
            if (Start == End) throw new Exception("ScheduleSource, Start & End are equals");
        }

        public bool NewDay
        {
            get
            {
                return Start.TotalHours > End.TotalHours;
            }
        }
    }

    public class Schedule
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Cost { get; set; }
        public int Type { get; internal set; }
    }

    public class Report
    {
        public int IdSchedule { get; set; }
        public int Seconds { get; set; }
        public double Cost { get; set; }

        public TimeSpan Time
        {
            get
            {
                return TimeSpan.FromMinutes(Seconds);
            }
        }

        public int Type { get; set; }
        public double Tarifa { get; set; }
        public DateTime Start { get; set; }

        public override string ToString()
        {
            return $"Tiempo: {Time}, Minutes: {Seconds}, Cost: {Cost}";
        }
    }
}
