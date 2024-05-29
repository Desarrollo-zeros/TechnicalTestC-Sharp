using Domain.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using EntityFrameworkCore.Testing.Moq;
using Infrastructure.Data.Data;
using Domain.Contracts.Base;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Domain.Contracts.Data;

namespace TestInfrastructure.Data
{
    public class UnitOfWorkTests
    {
        private readonly Mock<ITenant> _mockTenant;
        private readonly MasterDbContext _mockMasterContext;
        private readonly UnitOfWork<IMasterDbContext, IGenericRepository<Product, Guid>, Product, Guid> _unitOfWork;
        private readonly IGenericRepository<Product, Guid> _productRepository;
        private readonly Mock<IDbContextTransaction> _mockTransaction;

        public UnitOfWorkTests()
        {
            _mockTenant = new Mock<ITenant>();
            _mockTenant.Setup(t => t.TenantId).Returns(Guid.NewGuid());

            _mockMasterContext = Create.MockedDbContextFor<MasterDbContext>();

            _productRepository = new GenericRepository<Product, Guid, MasterDbContext>(_mockMasterContext, _mockTenant.Object);

            _unitOfWork = new UnitOfWork<IMasterDbContext, IGenericRepository<Product, Guid>, Product, Guid>(_mockMasterContext, _productRepository);

            _mockTransaction = new Mock<IDbContextTransaction>();

            var mockDatabaseFacade = new Mock<DatabaseFacade>(_mockMasterContext);
            mockDatabaseFacade.Setup(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockTransaction.Object);
            mockDatabaseFacade.Setup(m => m.BeginTransaction())
                .Returns(_mockTransaction.Object);

            var mockContext = Mock.Get(_mockMasterContext);
            mockContext.Setup(m => m.Database).Returns(mockDatabaseFacade.Object);
        }

        [Fact]
        public async Task InsertProduct_ShouldAddEntity()
        {
            var product = new Product("TestProduct", "TestProduct", Guid.NewGuid()) { };

            await _unitOfWork.Repository.InsertAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var result = await _unitOfWork.Repository.FindAsync(product.Id);
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task CommitTransaction_ShouldSaveChanges()
        {
            var product = new Product("TestProduct", "TestProduct", Guid.NewGuid()) { };

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Repository.InsertAsync(product);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            var result = await _unitOfWork.Repository.FindAsync(product.Id);
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task RollbackTransaction_ShouldNotSaveChanges()
        {
            var product = new Product("TestProduct", "TestProduct", Guid.NewGuid()) { };

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Repository.InsertAsync(product);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.RollbackTransactionAsync();
            var result = await _unitOfWork.Repository.FindAsync(product.Id);
            Assert.Null(result);
        }
    }
}
