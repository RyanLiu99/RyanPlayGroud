using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreWebApp1.Controllers
{
  [Route("[controller]/[Action]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly IOptions<RyanCfg> _ryanCfgIOptions;
    private readonly IOptionsSnapshot<RyanCfg> _ryanCfgIOptionSnapShot;
    private readonly IOptionsMonitor<RyanCfg> _byOptionMonitor;

    public ValuesController(IConfiguration configuration, IOptions<RyanCfg> ryanCfgOptions, 
      IOptionsSnapshot<RyanCfg> ryanCfgIOptionSnapShot, IOptionsMonitor<RyanCfg> montior)
    {
      _configuration = configuration;
      _ryanCfgIOptions = ryanCfgOptions;
      _ryanCfgIOptionSnapShot = ryanCfgIOptionSnapShot;
      _byOptionMonitor = montior;
    }

    //values/GetByGet
    //return {"cfgNumber":12,"cfgString":"String3","sub":{"subCfgNumber":112,"subCfgString":"StringSub3"}}
    [HttpGet]
    public object Get()
    {
      var byGet = _configuration.GetSection(RyanCfg.RyanCfgStr).Get<RyanCfg>();


      var byBind = new RyanCfg();
      _configuration.GetSection(RyanCfg.RyanCfgStr).Bind(byBind);
      

      return new
      {
        byGet, //will change
        byBind, //will change
        byInjectedIOption = _ryanCfgIOptions.Value, //no reload
        _ryanCfgIOptionSnapShot.Value, //realod
        _byOptionMonitor.CurrentValue //reload
      };
    }


    //return {"cfgNumber":12,"cfgString":"String3","sub":{"subCfgNumber":112,"subCfgString":"StringSub3"}}
    public object GetByInjectedOption()
    {

      return _ryanCfgIOptions.Value;
    }


    //return {"subCfgNumber":112,"subCfgString":"StringSub3"}
    public object GetSub()
    {
      var ryanCfgSub = _configuration.GetSection(RyanCfg.RyanCfgSubStr).Get<RyanCfgSub>();
      return ryanCfgSub;
    }

    // GET api/<ValuesController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<ValuesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<ValuesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
