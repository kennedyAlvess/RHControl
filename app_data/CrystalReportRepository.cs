using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;

namespace RHControl.Repository
{
    public static class CrystalReportRepository
    {
        public static void SetReportConnection(ReportDocument report)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;

            var builder = new System.Data.Common.DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            string servidor = builder["Data Source"].ToString();
            string usuario = builder["User Id"].ToString();
            string senha = builder["Password"].ToString();

            ConnectionInfo connInfo = new ConnectionInfo
            {
                ServerName = servidor,
                UserID = usuario,
                Password = senha,
                Type = ConnectionInfoType.SQL
            };

            foreach (Table table in report.Database.Tables)
            {
                TableLogOnInfo logon = table.LogOnInfo;
                logon.ConnectionInfo = connInfo;
                table.ApplyLogOnInfo(logon);
                table.Location = table.Location;
            }
        }
    }
}