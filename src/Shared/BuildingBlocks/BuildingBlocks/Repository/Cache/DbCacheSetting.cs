namespace BuildingBlocks.Repository.Cache;


#nullable disable
public class DbCacheSetting
{
    public bool Enabled { get; set; } = true;
    public string Connection { get; set; }
}