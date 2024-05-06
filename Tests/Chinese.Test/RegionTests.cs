using Chinese.Mainland;
using NStandard;
using System.Linq;
using Xunit;

namespace Chinese.Test;

public class RegionTests
{
    [Fact]
    public void Test1()
    {
        var result = Generator.Build();
        var country = result.Cities.First(x => x.Name == "北京市");
        Assert.Equal("东城区,西城区", country.Countries.Select(x => x.Name).Take(2).Join(","));
    }

    [Fact]
    public void Test2()
    {
        var result = Generator.Build();
        var country = result.Countries.First(x => x.Name == "长安区");
        Assert.Equal("石家庄市", country.City.Name);
        Assert.Equal("河北省", country.Province.Name);
    }
}
