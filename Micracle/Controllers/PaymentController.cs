using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Services;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IUserServices _userService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderServices _orderServices;


        public PaymentController(IPaymentServices paymentServices, IUserServices userService, ApplicationDbContext dbContext, IOrderServices orderServices)
        {
            _paymentServices = paymentServices;
            _userService = userService;
            _dbContext = dbContext;
            _orderServices = orderServices;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayemnt(string? searchterm)
        {
            try
            {
                var results = await _paymentServices.GetAllPayment(searchterm);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPaymentById(string paymentId)
        {
            try
            {
                var result = await _paymentServices.GetPaymentById(paymentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("payment/vnpay")]
        public async Task<IActionResult> AddPayment(PaymentDTO paymentDTO, string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var order = await _orderServices.GetOrderById(paymentDTO.OrderId);
            try
            {
                var payment = new PaymentDTO();
                if (paymentDTO.Method == "VnPay")
                {
                    var vnPayModel = new VnPaymentRequestModel()
                    {
                        Amount = order.TotalPrice,
                        CreatedDate = DateTime.Now,
                        Description = "thanh toán VnPay",
                        OrderId = paymentDTO.OrderId,
                        FullName = user.FullName,
                    };
                    if (vnPayModel.Amount < 0)
                    {
                        return BadRequest("The amount entered cannot be less than 0. Please try again");
                    }
                    var paymentUrl = _paymentServices.CreatePaymentUrl(HttpContext, vnPayModel, userId);
                    return Ok(new { url = paymentUrl });
                    //return Redirect(_vpnPayServices.CreatePaymentUrl(HttpContext, vnPayModel, userId));
                    //return new JsonResult(_vpnPayServices.CreatePaymentUrl(HttpContext, vnPayModel, userId));
                }
                return BadRequest("Payment not successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("PaymentBack")]
        //public async Task<IActionResult> PaymenCalltBack()
        //{
        //    var queryParameters = HttpContext.Request.Query;
        //    // Kiểm tra và lấy giá trị 'vnp_OrderInfo' từ Query
        //    string orderInfo = queryParameters["vnp_OrderInfo"];
        //    string userId = _paymentServices.GetUserId(orderInfo);
        //    string transactionId = _paymentServices.GetOrderId(orderInfo);
        //    double amount = double.Parse(queryParameters["vnp_Amount"]);
        //    if (string.IsNullOrEmpty(orderInfo))
        //    {
        //        return BadRequest("Thông tin đơn hàng không tồn tại.");
        //    }
        //    // Phân tích chuỗi 'orderInfo' để lấy các thông tin cần thiết
        //    var orderInfoDict = new Dictionary<string, string>();
        //    string[] pairs = orderInfo.Split(',');
        //    foreach (var pair in pairs)
        //    {
        //        string[] keyValue = pair.Split(':');
        //        if (keyValue.Length == 2)
        //        {
        //            orderInfoDict[keyValue[0].Trim()] = keyValue[1].Trim();
        //        }
        //    }
        //    // Lấy ví tiền của người dùng dựa trên 'UserID'
        //    var wallet = await _walletServices.GetWalletByUserId(userId);
        //    if (wallet == null)
        //    {
        //        return BadRequest("Wallet not found for the given user.");
        //    }
        //    // Tạo và lưu trữ thông tin giao dịch     
        //    var paymentDto = new PaymentDTO()
        //    {
        //         = transactionId,
        //        Status = "Completed",
        //        Amount = (float)amount / 100,  // Chia cho 100 nếu giá trị 'amount' là theo đơn vị nhỏ nhất của tiền tệ
        //        Payment = "VnPay",
        //        CreatedDate = DateTime.Now,
        //    };
        //    var result = await _services.AddTransaction(wallet.ID, transactiondto);
        //    if (result == "Success")
        //    {
        //        await _walletServices.UpdateWalletBalanceAsync(transactionId);
        //        return Redirect("https://meet.google.com/fcd-wvxs-cvn?authuser=1" + userId);
        //    }
        //    return BadRequest("Invalid transaction data.");
        //}
    }
}
