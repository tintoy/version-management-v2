using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DD.Cloud.VersionManagement.Controllers.Api
{
    using DataAccess;

    [Route("api/v2/versions")]
    public class VersionsController
      : ApiController
    {
      readonly VersionManagementEntities _entities;

      public VersionsController(VersionManagementEntities entities)
      {
        if (entities == null)
          throw new ArgumentNullException(nameof(entities));

        _entities = entities;
      }

      [HttpGet("{productName}/{releaseName}/{commitId}")]
      public IActionResult GetVersion(string productName, string releaseName, string commitId)
      {
        return Ok("Not implemented yet.");
      }
    }
}
