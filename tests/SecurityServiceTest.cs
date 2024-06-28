using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;
using apbd_project.Service;
using apbd_project.Service.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace tests;

[TestClass]
public class SecurityServiceTest
{
    private DbContextOptions<ApplicationContext> _options;
    private ApplicationContext _context;
    private Mock<IConfiguration> _configurationMock;

    // This method is called before each test method
    [TestInitialize]
    public void Setup()
    {
        // Create an in-memory database for testing and seed it with some data
        _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "SecurityServiceTestDb")
            .Options;
        _context = new ApplicationContext(_options);

        _context.Users.AddRange(new List<User>
        {
            new User
            {
                Id = 1,
                Username = "admin",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = "admin"
            },
            new User
            {
                Id = 2,
                Username = "user",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("user"),
                Role = "user"
            }
        });
        _context.SaveChanges();

        // Mock IConfiguration to return a key for JWT token generation
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(x => x["Jwt:Key"]).Returns("gerfwetgfdsregfsdvtgfbddsvfdgbfvsdrgfdvcs");
    }

    // This method is called after each test method
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task RegisterAsync_WhenUserDoesNotExist_Success()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);
        var registerOrLoginUserDto = new RegisterOrLoginUserDto
        {
            Username = "newuser",
            Password = "newpassword"
        };

        // Act
        var result = await securityService.RegisterAsync(registerOrLoginUserDto);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.AccessToken);
        Assert.IsNotNull(result.RefreshToken);
    }

    [TestMethod]
    public async Task RegisterAsync_WhenUserExists_Failure()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);
        var registerOrLoginUserDto = new RegisterOrLoginUserDto
        {
            Username = "admin",
            Password = "admin"
        };

        // Act
        var result = await securityService.RegisterAsync(registerOrLoginUserDto);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNull(result.AccessToken);
        Assert.IsNull(result.RefreshToken);
    }

    [TestMethod]
    public async Task LoginAsync_WhenUserExists_Success()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);
        var registerOrLoginUserDto = new RegisterOrLoginUserDto
        {
            Username = "admin",
            Password = "admin"
        };

        // Act
        var result = await securityService.LoginAsync(registerOrLoginUserDto);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.AccessToken);
        Assert.IsNotNull(result.RefreshToken);
    }

    [TestMethod]
    public async Task LoginAsync_WhenUserDoesNotExist_Failure()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);
        var registerOrLoginUserDto = new RegisterOrLoginUserDto
        {
            Username = "nonexistentuser",
            Password = "nonexistentpassword"
        };

        // Act
        var result = await securityService.LoginAsync(registerOrLoginUserDto);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNull(result.AccessToken);
        Assert.IsNull(result.RefreshToken);
    }

    [TestMethod]
    public async Task LoginAsync_WhenPasswordIsIncorrect_Failure()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);
        var registerOrLoginUserDto = new RegisterOrLoginUserDto
        {
            Username = "admin",
            Password = "incorrectpassword"
        };

        // Act
        var result = await securityService.LoginAsync(registerOrLoginUserDto);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNull(result.AccessToken);
        Assert.IsNull(result.RefreshToken);
    }

    [TestMethod]
    public async Task LoginAsync_WhenPasswordIsNull_ThrowException()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);
        var registerOrLoginUserDto = new RegisterOrLoginUserDto
        {
            Username = "admin",
            Password = null
        };

        // Act and Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => securityService.LoginAsync(registerOrLoginUserDto));
    }

    [TestMethod]
    public async Task RefreshTokenAsync_WhenUserExistsAndRefreshTokenIsValid_Success()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);

        var user = new User
        {
            Id = 3,
            Username = "testuser",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("testuserpassword"),
            Role = "user",
            RefreshToken = "somerandomrefreshtoken",
            RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await securityService.RefreshTokenAsync(user.RefreshToken);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.AccessToken);
        Assert.IsNotNull(result.RefreshToken);
    }

    [TestMethod]
    public async Task RefreshTokenAsync_WhenUserExistsAndRefreshTokenIsExpired_Failure()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);

        var user = new User
        {
            Id = 3,
            Username = "testuser",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("testuserpassword"),
            Role = "user",
            RefreshToken = "somerandomrefreshtoken",
            RefreshTokenExpiryTime = DateTime.Now.AddDays(-1)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await securityService.RefreshTokenAsync(user.RefreshToken);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNull(result.AccessToken);
        Assert.IsNull(result.RefreshToken);
    }

    [TestMethod]
    public async Task RefreshTokenAsync_WhenUserDoesNotExist_Failure()
    {
        // Arrange
        var securityService = new SecurityService(_context, _configurationMock.Object);

        // Act
        var result = await securityService.RefreshTokenAsync("nonexistentrefreshtoken");

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNull(result.AccessToken);
        Assert.IsNull(result.RefreshToken);
    }
}