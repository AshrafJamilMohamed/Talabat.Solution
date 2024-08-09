using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;

namespace Talabat.APIs.Controllers
{

    public class AccountController : BaseAPIController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAuthService authService;
        private readonly IGenericRepository<Address> addressRepository;
        private readonly IMapper mapper;

        public AccountController
                    (
                        UserManager<ApplicationUser> _userManager,
                        SignInManager<ApplicationUser> _signInManager,
                        IAuthService _authService,
                        IGenericRepository<Address> AddressRepository,
                        IMapper Mapper

                    )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            authService = _authService;
            addressRepository = AddressRepository;
            mapper = Mapper;
        }

        // Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await userManager.FindByEmailAsync(model.Email);
            if (User is { })
            {
                var result = await signInManager.CheckPasswordSignInAsync(User, model.Password, false);
                if (result.Succeeded)
                    return Ok(
                        new UserDto()
                        {
                            Email = User.Email,
                            DispalyName = User.DispalyName,
                            Token = await authService.CreateTokenAsync(User, userManager)
                        });
            }

            return BadRequest(new ApiResponse(401));

        }

        // Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            // Check if this user has an email or not
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is { }) return BadRequest(new ApiResponse(400));
            var NewUser = new ApplicationUser()
            {
                DispalyName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split("@")[0]

            };
            var result = await userManager.CreateAsync(NewUser, model.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DispalyName = NewUser.DispalyName,
                Token = await authService.CreateTokenAsync(NewUser, userManager),
                Email = NewUser.Email
            });

        }


        // GetCurrentUser
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            var CurrentUser = await userManager.FindByEmailAsync(UserEmail);

            return Ok
                (
                    new UserDto()
                    {
                        Email = CurrentUser.Email,
                        DispalyName = CurrentUser.DispalyName,
                        Token = await authService.CreateTokenAsync(CurrentUser, userManager)
                    }
                );
        }

        // Get User Address

        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<Address>> GetUserAddress()
        {
            var user = await userManager.FindUserWithAddressAsync(User);
            return Ok(mapper.Map<AddressDTO>(user?.Address));
        }

        // UpdateUserAddress

        [HttpPut("Address")]
        [Authorize]
        [ProducesErrorResponseType(typeof(ApiValidationErrorRepsonse))]
        [ProducesDefaultResponseType(typeof(Address))]
        
        public async Task<ActionResult<Address>> UpdateUserAddress(AddressDTO address)
        {
            var user = await userManager.FindUserWithAddressAsync(User);

            var UpdatedAddress = mapper.Map<Address>(address);
            UpdatedAddress.Id = user.Address.Id;
            user.Address = UpdatedAddress;
            var Result = await userManager.UpdateAsync(user);

            if (!Result.Succeeded)
                return BadRequest(new ApiValidationErrorRepsonse() { Errors = Result.Errors.Select(E => E.Description) });

            return Ok(UpdatedAddress);

        }
    }
}
