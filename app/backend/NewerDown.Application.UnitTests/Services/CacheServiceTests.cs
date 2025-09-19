using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NewerDown.Application.Constants;
using NewerDown.Application.Services;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.UnitTests.Services;

 [TestFixture]
 public class CacheServiceTests
 {
     private Mock<IDistributedCache> _distributedCacheMock; 
     private CacheService _cacheService;

     private static JsonSerializerOptions _jsonSerializerOptions = new()
     {
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
         WriteIndented = true
     };
     
     [SetUp] 
     public void SetUp() 
     {
         _distributedCacheMock = new Mock<IDistributedCache>(); 
         _cacheService = new CacheService(_distributedCacheMock.Object);
     }
     
     [Test]
     public async Task SetAsync_ShouldStoreSerializedValueInCache()
     {
         // Arrange
         var key = "test-key";
         var testObject = new { Name = "Test", Age = 25 };

         var expectedJson = JsonSerializer.Serialize(testObject, _jsonSerializerOptions);

         // Act
         await _cacheService.SetAsync(key, testObject);

         // Assert
         _distributedCacheMock.Verify(cache =>
                 cache.SetAsync(
                     key,
                     It.Is<byte[]>(b => Encoding.UTF8.GetString(b) == expectedJson),
                     It.Is<DistributedCacheEntryOptions>(opt =>
                         opt.AbsoluteExpirationRelativeToNow.Value.TotalMinutes == CacheConstants.DefaultCacheDurationInMinutes),
                     It.IsAny<CancellationToken>()),
             Times.Once);
     }

     [Test]
     public async Task GetAsync_ShouldReturnDeserializedObject_WhenKeyExists()
     {
         // Arrange
         var key = "user";
         var originalObject = new User { Id = Guid.NewGuid(), UserName = "Alice" };
    
         var json = JsonSerializer.Serialize(originalObject, _jsonSerializerOptions);

         var bytes = Encoding.UTF8.GetBytes(json);

         _distributedCacheMock
             .Setup(cache => cache.GetAsync(key, It.IsAny<CancellationToken>()))
             .ReturnsAsync(bytes);

         // Act
         var result = await _cacheService.GetAsync<User>(key);

         // Assert
         Assert.That(result, Is.Not.Null);
         Assert.That(result.Id, Is.EqualTo(originalObject.Id));
         Assert.That(result.UserName, Is.EqualTo(originalObject.UserName));
     }
     
     [Test]
     public async Task GetAsync_ShouldReturnDefault_WhenKeyDoesNotExist()
     {
         // Arrange
         var key = "nonexistent";

         _distributedCacheMock
             .Setup(cache => cache.GetAsync(key, It.IsAny<CancellationToken>()))
             .ReturnsAsync((byte[]?)null);

         // Act
         var result = await _cacheService.GetAsync<User>(key);

         // Assert
         Assert.That(result, Is.Null);
     }

     [Test] 
     public async Task RemoveAsync_ShouldCallRemoveOnDistributedCache() 
     { 
         // Arrange
         var key = "remove-me";

         // Act
         await _cacheService.RemoveAsync(key);

         // Assert
         _distributedCacheMock.Verify(cache =>
                cache.RemoveAsync(key, It.IsAny<CancellationToken>()), Times.Once);
     }
}
    