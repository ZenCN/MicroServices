using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.IO;

namespace DbAccess
{
    public class Map
    {
        public static TEntity MapEntity<TEntity>(SqlDataReader reader, PropertyInfo[] properties) where TEntity : class, new()
        {
            try
            {
                //properties 为了避免多次反射，将其参数化，在C#编程中，应时刻有着面向对象的思想，而不是面向过程
                //var properties = typeof(TEntity).GetProperties();
                var entity = new TEntity();
                foreach (var propertie in properties)
                {
                    if (propertie.CanWrite)//判断属性是否可写
                    {
                        try
                        {
                            var index = reader.GetOrdinal(propertie.Name.Replace("_", ""));//列序号
                            var data = reader.GetValue(index);//指定列值
                            if (data != DBNull.Value)
                            {
                                if (propertie.PropertyType.IsGenericType && propertie.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    propertie.SetValue(entity, Convert.ChangeType(data, propertie.PropertyType.GetGenericArguments()[0]), null);
                                }
                                else
                                {
                                    propertie.SetValue(entity, Convert.ChangeType(data, propertie.PropertyType), null);
                                }
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TEntity MapEntity<TEntity>(OracleDataReader reader, PropertyInfo[] properties) where TEntity : class, new()
        {
            try
            {
                //properties 为了避免多次反射，将其参数化，在C#编程中，应时刻有着面向对象的思想，而不是面向过程
                //var properties = typeof(TEntity).GetProperties();
                var entity = new TEntity();
                foreach (var propertie in properties)
                {
                    if (propertie.CanWrite)//判断属性是否可写
                    {
                        try
                        {
                            var index = reader.GetOrdinal(propertie.Name.Replace("_", ""));//列序号
                            var data = reader.GetValue(index);//指定列值
                            if (data != DBNull.Value)
                            {
                                if (propertie.PropertyType.IsGenericType && propertie.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    propertie.SetValue(entity, Convert.ChangeType(data, propertie.PropertyType.GetGenericArguments()[0]), null);
                                }
                                else
                                {
                                    propertie.SetValue(entity, Convert.ChangeType(data, propertie.PropertyType), null);
                                }
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
