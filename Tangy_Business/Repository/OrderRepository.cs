using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModel;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public OrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderDTO> Create(OrderDTO objDTO)
        {
            try
            {
                var obj = _mapper.Map<OrderDTO, Order>(objDTO);

                // Add OrderHeader so that the Id gets generated
                _db.OrderHeaders?.Add(obj.OrderHeader);
                await _db.SaveChangesAsync();

                // Add OrderDetails using the Id created in the previous step
                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                }
                _db.OrderDetails?.AddRange(obj.OrderDetails);
                await _db.SaveChangesAsync();

                return new OrderDTO()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(obj.OrderDetails).ToList()
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Delete(int id)
        {
            var objOrderHeader = await _db.OrderHeaders?.FirstOrDefaultAsync(o => o.Id == id)!;
            if (objOrderHeader is not null)
            {
                IEnumerable<OrderDetail> objOrderDetail = _db.OrderDetails?.Where(od => od.OrderHeaderId == id)!;

                _db.OrderDetails?.RemoveRange(objOrderDetail);
                _db.OrderHeaders.Remove(objOrderHeader);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<OrderDTO> Get(int id)
        {
            Order order = new()
            {
                OrderHeader = await _db.OrderHeaders?.FirstOrDefaultAsync(oh => oh.Id == id)!,
                OrderDetails = _db.OrderDetails?.Where(od => od.OrderHeaderId == id)
            };

            if (order is not null)
            {
                return _mapper.Map<Order, OrderDTO>(order);
            }
            return new OrderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
        {
            List<Order> ordersFromDb = new List<Order>();
            IEnumerable<OrderHeader> orderHeadersList = _db.OrderHeaders!;
            IEnumerable<OrderDetail> orderDetailsList = _db.OrderDetails!;

            foreach (var header in orderHeadersList)
            {
                Order order = new()
                {
                    OrderHeader = header,
                    OrderDetails = orderDetailsList.Where(o => o.OrderHeaderId == header.Id)
                };
                ordersFromDb.Add(order);
            }

            // TODO: Add some filtering

            return await Task.FromResult(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(ordersFromDb));
        }

        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id)
        {
            var data = await _db.OrderHeaders!.FindAsync(id);
            if (data is null)
            {
                return new OrderHeaderDTO();
            }

            if (data.Status == SD.Status_Pending)
            {
                data.Status = SD.Status_Confirmed;
                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
            }
            return new OrderHeaderDTO();
        }

        public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO)
        {
            if (objDTO is not null)
            {
                var orderHeader = _mapper.Map<OrderHeaderDTO, OrderHeader>(objDTO);
                _db.OrderHeaders?.Update(orderHeader);
                await _db.SaveChangesAsync();
                _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);
            }
            return new OrderHeaderDTO();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _db.OrderHeaders!.FindAsync(orderId);
            if (data is null)
            {
                return false;
            }

            data.Status = status;
            if (status == SD.Status_Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
