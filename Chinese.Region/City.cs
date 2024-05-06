namespace Chinese.Mainland;

public class City : ICodeName
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required Province Province { get; set; }
    public required ICollection<Country> Countries { get; set; }
}
