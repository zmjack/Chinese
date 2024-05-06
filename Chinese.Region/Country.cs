namespace Chinese.Mainland;

public class Country : ICodeName
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required City City { get; set; }
    public Province Province => City.Province;
}
