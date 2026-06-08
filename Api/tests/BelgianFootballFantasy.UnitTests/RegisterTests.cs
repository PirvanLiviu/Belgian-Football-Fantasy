using Api.Dtos;
using Api.Repoistories;

public class RegisterTests
{
  [Fact]
  public async void RegisterTest()
  {
    var body = new RegisterPayload { Username = "liviu", Email = "liviu@liviu.com", Password = "Liviu013%", Formation = "4-3-3"};

    var res = await 
  }
}
