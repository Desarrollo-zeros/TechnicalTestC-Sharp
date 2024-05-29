using Domain.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using EntityFrameworkCore.Testing.Moq;
using Domain.Contracts.Base;
using Infrastructure.Data.Data;
using Domain.Contracts.Data;

namespace TestInfrastructure.Data.Repositories
{
    public class GenericRepositoryTests
    {
        private readonly Mock<ITenant> _mockTenant;
        private readonly MasterDbContext _mockMasterContext;
        private readonly GenericRepository<Product, Guid, IMasterDbContext> _repository;

        public GenericRepositoryTests()
        {
            _mockTenant = new Mock<ITenant>();
            _mockTenant.Setup(t => t.TenantId).Returns(Guid.NewGuid());

            _mockMasterContext = Create.MockedDbContextFor<MasterDbContext>();

            _repository = new GenericRepository<Product, Guid, IMasterDbContext>(_mockMasterContext, _mockTenant.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldAddEntity()
        {
            var product = new Product("TestProduct", "TestProduct", Guid.NewGuid()) { };

            await _repository.InsertAsync(product);
            Console.WriteLine(product);
            Assert.NotEqual(product.Id, Guid.Empty);

        }

        [Fact]
        public async Task FindAsync_ShouldReturnEntity()
        {
            var productId = Guid.NewGuid();
            var product = new Product("TestProduct", "TestProduct", Guid.NewGuid()) { Id = productId };
            _mockMasterContext.Set<Product>().Add(product);
            _mockMasterContext.SaveChanges();

            var result = await _repository.FindAsync(productId);

            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            var productId = Guid.NewGuid();
            var product = new Product("TestProduct", "TestProduct", Guid.NewGuid()) { Id = productId };
            _mockMasterContext.Set<Product>().Add(product);
            _mockMasterContext.SaveChanges();

            var result = await _repository.DeleteAsync(productId);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnFilteredEntities()
        {
            var tenantId = _mockTenant.Object.TenantId;
            var products = new List<Product>
            {
                new Product("TestProduc 1", "TestProduct 1", Guid.NewGuid()) { Id = Guid.NewGuid(), TenantId = tenantId, IsDeleted = false },
                new Product("TestProduct 2", "TestProduct 2", Guid.NewGuid()) { Id = Guid.NewGuid(), TenantId = tenantId, IsDeleted = false }
            }.AsQueryable();

            _mockMasterContext.Set<Product>().AddRange(products);
            _mockMasterContext.SaveChanges();

            var result = await _repository.GetAll().ToListAsync();

            Assert.Equal(2, result.Count);
            Assert.All(result, product => Assert.Equal(tenantId, product.TenantId));
        }
    }
}
