using DbAccess;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DbQuery
{
    [CLSCompliant(false)]
    public class WriteBatterySnInDmsFromMes : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Task task = new Task(() =>
            {
                Db cfdms = new Db(ConfigurationManager.ConnectionStrings["CFDMSDB"].ToString(), ConnectionType.Sql);
                try
                {
                    SqlDataReader reader = cfdms.GetDataReader("select top 500 vin from hp_vehicle where classid in('e1','e2') and querybatterysndate = '1990-1-1'", true);
                    if (reader.HasRows)
                    {
                        string vins = "";
                        int vins_count = 0;
                        while (reader.Read())
                        {
                            vins += "'" + reader[0].ToString() + "',";
                            vins_count++;
                        }
                        vins = vins.Remove(vins.Length - 1);

                        string sql = File.ReadSql("ReadVehcileBatteryInfo");
                        sql = sql.Replace("@VIN", vins);

                        Db mesdb = new Db(ConfigurationManager.ConnectionStrings["MesDb"].ToString(), ConnectionType.Oracle);
                        FileStream fs = null;
                        StreamWriter sw = null;
                        try
                        {
                            OracleDataReader _reader = mesdb.GetDataReader(sql);
                            sql = "";
                            string str = "";
                            int count = 0;
                            fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\UpdatedLogs\\" + DateTime.Now.ToString("yy-MM-dd HH.mm.ss") + ".txt", FileMode.Create, FileAccess.Write);
                            sw = new StreamWriter(fs);
                            if (_reader.HasRows)
                            {
                                sw.WriteLine(Environment.NewLine);  //换行
                                while (_reader.Read())
                                {
                                    str = "update hp_vehicle set primarybatterysn='" + _reader["PrimaryBatterySN"].ToString() +
                                    "',sparebatterysn='" + _reader["SpareBatterySN"].ToString() + "',isnewenergy=1,querybatterysndate=getdate() where vin='" +
                                    _reader["VIN"].ToString() + "';";

                                    vins = vins.Replace("'" + _reader["VIN"].ToString() + "',", "");
                                    sql += str;
                                    sw.WriteLine(str);
                                }
                                count = cfdms.Command(sql);
                                sw.WriteLine("query total " + vins_count + " record, and updated " + count + " records success from mesdb");
                                sw.WriteLine(Environment.NewLine);
                                sw.WriteLine("================================================================================================");
                                sw.WriteLine(Environment.NewLine);
                            }

                            sw.WriteLine("these " + (vins_count - count) + " vehicle's battery info are not exist in mesdb, and just update the isnewenergy flag and querybatterysndate field:");
                            sw.WriteLine(Environment.NewLine);
                            sql = "update hp_vehicle set isnewenergy=1,querybatterysndate=getdate() where vin in(" + vins + ");";
                            sw.WriteLine(sql);
                            count = cfdms.Command(sql);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            mesdb.CloseConnection();
                            sw.Close();
                            fs.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally {
                    cfdms.CloseConnection();
                }
            });
            task.Start();

            return task;
        }
    }
}