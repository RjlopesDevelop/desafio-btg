using ConsumerBTGService.Application.Interfaces;
using ConsumerBTGService.Domain.DTOs;
using ConsumerBTGService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerBTGService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IOrderService _orderService;

        public OrderController(IRabbitMqService rabbitMqService, IOrderService orderService)
        {
            _rabbitMqService = rabbitMqService;
            _orderService = orderService;
        }

        [HttpPost]
        public ActionResult Post([FromBody] OrderDTO orderDTO)
        {
            _rabbitMqService.PostMessage(orderDTO);
            return Ok("Mensagem enviada!");
        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromQuery] int customerId)
        {
            var result = _orderService.GetOrdersByCustomerId(customerId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


    }
}