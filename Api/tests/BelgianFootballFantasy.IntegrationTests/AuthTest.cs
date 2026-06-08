using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Dtos;
using Api.Repoistories;

namespace Api.Tests;

public class AuthRepositoryTests : IDisposable
{
    // ──────────────────────────────────────────────
    // Infrastructure
    // ──────────────────────────────────────────────
    private readonly FootballFantasyDb _context;
    private readonly AuthRepository _sut;

    public AuthRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FootballFantasyDb>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // isolated per test class instance
            .Options;

        _context = new FootballFantasyDb(options);
        _sut = new AuthRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    // ──────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────

    /// <summary>Valid payload that should always pass validation.</summary>
    private static RegisterPayload ValidRegisterPayload(string username = "testuser") => new()
    {
        Username  = username,
        Password  = "ValidPass1!",   
        Formation = "4-3-3",
        Email = "test@user.com"
    };

    private static LoginPayload ValidLoginPayload(string email = "test@user.com") => new()
    {
        Email = email,
        Password = "ValidPass1!"
    };

    // ══════════════════════════════════════════════
    // Register – success path
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Register_WithValidPayload_ReturnsSuccess()
    {
        var result = await _sut.Register(ValidRegisterPayload());

        Assert.True(result.Success);
        Assert.Equal("User created", result.Message);
    }

    [Fact]
    public async Task Register_WithValidPayload_PersistsUserToDatabase()
    {
        await _sut.Register(ValidRegisterPayload("dbuser"));

        var userExists = await _context.Users.AnyAsync(u => u.Username == "dbuser");
        Assert.True(userExists);
    }

    [Fact]
    public async Task Register_WithValidPayload_StoresHashedPassword()
    {
        const string rawPassword = "ValidPass1!";
        await _sut.Register(ValidRegisterPayload() with { Password = rawPassword });

        var user = await _context.Users.FirstAsync(u => u.Username == "testuser");
        Assert.NotEqual(rawPassword, user.Password); // password must NOT be stored in plain text
        Assert.True(BCrypt.Net.BCrypt.Verify(rawPassword, user.Password));
    }

    // ══════════════════════════════════════════════
    // Register – duplicate username
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Register_WithDuplicateUsername_ReturnsFailure()
    {
        await _sut.Register(ValidRegisterPayload("dupeuser"));

        var result = await _sut.Register(ValidRegisterPayload("dupeuser"));

        Assert.False(result.Success);
        Assert.Equal("Username already exists", result.Message);
    }

    [Fact]
    public async Task Register_WithDuplicateUsername_DoesNotCreateSecondUser()
    {
        await _sut.Register(ValidRegisterPayload("dupeuser"));
        await _sut.Register(ValidRegisterPayload("dupeuser"));

        var count = await _context.Users.CountAsync(u => u.Username == "dupeuser");
        Assert.Equal(1, count);
    }

    // ══════════════════════════════════════════════
    // Register – validation failures
    // ══════════════════════════════════════════════

    [Theory]
    [InlineData("")]          // empty password
    [InlineData("short")]     // too short / weak
    [InlineData("alllowercase1")] // missing special char / uppercase – adjust to your rules
    public async Task Register_WithInvalidPassword_ReturnsValidationError(string badPassword)
    {
        var payload = ValidRegisterPayload() with { Password = badPassword };

        var result = await _sut.Register(payload);

        Assert.False(result.Success);
        Assert.NotNull(result.Message);
        Assert.NotEmpty(result.Message);
    }

    [Theory]
    [InlineData("")]           // empty formation
    [InlineData("9-9-9")]      // nonsensical formation – adjust to your validation rules
    public async Task Register_WithInvalidFormation_ReturnsValidationError(string badFormation)
    {
        var payload = ValidRegisterPayload() with { Formation = badFormation };

        var result = await _sut.Register(payload);

        Assert.False(result.Success);
        Assert.NotNull(result.Message);
    }

    [Fact]
    public async Task Register_WithInvalidPayload_DoesNotPersistUser()
    {
        var payload = ValidRegisterPayload() with { Password = "" };

        await _sut.Register(payload);

        var userExists = await _context.Users.AnyAsync(u => u.Username == payload.Username);
        Assert.False(userExists);
    }

    // ══════════════════════════════════════════════
    // Login – success path
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Login_WithCorrectCredentials_ReturnsSuccess()
    {
        await _sut.Register(ValidRegisterPayload());

        var result = await _sut.Login(ValidLoginPayload());

        Assert.True(result.Success);
    }

    [Fact]
    public async Task Login_WithCorrectCredentials_ReturnsLoginResponseData()
    {
        await _sut.Register(ValidRegisterPayload());

        var result = await _sut.Login(ValidLoginPayload());

        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task Login_WithCorrectCredentials_ReturnsTokenInData()
    {
        await _sut.Register(ValidRegisterPayload());

        var result = await _sut.Login(ValidLoginPayload());

        // Adjust the property name to whatever LoginResponse exposes (e.g. Token, Jwt, AccessToken)
        Assert.NotNull(result.Data!.Token);
        Assert.NotEmpty(result.Data.Token);
    }

    // ══════════════════════════════════════════════
    // Login – wrong password
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsFailure()
    {
        await _sut.Register(ValidRegisterPayload());

        var result = await _sut.Login(ValidLoginPayload() with { Password = "WrongPass99!" });

        Assert.False(result.Success);
        Assert.Null(result.Data);
    }

    // ══════════════════════════════════════════════
    // Login – non-existent user
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Login_WithNonExistentUsername_ReturnsFailure()
    {
        var result = await _sut.Login(ValidLoginPayload("ghostuser"));

        Assert.False(result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task Login_WithNonExistentUsername_ReturnsInformativeMessage()
    {
        var result = await _sut.Login(ValidLoginPayload("ghostuser"));

        Assert.NotNull(result.Message);
        Assert.NotEmpty(result.Message);
    }

    // ══════════════════════════════════════════════
    // Login – data not leaked between accounts
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Login_CannotLoginToOtherUsersAccount_WithOwnPassword()
    {
        await _sut.Register(ValidRegisterPayload("user_a") with { Password = "PasswordA1!" });
        await _sut.Register(ValidRegisterPayload("user_b") with { Password = "PasswordB2@" });

        // Try logging into user_a with user_b's password
        var result = await _sut.Login(new LoginPayload { Email = "userb@test.com", Password = "PasswordB2@" });

        Assert.False(result.Success);
    }
}
