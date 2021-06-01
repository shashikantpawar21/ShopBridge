using System;
using Xunit;
using Moq;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using Microsoft.Extensions.Logging;
using ShopBridge.Api.Controllers;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Api.Dtos;
using FluentAssertions;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace ShopBridge.UnitTests
{
    public class InventoryControllerTests
    {
        private readonly Mock<IInventoryRepo> repositoryStub = new Mock<IInventoryRepo>();
        private readonly Mock<IMapper> mapperStub = new Mock<IMapper>();
        private readonly Mock<ILogger<InventoryController>> loggerStub = new Mock<ILogger<InventoryController>>();

        [Fact]
        public async Task GetInventoryItemByIdAsync_WithUnexistingItem_ReturnsNotFound()
        {
            repositoryStub.Setup(repo => repo.GetInventoryItemByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((InventoryItem)null);

            var controller = new InventoryController(repositoryStub.Object, mapperStub.Object, loggerStub.Object);

            var result = await controller.GetInventoryItemByIdAsync(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetInventoryItemByIdAsync_WithExistingItem_ReturnsExpectedItem()
        {
            var expectedItem = CreateRandomItem();
            var expectedItemDto = CreateRandomItemDto();
            repositoryStub.Setup(repo => repo.GetInventoryItemByIdAsync(It.IsAny<int>()))
           .ReturnsAsync(expectedItem);

            mapperStub.Setup(m => m.Map<InventoryItemReadDto>(It.IsAny<InventoryItem>())).Returns(expectedItemDto);
            var controller = new InventoryController(repositoryStub.Object, mapperStub.Object, loggerStub.Object);

            var result = await controller.GetInventoryItemByIdAsync(It.IsAny<int>());
            var okResult = result.Result as OkObjectResult;
            var model = okResult.Value as InventoryItemReadDto;

            model.Should().BeEquivalentTo(expectedItemDto,
            options => options.ComparingByMembers<InventoryItemReadDto>());
        }

        [Fact]
        public async Task GetAllInventoriesAsync_WithExistingItem_ReturnsAllExpectedItems()
        {
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem() };
            var expectedItemsDto = new[] { CreateRandomItemDto(), CreateRandomItemDto() };
            repositoryStub.Setup(repo => repo.GetAllInventoriesAsync())
          .ReturnsAsync(expectedItems);
            mapperStub.Setup(m => m.Map<IEnumerable<InventoryItemReadDto>>(It.IsAny<IEnumerable<InventoryItem>>())).Returns(expectedItemsDto);
            var controller = new InventoryController(repositoryStub.Object, mapperStub.Object, loggerStub.Object);

            var result = await controller.GetAllInventoriesAsync();
            var okResult = result.Result as OkObjectResult;
            var model = okResult.Value as IEnumerable<InventoryItemReadDto>;

            model.Should().BeEquivalentTo(expectedItemsDto,
            options => options.ComparingByMembers<IEnumerable<InventoryItemReadDto>>());
        }

        [Fact]
        public async Task CreateInventoryItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            var expectedItem = CreateRandomItem();
            var expectedItemDto = CreateRandomItemDto();
            mapperStub.Setup(m => m.Map<InventoryItem>(It.IsAny<InventoryItemCreateDto>())).Returns(expectedItem);
            mapperStub.Setup(m => m.Map<InventoryItemReadDto>(It.IsAny<InventoryItem>())).Returns(expectedItemDto);
            var controller = new InventoryController(repositoryStub.Object, mapperStub.Object, loggerStub.Object);

            var result = await controller.CreateInventoryItemAsync(It.IsAny<InventoryItemCreateDto>());
            var okResult = result as CreatedAtRouteResult;
            var model = okResult.Value as InventoryItemReadDto;

            model.Should().BeEquivalentTo(expectedItemDto,
            options => options.ComparingByMembers<InventoryItemReadDto>());

        }

        [Fact]
        public async Task UpdateInventoryItemAsync_WithExistingItem_ReturnsNoContent()
        {
            var existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetInventoryItemByIdAsync(It.IsAny<int>()))
           .ReturnsAsync(existingItem);
            var controller = new InventoryController(repositoryStub.Object, mapperStub.Object, loggerStub.Object);
            var result = await controller.UpdateInventoryItemAsync(It.IsAny<int>(), It.IsAny<InventoryItemUpdateDto>());
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteInventoryItemAsync_WithExistingItem_ReturnsNoContent()
        {
            var existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetInventoryItemByIdAsync(It.IsAny<int>()))
           .ReturnsAsync(existingItem);
            var controller = new InventoryController(repositoryStub.Object, mapperStub.Object, loggerStub.Object);
            var result = await controller.DeleteInventoryItemAsync(It.IsAny<int>());
            result.Should().BeOfType<NoContentResult>();
        }
        private InventoryItem CreateRandomItem()
        {
            return new InventoryItem()
            {
                Id = 1,
                Name = "Apple 11",
                Description = "Brand new apple phone",
                Price = 80000
            };

        }

        private InventoryItemReadDto CreateRandomItemDto()
        {
            return new InventoryItemReadDto()
            {
                Description = "Brand new apple phone",
                Name = "Apple 11",
                Price = 80000
            };

        }
    }
}
