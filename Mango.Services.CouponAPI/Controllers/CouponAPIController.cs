using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
                return Ok(_response);
            }
            catch (Exception ex) 
            { 
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                //return StatusCode(500, "Internal server error");
                return StatusCode(500, _response.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponId == id);                
                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon with ID {id} not found.";
                    return NotFound(_response);
                }                
                _response.Result = _mapper.Map<CouponDto>(coupon);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response.Message);
            }
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public IActionResult GetByCode(string code)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());
                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon with code {code} not found.";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CouponDto>(coupon);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CouponDto couponDto)
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDto);
            _db.Coupons.Add(coupon);
            _db.SaveChanges();

            _response.Result = _mapper.Map<CouponDto>(coupon);
            return Ok(_response);
        }

        [HttpPut]
        public IActionResult Put([FromBody] CouponDto couponDto)
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDto);
            _db.Coupons.Update(coupon);
            _db.SaveChanges();

            _response.Result = _mapper.Map<CouponDto>(coupon);
            return Ok(_response);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponId == id);
            if(coupon == null)
            {
                _response.IsSuccess = false;
                _response.Message = "No coupon found";
                return NotFound(_response);
            }
            _db.Coupons.Remove(coupon);
            _db.SaveChanges();

            _response.IsSuccess = true;
            _response.Message = "Coupon deleted successfully";
            return Ok(_response);
        }
    }
}
