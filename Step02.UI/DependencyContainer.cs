//namespace Step02.Common;

//public class DependencyContainer
//{

//    private DatabaseContext _databaseContext;


//    private IProductRepository _proRepo;
//    private IUserRepository _userRepository;
//    private IOrderRepository _orderRepository;


//    private IProductService _productService;
//    private IUserService _userService;
//    private IOrderService _orderService;


//    public DependencyContainer()
//    {
//        _databaseContext = new DatabaseContext();
//    }



//    public DatabaseContext DbContext => _databaseContext;

//    public IProductRepository ProductRepository
//    {
//        get
//        {
//            if (_proRepo == null)
//            {
//                Console.WriteLine("🔧 Creating ProductRepository...");
//                _proRepo = new ProductRepository(_databaseContext);
//            }

//            return _proRepo;
//        }
//    }

//    public IOrderRepository OrderRepository
//    {
//        get
//        {
//            if (_orderRepository == null)
//            {
//                Console.WriteLine("🔧 Creating OrderRepository...");
//                _orderRepository = new OrderRepository(_databaseContext);
//            }
//            return _orderRepository;
//        }
//    }

//    public IUserRepository UserRepository
//    {
//        get
//        {
//            if (_userRepository == null)
//            {
//                Console.WriteLine("🔧 Creating UserRepository...");
//                _userRepository = new UserRepository(_databaseContext);
//            }
//            return _userRepository;
//        }
//    }


//    public IProductService ProductService
//    {
//        get
//        {
//            if (_productService == null)
//            {
//                Console.WriteLine("🔧 Creating ProductService with Logging...");

//                // لایه واقعی
//                var realService = new ProductService(ProductRepository);

//                // لایه لاگینگ (Decorator)
//                _productService = new ProductLoggingService(realService);
//            }
//            return _productService;
//        }
//    }

//    public IOrderService OrderService
//    {
//        get
//        {
//            if (_orderService == null)
//            {
//                Console.WriteLine("🔧 Creating OrderService with Logging...");

//                // لایه واقعی
//                var _orderService = new OrderService(OrderRepository, ProductRepository, UserRepository);

//                // لایه لاگینگ (Decorator)
//                //_orderService = new OrderLoggingService(realService);
//            }
//            return _orderService;
//        }
//    }


//    public IUserService UserService
//    {
//        get
//        {
//            if (_userService == null)
//            {
//                Console.WriteLine("🔧 Creating UserService with Logging...");

//                // لایه واقعی
//                var _userService = new UserService(UserRepository);

//                // لایه لاگینگ (Decorator)
//                //_userService = new UserLoggingService(realService);
//            }
//            return _userService;
//        }
//    }

//}
