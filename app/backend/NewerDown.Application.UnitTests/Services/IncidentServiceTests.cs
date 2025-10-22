using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Application.Time;
using NewerDown.Application.UnitTests.Helpers;
using NewerDown.Domain.DTOs.Incidents;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
    public class IncidentServiceTests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private Mock<IScopedTimeProvider> _timeProviderMock;
        private IncidentService _incidentService;

        private readonly Guid _userId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _context = new DbContextProvider().BuildDbContext();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<IncidentMappingProfile>(); // Assume this exists
            });
            _mapper = mapperConfig.CreateMapper();

            _timeProviderMock = new Mock<IScopedTimeProvider>();
            _timeProviderMock.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            _incidentService = new IncidentService(_context, _mapper, _timeProviderMock.Object);
        }
        
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnUserIncidents()
        {
            // Arrange
            var monitor = new Domain.Entities.Monitor()
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                Name = "Google",
                Target = "google.com"
            };

            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Monitor = monitor,
                StartedAt = DateTime.UtcNow.AddMinutes(-10)
            };
            
            _context.Monitors.Add(monitor);
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            // Act
            var result = await _incidentService.GetAllAsync(_userId);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(incident.Id));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnIncident_WhenFound()
        {
            // Arrange
            var monitor = new Domain.Entities.Monitor()
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                Name = "Google",
                Target = "google.com"
            };
            
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Monitor = monitor,
                StartedAt = DateTime.UtcNow.AddMinutes(-5)
            };
            
            _context.Monitors.Add(monitor);
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            // Act
            var result = await _incidentService.GetByIdAsync(incident.Id, _userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(incident.Id));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Act
            var result = await _incidentService.GetByIdAsync(Guid.NewGuid(), _userId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AcknowledgeIncidentAsync_ShouldAcknowledge_WhenValid()
        {
            // Arrange
            var monitor = new Domain.Entities.Monitor()
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                Name = "Google",
                Target = "google.com",
            };
            
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Monitor = monitor,
                StartedAt = DateTime.UtcNow.AddMinutes(-15),
                IsAcknowledged = false
            };
            
            _context.Monitors.Add(monitor);
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            // Act
            await _incidentService.AcknowledgeIncidentAsync(incident.Id, _userId);

            var updated = await _context.Incidents.FindAsync(incident.Id);

            // Assert
            Assert.That(updated.IsAcknowledged, Is.True);
        }

        [Test]
        public void AcknowledgeIncidentAsync_ShouldThrow_WhenIncidentNotFound()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _incidentService.AcknowledgeIncidentAsync(Guid.NewGuid(), _userId));

            Assert.That(ex!.Message, Does.Contain("Incident not found"));
        }

        [Test]
        public async Task CommentIncidentAsync_ShouldAddComment_WhenNotExists()
        {
            // Arrange
            var monitor = new Domain.Entities.Monitor()
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                Name = "Google",
                Target = "google.com"
            };
            
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Monitor = monitor
            };
            
            _context.Monitors.Add(monitor);
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            var commentDto = new CreateIncidentCommentDto
            {
                IncidentId = incident.Id,
                UserId = _userId,
                Comment = "Investigating..."
            };

            // Act
            await _incidentService.CommentIncidentAsync(commentDto);

            // Assert
            var comment = await _context.IncidentComments.FirstOrDefaultAsync();
            Assert.That(comment, Is.Not.Null);
            Assert.That(comment!.Comment, Is.EqualTo(commentDto.Comment));
        }

        [Test]
        public async Task CommentIncidentAsync_ShouldThrow_WhenCommentExists()
        {
            // Arrange
            var monitor = new Domain.Entities.Monitor()
            {
                Id = Guid.NewGuid(), 
                UserId = _userId,
                Name = "Google",
                Target = "google.com"
            };
            
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Monitor = monitor
            };

            var existingComment = new IncidentComment
            {
                Id = Guid.NewGuid(),
                Incident = incident,
                IncidentId = incident.Id,
                Comment = "Already exists"
            };

            _context.Monitors.Add(monitor);
            _context.Incidents.Add(incident);
            _context.IncidentComments.Add(existingComment);
            await _context.SaveChangesAsync();

            var dto = new CreateIncidentCommentDto
            {
                IncidentId = incident.Id,
                UserId = _userId,
                Comment = "Should not be added"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _incidentService.CommentIncidentAsync(dto));

            Assert.That(ex!.Message, Does.Contain("Incident comment has already been created"));
        }
    }