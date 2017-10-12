using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using DnsClient.Protocol;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeamLab.DnsLookupAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DnsController : ControllerBase
    {
        private ILookupClient _client;

        public DnsController(ILookupClient client)
        {
            _client = client;
        }

        /// <summary>
        /// DNSs the query.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <returns></returns>
        [MapToApiVersion("2.0")]
        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("DnsQuery")]
        public IActionResult DnsQuery(string domain)
        {
            var records = _client.Query(domain, QueryType.ANY).AllRecords.ToList();

            return Ok(records);
        }

        /// <summary>
        /// DNSs the query.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="dns">The DNS server.</param>
        /// <param name="dnsPort">The DNS server port.</param>
        /// <returns></returns>
        [MapToApiVersion("3.0")]
        [HttpGet]
        [Route("DnsQuery")]
        public IActionResult DnsQuery(string domain,string dns = "8.8.8.8", int dnsPort = 53)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(dns), dnsPort);
            var client = new LookupClient(endpoint)
            {
                UseCache = true

            };

            var records = _client.Query(domain, QueryType.ANY).AllRecords.ToList();

            return Ok(records);
        }

        /// <summary>
        /// Echoes the specified domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("Echo")]
        public string Echo(string domain)
        {
            return domain;
        }
    }
}
