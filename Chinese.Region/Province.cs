namespace Chinese.Mainland;

public class Province : ICodeName
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required ICollection<City> Cities { get; set; }
}
