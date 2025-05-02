//Modelsforall

Public class Product {
    public int ID{ get; set;}
    public int Name { get; set;}
    public int Price { get; set;}
     public int Stock {get; set;}
}
public class Customer{
    public int Id {get; set;}
    public string Name { get; set; }
    public string Email { get; set; }

}
public class Order {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; }
}

public class OrderItem {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}
////////////////////////////////*CODE()*//////////////////////////////
//Productcontroller
[APi controller]
[Route("api/products")]
Public class ProductController:ControllerBase{
    private readonly AppDbContext_context;
    PUBLIC ProductController(AppDbContext context) =>_context = context;
    [HttpGet]
    public async Task<ApplicationResult<IEnumerable<Product>>> Get()=>
    await _context.products.ToListAsync();
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(int id){
        varproduct = await _context.Products.FindAsync(id){
            return product==null?Not Found(): product;
        }
    }
    [HttpPost]
    public async Task<IActionResult>Create (Product product){
        _context.Product.Add(product);
        await _context.SaveChangeAsync();
        return CreateAtAction(nameof(Get), new{id = product.Id },product);

    }
    [HttpPut("{id}")]

    public async Task<IActionResult> Update (int id, Product product){
        if(id != product.Id) return BadRequest();
        _context.Entry(product).State= EntityState.Modified;
        await _context.SAVEchangesAsync();
        return NoContent();

    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> Delete(int id){
        var product = await _context.Products.FindAsync(Id);
        if (product== null) return NoContentFounf();
        _context.Products.Remove(product);
        await _context.SAVEchangesAsync();
        return NoContent();
    }
}
/////////////////////////////////////Develop a Restful API for Customer CRUD Operations: Implement a full CRUD API for handling customer data management.

//customercontroller code



[APiController]
[Route("api/Customers")]

publicclass customercontroller : ControllerBase{
    private readonly AppDbContext _context;
    public customercontroller(AppDbContext context) => _context= context;

    [HttpGet]

    public async Task<ActionResult<IEnumerable<Customer>>> Get() => await _context.customer.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> Get(int id){
        var customer = await _context.Customer.FindAsync(id);
        return customer == null ? NotFound(): customer;

    }

    [HttpPost]
    public async Task<IActionResult> Create (Customer customer){
        _context.Customer.Add(customer);
        await _context.SaveChangeAsync()
        return CreateAtAction(nameof(Get), new { id = customer.ID}, customer);

    }

    [HttpPut("{id}")]

    public async Task<IActionResult> Update(int id, Customer customer){
        if(id != customer.ID) return BadRequest();
        _context.Entry(customer).State=EntityState.Modified;
        await _context.SaveChangeAsync();
        return NoContent();

    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> Delete(int id){
        var customer= await _context.Customer.FindAsync(id);
        if(customer==null) return NotFound();
        _context.Customer.Remove(customer);
        await _context.SaveChangeAsync();
        return NoContent();
    }


}


/////Implement Order Processing and Display: Enable the functionality to process and
/// sell products, and provide an endpoint to retrieve and display customer orders.



//OrderController code

[APiController]
[Route("api/orders")]

public class OrderController : ControllerBase{
    private readonly AppDbContext _context;
    public OrderController(AppDbContext context) => _context= context;

    [HttpPost]
    Public async Task<IActionResult> CreateOrder ([FromBody] Order order){
        foreach (var item in order.Items)
        {
            var product = await _context.Products.FindAsync(item.ProoductId);
            if(product == null || product.Stock<item.Quantity)
            return BadRequest("Invalid Product")
            product.Stock -= item.Quantity;

            
        }
        _context.Ordes.Add(order);
        await _context.SaveChangeAsync();
        return Ok(order);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomer(int customerId) {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
    }
}