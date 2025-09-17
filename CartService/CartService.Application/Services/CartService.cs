using CartService.Application.Constants;
using CartService.Application.Dtos.Requests;
using CartService.Application.Dtos.Responses;
using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Application.Interfaces.IService;
using CartService.Application.Wrappers;
using CartService.Domain.Models;
using ProductService.Application.Services;

namespace CartService.Application.Services
{
    public class CartService : GenericService<CartRequestDto, CartResponseDto, Cart>, ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAppLogger<GenericService<CartRequestDto, CartResponseDto, Cart>> _logger;
        private readonly IAppMapper _mapper;

        public CartService(
            IGenericRepository<Cart> repository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IAppMapper mapper,
            IAppLogger<GenericService<CartRequestDto, CartResponseDto, Cart>> logger) : base(repository, mapper, logger)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<CartResponseDto>> GetCartByUserIdAsync(string userId)
        {
            _logger.LogInformation($"Getting cart for user: {userId}");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogInformation($"No cart found for user: {userId}, creating new cart");
                cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                await _cartRepository.AddAsync(cart);
            }

            var response = _mapper.Map<CartResponseDto>(cart);
            return ServiceResult<CartResponseDto>.Ok(
                StatusCodes.SUCCESS,
                response,
                "Cart retrieved successfully");
        }

        public async Task<ServiceResult<CartResponseDto>> AddItemToCartAsync(string userId, AddCartItemRequestDto item)
        {
            _logger.LogInformation($"Adding item to cart for user: {userId}");

            // Check if product exists and is available
            var product = await _productRepository.GetByOriginalIdAsync(item.ProductId);
            if (product == null)
            {
                _logger.LogWarning($"Product not found: {item.ProductId}");
                return ServiceResult<CartResponseDto>.Fail(
                    StatusCodes.NOT_FOUND,
                    "Product not found",
                    new List<Error> { new Error { Field = "ProductId", Message = "The specified product does not exist" } });
            }

            if (!product.IsAvailable)
            {
                _logger.LogWarning($"Product is not available: {item.ProductId}");
                return ServiceResult<CartResponseDto>.Fail(
                    StatusCodes.BAD_REQUEST,
                    "Product is not available",
                    new List<Error> { new Error { Field = "ProductId", Message = "The specified product is not available for purchase" } });
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogInformation($"No cart found for user: {userId}, creating new cart");
                cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                await _cartRepository.AddAsync(cart);
            }

            var cartItem = _mapper.Map<CartItem>(item);
            // Set the current price from the product
            cartItem.Price = product.Price;

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                _logger.LogInformation($"Updating quantity for existing item in cart");
                // Update the price in case it changed
                existingItem.Price = product.Price;
                await _cartRepository.UpdateItemQuantityAsync(cart.Id, item.ProductId, existingItem.Quantity + item.Quantity);
            }
            else
            {
                _logger.LogInformation($"Adding new item to cart");
                await _cartRepository.AddItemToCartAsync(cart.Id, cartItem);
            }

            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId);
            var response = _mapper.Map<CartResponseDto>(updatedCart!);

            return ServiceResult<CartResponseDto>.Ok(
                StatusCodes.SUCCESS,
                response,
                "Item added to cart successfully");
        }

        public async Task<ServiceResult> RemoveItemFromCartAsync(string userId, string productId)
        {
            _logger.LogInformation($"Removing item from cart for user: {userId}, product: {productId}");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning($"Cart not found for user: {userId}");
                return ServiceResult.Fail(StatusCodes.NOT_FOUND, "Cart not found");
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem == null)
            {
                _logger.LogWarning($"Item not found in cart: {productId}");
                return ServiceResult.Fail(StatusCodes.NOT_FOUND, "Item not found in cart");
            }

            await _cartRepository.RemoveItemFromCartAsync(cart.Id, productId);
            return ServiceResult.Ok(StatusCodes.NO_CONTENT, "Item removed from cart successfully");
        }

        public async Task<ServiceResult> UpdateItemQuantityAsync(string userId, string productId, int quantity)
        {
            _logger.LogInformation($"Updating item quantity for user: {userId}, product: {productId}, quantity: {quantity}");

            if (quantity < 1)
            {
                _logger.LogWarning($"Invalid quantity provided: {quantity}");
                return ServiceResult.Fail(
                    StatusCodes.BAD_REQUEST,
                    "Invalid quantity",
                    new List<Error> { new Error { Field = "quantity", Message = "Quantity must be greater than 0" } });
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning($"Cart not found for user: {userId}");
                return ServiceResult.Fail(StatusCodes.NOT_FOUND, "Cart not found");
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem == null)
            {
                _logger.LogWarning($"Item not found in cart: {productId}");
                return ServiceResult.Fail(StatusCodes.NOT_FOUND, "Item not found in cart");
            }

            await _cartRepository.UpdateItemQuantityAsync(cart.Id, productId, quantity);
            return ServiceResult.Ok(StatusCodes.NO_CONTENT, "Item quantity updated successfully");
        }

        public async Task<ServiceResult> ClearCartAsync(string userId)
        {
            _logger.LogInformation($"Clearing cart for user: {userId}");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning($"Cart not found for user: {userId}");
                return ServiceResult.Fail(StatusCodes.NOT_FOUND, "Cart not found");
            }

            await _cartRepository.ClearCartAsync(cart.Id);
            return ServiceResult.Ok(StatusCodes.NO_CONTENT, "Cart cleared successfully");
        }
    }
}
