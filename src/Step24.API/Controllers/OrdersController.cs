using Microsoft.AspNetCore.Mvc;
using Step022.Application.Orders;

namespace Step24.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{

    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        //- Log Requerst
        var orders = _orderService.GetAllOrders();
        return Ok(orders);

        //- Log Response
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var order = _orderService.GetOrder(id);
        return Ok(order);
    }

    [HttpPost]
    public IActionResult Create(int userId, List<(int ProductId, int Quantity)> items)
    {
        var order = _orderService.CreateOrder(userId, items);
        return Ok(order);
    }


    [HttpPost("{id:int}/confirm")]
    public IActionResult Confirm(int id)
    {
        var order = _orderService.ConfirmOrder(id);
        return Ok(order);
    }

    [HttpPost("{id:int}/ship")]
    public IActionResult Ship(int id)
    {
        var order = _orderService.ShipOrder(id);
        return Ok(order);
    }

    [HttpPost("{id:int}/deliver")]
    public IActionResult Deliver(int id)
    {
        var order = _orderService.DeliverOrder(id);
        return Ok(order);
    }

    [HttpPost("{id:int}/cancel")]
    public IActionResult Cancel(int id)
    {
        var order = _orderService.CancelOrder(id);
        return Ok(order);
    }

}
