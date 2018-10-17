using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DbAccess;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace MicroServices.Controllers
{

    [RoutePrefix("api/vehcile")]
    public class GetNewEnergyBatteryInfoController : ApiController
    {
        private readonly IVehcileService _service;

        public GetNewEnergyBatteryInfoController(IVehcileService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tries to retrieve all foo objects.
        /// </summary>
        /// <returns>200 with collection of foo objects.</returns>
        [HttpGet, Route(""), ResponseType(typeof(IEnumerable<Vehcile>))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(VehcileListResponseExample))]
        public async Task<IHttpActionResult> Get(string vins)
        {
            var list = await _service.Get(vins).ConfigureAwait(false);

            return Ok(list);
        }
    }

    public class Vehcile
    {
        public string VIN { get; set; }

        public string PRIMARYBATTERYSN { get; set; }

        public string SPAREBATTERYSN { get; set; }
    }

    public interface IVehcileService
    {
        Task<IEnumerable<Vehcile>> Get(string vins);
    }

    public class VehcileService : IVehcileService
    {
        public Task<IEnumerable<Vehcile>> Get(string vins)
        {
            Vehcile vehcile = null;
            List<Vehcile> list = new List<Vehcile>();
            Db db = new Db(ConfigurationManager.ConnectionStrings["MesDb"].ToString(), ConnectionType.Oracle);
            try
            {
                OracleDataReader reader = db.GetDataReader(File.ReadSql("ReadVehcileBatteryInfo").Replace("@VIN", vins));
                var properties = typeof(Vehcile).GetProperties();
                while (reader.Read())
                {
                    vehcile = Map.MapEntity<Vehcile>(reader, properties);
                    list.Add(vehcile);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.CloseConnection();
            }

            return Task.FromResult<IEnumerable<Vehcile>>(list);
        }
    }

    public class VehcileListResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Vehcile>
            {
                new Vehcile {VIN = "LM7EGEMEXJA210017", PRIMARYBATTERYSN = "02CPE043JM1HDA88V0240001", SPAREBATTERYSN = "02CPE044JM1HDA88V0240002"},
                new Vehcile {VIN = "LN86GCAE0JB057691", PRIMARYBATTERYSN = "04XJB043JM1HDA88V3360001", SPAREBATTERYSN = "04XJB044JM1HDA88V3360002"}
            };
        }
    }
}
